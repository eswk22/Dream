using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace ExecutionEngine.Common.Connect
{
    using IntellidenException = com.intelliden.common.IntellidenException;
    using ApiFactory = com.intelliden.icos.api.ApiFactory;
    using ApiSession = com.intelliden.icos.api.ApiSession;
    using ResourceDataFactory = com.intelliden.icos.api.resources.ResourceDataFactory;
    using ResourceManager = com.intelliden.icos.api.resources.ResourceManager;
    using WorkflowDataFactory = com.intelliden.icos.api.workflow.WorkflowDataFactory;
    using WorkflowManager = com.intelliden.icos.api.workflow.WorkflowManager;
    using Command = com.intelliden.icos.idc.Command;
    using CommandWork = com.intelliden.icos.idc.CommandWork;
    using DataAccessRight = com.intelliden.icos.idc.DataAccessRight;
    using NativeCommandSet = com.intelliden.icos.idc.NativeCommandSet;
    using NativeCommandSetCommand = com.intelliden.icos.idc.NativeCommandSetCommand;
    using NetworkResourceKey = com.intelliden.icos.idc.NetworkResourceKey;
    using Resource = com.intelliden.icos.idc.Resource;
    using ResourceContainerDescriptor = com.intelliden.icos.idc.ResourceContainerDescriptor;
    using ResourceContentDescriptor = com.intelliden.icos.idc.ResourceContentDescriptor;
    using VtmosCriterion = com.intelliden.icos.idc.VtmosCriterion;
    using Work = com.intelliden.icos.idc.Work;
    using WorkKey = com.intelliden.icos.idc.WorkKey;
    using Log = com.resolve.util.Log;
    using Application.Utility.Logging;
    public class ITNCMConnect : Connect
    {
        private static ILogger _logger = new CrucialLogger();   
        private static readonly string CNAME = typeof(ITNCMConnect).FullName;
        /*  57 */
        internal readonly int execWindow = 14400000;
        internal string user;
        internal string password;
        internal string server;
        internal int port;
        /*  65 */
        internal ApiSession session = null;
        //ORIGINAL LINE: public ITNCMConnect(String user, String password, String server, int port) throws Exception
        public ITNCMConnect(string user, string password, string server, int port)
        {   /*  69 */
            this.user = user;
            /*  70 */
            this.password = password;
            /*  71 */
            this.server = server;
            /*  72 */
            this.port = port;
            /*  74 */
            this.session = Session;
        }
        public virtual void connect()
        {   /*  79 */
            this.session = Session;
        }
        //ORIGINAL LINE: public com.intelliden.icos.api.ApiSession getSession() throws Exception
        public virtual ApiSession Session
        {
            get
            {
                this.session = ApiFactory.getSession(this.user, this.password, this.server, this.port);
                return this.session;
            }
        }
        public virtual void close()
        {
            try
            {   /*  92 */
                this.session.disconnect();
            }
            catch (Exception e)
            {   /*  96 */
                _logger.Error(e.Message, e);
            }
        }
        //ORIGINAL LINE: public String submitAndWaitNCS(String deviceName, String ncsName, String ncsRealm, boolean searchSubRealms, java.util.Map params, String description) throws Exception
        public virtual string submitAndWaitNCS(string deviceName, string ncsName, string ncsRealm, bool searchSubRealms, IDictionary @params, string description)
        {   /* 112 */
            string uowId = null;
            /* 114 */
            uowId = submitNCS(deviceName, ncsName, ncsRealm, searchSubRealms, @params, description);
            /* 115 */
            waitForUOW(uowId);
            /* 117 */
            return uowId;
        }
        //ORIGINAL LINE: public String submitNCS(String deviceName, String ncsName, String ncsRealm, boolean searchSubRealms, java.util.Map params, String description) throws Exception
        public virtual string submitNCS(string deviceName, string ncsName, string ncsRealm, bool searchSubRealms, IDictionary @params, string description)
        {   /* 124 */
            NativeCommandSet ncs = (NativeCommandSet)getResource(this.session, ncsName, ncsRealm, searchSubRealms, "NativeCommandSet", null);
            /* 125 */
            ResourceContentDescriptor nrDesc = getNetworkResourceDescriptor(deviceName, this.session);
            /* 126 */
            string result = submitNCS(this.session, (NetworkResourceKey)nrDesc.ResourceKey, ncs, @params, description);
            /* 129 */
            return result;
        }
        public virtual Work pollWork(string uowId)
        {   /* 134 */
            waitForUOW(uowId);
            /* 135 */
            return getWork(uowId);
        }
        public virtual void waitForUOW(string uowId)
        {   /* 140 */
            CountDownLatch latch = new CountDownLatch(1);
            /* 141 */
            Thread poller = new Thread(new ITNCMUoWPoller(this.session, uowId, latch));
            /* 142 */
            poller.Start();
            try
            {   /* 145 */
                latch.await();
            }
            catch (ThreadInterruptedException)
            {
            }
        }
        //ORIGINAL LINE: public String submitNCS(com.intelliden.icos.api.ApiSession session, String deviceName, String ncsName, String ncsRealm, boolean searchSubrealms, java.util.Map params, String uowDescription) throws Exception
        public virtual string submitNCS(ApiSession session, string deviceName, string ncsName, string ncsRealm, bool searchSubrealms, IDictionary @params, string uowDescription)
        {   /* 189 */
            ResourceContentDescriptor nrDesc = getNetworkResourceDescriptor(deviceName, session);
            /* 190 */
            if (nrDesc == null)
            {   /* 192 */
                throw new Exception("Network Resource was not found: " + deviceName);
            }   /* 194 */
            NetworkResourceKey nrKey = (NetworkResourceKey)nrDesc.ResourceKey;
            /* 197 */
            string resourceType = "NativeCommandSet";
            /* 198 */
            VtmosCriterion vtmos = null;
            /* 199 */
            NativeCommandSet ncs = (NativeCommandSet)getResource(session, ncsName, ncsRealm, searchSubrealms, resourceType, vtmos);
            /* 200 */
            if (ncs == null)
            {   /* 202 */
                throw new Exception("Native Command Set was not found: " + ncsName);
            }
            /* 205 */
            Properties paramProps = new Properties();
            /* 206 */
            paramProps.putAll(@params);
            /* 209 */
            ncs.applyParameters(paramProps);
            /* 213 */
            NativeCommandSetCommand command = session.resourceManager().dataFactory().newNativeCommandSetCommand();
            /* 214 */
            List<NetworkResourceKey> nrKeys = new ArrayList();
            /* 215 */
            nrKeys.Add(nrKey);
            /* 216 */
            command.Keys = nrKeys;
            /* 217 */
            command.addCommandSet(ncs);
            /* 220 */
            command.Comment = uowDescription;
            /* 231 */
            return executeCommand(session, command);
        }
        //ORIGINAL LINE: public String submitNCS(com.intelliden.icos.api.ApiSession session, com.intelliden.icos.idc.NetworkResourceKey nrKey, com.intelliden.icos.idc.NativeCommandSet ncs, java.util.Map params, String uowDescription) throws Exception
        public virtual string submitNCS(ApiSession session, NetworkResourceKey nrKey, NativeCommandSet ncs, IDictionary @params, string uowDescription)
        {   /* 264 */
            if (@params != null)
            {   /* 266 */
                Properties paramProps = new Properties();
                /* 267 */
                paramProps.putAll(@params);
                /* 268 */
                ncs.applyParameters(paramProps);
            }
            /* 273 */
            NativeCommandSetCommand command = session.resourceManager().dataFactory().newNativeCommandSetCommand();
            /* 274 */
            List<NetworkResourceKey> nrKeys = new ArrayList();
            /* 275 */
            nrKeys.Add(nrKey);
            /* 276 */
            command.Keys = nrKeys;
            /* 277 */
            command.addCommandSet(ncs);
            /* 280 */
            command.Comment = uowDescription;
            /* 291 */
            return executeCommand(session, command);
        }
        //ORIGINAL LINE: public String submitNCS(com.intelliden.icos.api.ApiSession session, String deviceName, com.intelliden.icos.idc.NativeCommandSet ncs, java.util.Map params, String uowDescription) throws Exception
        public virtual string submitNCS(ApiSession session, string deviceName, NativeCommandSet ncs, IDictionary @params, string uowDescription)
        {   /* 325 */
            ResourceContentDescriptor nrDesc = getNetworkResourceDescriptor(deviceName, session);
            /* 326 */
            if (nrDesc == null)
            {   /* 328 */
                throw new Exception("Network Resource was not found: " + deviceName);
            }   /* 330 */
            NetworkResourceKey nrKey = (NetworkResourceKey)nrDesc.ResourceKey;
            /* 333 */
            if (@params != null)
            {   /* 335 */
                Properties paramProps = new Properties();
                /* 336 */
                paramProps.putAll(@params);
                /* 338 */
                ncs.applyParameters(paramProps);
            }
            /* 343 */
            NativeCommandSetCommand command = session.resourceManager().dataFactory().newNativeCommandSetCommand();
            /* 344 */
            List<NetworkResourceKey> nrKeys = new ArrayList();
            /* 345 */
            nrKeys.Add(nrKey);
            /* 346 */
            command.Keys = nrKeys;
            /* 347 */
            command.addCommandSet(ncs);
            /* 350 */
            command.Comment = uowDescription;
            /* 361 */
            return executeCommand(session, command);
        }
        //ORIGINAL LINE: protected String submitUow(com.intelliden.icos.api.ApiSession session, com.intelliden.icos.idc.Command command, String uowDescription) throws Exception
        protected internal virtual string submitUow(ApiSession session, Command command, string uowDescription)
        {   /* 382 */
            WorkflowManager wfManager = session.workflowManager();
            /* 388 */
            CommandWork work = wfManager.dataFactory().newCommandWork();
            /* 397 */
            work.Command = command;
            /* 398 */
            work.Comment = uowDescription;
            /* 399 */
            work.ExecutionWindowStartTime = DateTime.Now;
            /* 400 */
            work.ExecutionWindowEndTime = new DateTime(DateTimeHelperClass.CurrentUnixTimeMillis() + 14400000L);
            /* 406 */
            bool overrideUowConflicts = true;
            /* 407 */
            WorkKey key = wfManager.submit(work, overrideUowConflicts);
            /* 411 */
            return key.FriendlyName;
        }
        //ORIGINAL LINE: public String executeCommand(com.intelliden.icos.api.ApiSession session, com.intelliden.icos.idc.Command command) throws com.intelliden.common.IntellidenException
        public virtual string executeCommand(ApiSession session, Command command)
        {   /* 440 */
            bool overrideUowConflicts = true;
            /* 450 */
            Command submittedCommand = session.workflowManager().executeCommandNonBlocking(command, overrideUowConflicts);
            /* 454 */
            long uowId = submittedCommand.ExecutionId;
            /* 456 */
            return "" + uowId;
        }
        //ORIGINAL LINE: public static com.intelliden.icos.idc.Resource getResource(com.intelliden.icos.api.ApiSession apisession, String resourceName, String realm, boolean searchSubrealms, String resourceType, com.intelliden.icos.idc.VtmosCriterion vtmos) throws com.intelliden.common.IntellidenException
        public static Resource getResource(ApiSession apisession, string resourceName, string realm, bool searchSubrealms, string resourceType, VtmosCriterion vtmos)
        {   /* 492 */
            string MNAME = CNAME + ".getResource(): ";
            /* 494 */
            Resource resource = null;
            /* 495 */
            ResourceManager resourceManager = apisession.resourceManager();
            /* 497 */
            ICollection<ResourceContentDescriptor> descriptors = getResourceDescriptors(apisession, resourceName, realm, searchSubrealms, resourceType, vtmos);
            /* 500 */
            if ((null != descriptors) && (descriptors.Count > 0))
            {
                /* 504 */
                if (descriptors.Count > 1)
                {   /* 506 */
                    _logger.Error(MNAME + "More than one resource was returned in the  search for " + resourceName,null);
                }
                /* 510 */
                ResourceContentDescriptor descriptor = (ResourceContentDescriptor)descriptors.GetEnumerator().next();
                /* 511 */
                resource = resourceManager.getResource(descriptor);
            }
            /* 514 */
            return resource;
        }
        //ORIGINAL LINE: public static java.util.Collection<com.intelliden.icos.idc.ResourceContentDescriptor> getResourceDescriptors(com.intelliden.icos.api.ApiSession apisession, String resourceName, String realm, boolean searchSubrealms, String resourceType, com.intelliden.icos.idc.VtmosCriterion vtmos) throws com.intelliden.common.IntellidenException
        public static ICollection<ResourceContentDescriptor> getResourceDescriptors(ApiSession apisession, string resourceName, string realm, bool searchSubrealms, string resourceType, VtmosCriterion vtmos)
        {   /* 544 */
            string MNAME = CNAME + ".getResourceDescriptors(): ";
            /* 546 */
            ResourceManager resourceManager = apisession.resourceManager();
            /* 549 */
            ResourceContainerDescriptor realmDescriptor = null;
            /* 551 */
            if (null == realm)
            {
                try
                {   /* 555 */
                    realmDescriptor = resourceManager.RootContainer;
                }
                catch (IntellidenException ie)
                {   /* 559 */
                    _logger.Error(MNAME + "Exception getting the ResourceContainerDescriptor" + " for the root realm");
                    /* 560 */
                    throw ie;
                }
            }
            else
            {
                try
                {   /* 567 */
                    realmDescriptor = resourceManager.getContainer(realm);
                }
                catch (IntellidenException ie)
                {   /* 571 */
                    _logger.Error(MNAME + "Exception getting the ResourceContainerDescriptor" + " for the realm " + realm);
                    /* 572 */
                    throw ie;
                }
            }   /* 575 */
            if (null == realmDescriptor)
            {   /* 577 */
                string message = "A null ResourceContainerDescriptor  was returned for the realm " + realm;
                /* 578 */
                _logger.Error(MNAME + message);
                /* 579 */
                throw new IntellidenException(message);
            }
            /* 584 */
            int accessMask = DataAccessRight.VIEW.Id;
            /* 586 */
            ICollection<ResourceContentDescriptor> descriptors = resourceManager.search(realmDescriptor.Key, searchSubrealms, resourceType, resourceName, vtmos, accessMask, false, false, false, null);
            /* 588 */
            return descriptors;
        }
        //ORIGINAL LINE: public static com.intelliden.icos.idc.ResourceContentDescriptor getNetworkResourceDescriptor(String resName, com.intelliden.icos.api.ApiSession session) throws com.intelliden.common.IntellidenException
        public static ResourceContentDescriptor getNetworkResourceDescriptor(string resName, ApiSession session)
        {   /* 605 */
            string MNAME = CNAME + ".getNetworkResourceDescriptor(): ";
            /* 607 */
            ResourceContentDescriptor desc = null;
            /* 608 */
            ICollection<ResourceContentDescriptor> descriptors = getResourceDescriptors(session, resName, null, true, "NetworkResource", null);
            /* 610 */
            if ((null != descriptors) && (descriptors.Count > 0))
            {   /* 612 */
                if (descriptors.Count > 1)
                {   /* 614 */
                    _logger.Error(MNAME + "The search for the NetworkResource " + resName + " returned more than one result.  Will " + " use the first value in the returned Collection");
                }   /* 616 */
                IEnumerator<ResourceContentDescriptor> iter = descriptors.GetEnumerator();
                /* 617 *///JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                if (iter.MoveNext())
                {   /* 619 *///JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                    desc = (ResourceContentDescriptor)iter.Current;
                }
            }
            /* 623 */
            return desc;
        }
        public virtual Work getWork(string uowId)
        {   /* 631 */
            Work result = null;
            try
            {   /* 635 */
                WorkflowManager wfManager = this.session.workflowManager();
                /* 636 */
                Work work = null;
                try
                {   /* 639 */
                    result = wfManager.getWork(uowId);
                           _logger.Trace("Work State: " + work.State + " Execute Status: " + work.ExecutionStatus);
                }
                catch (IntellidenException ie)
                {   /* 647 */
                    _logger.Error("Error fetching UOW " + uowId + " from the ITNCM server");
                    /* 648 */
                    throw ie;
                }
            }
            catch (Exception e)
            {   /* 653 */
                _logger.Error(e.Message, e);
            }
            /* 656 */
            return result;
        }
        public virtual string getWorkLog(string uowId)
        {   /* 661 */
            string result = null;
            try
            {   /* 665 */
                WorkflowManager wfManager = this.session.workflowManager();
                /* 666 */
                result = new string(wfManager.getWorkLog(uowId));
            }
            catch (Exception e)
            {   /* 670 */
                _logger.Error(e.Message, e);
            }
            /* 673 */
            return result;
        }
        public virtual string execCmdSet(string device, string cmdSet, string cmdSetRealm, IDictionary @params, string comment)
        {   /* 682 */
            string uowId = null;
            try
            {   /* 686 */
                uowId = submitNCS(this.session, device, cmdSet, cmdSetRealm, false, @params, comment);
            }
            catch (Exception e)
            {   /* 690 */
                _logger.Error(e.Message, e);
            }
            /* 693 */
            return uowId;
        }
        public virtual IDictionary<string, IList<string>> listCmdSet(string startRealm)
        {   /* 701 */
            IDictionary<string, IList<string>> result = new Hashtable();
            try
            {   /* 705 */
                ICollection<NativeCommandSet> cmdSets = collectCommandSets(this.session, startRealm, false);
                /* 707 */
                foreach (NativeCommandSet cmdSet in cmdSets)
                {   /* 709 */
                    string name = cmdSet.Name;
                    /* 712 */
                    IList<string> paramNames = extractParams(cmdSet);
                    /* 720 */
                    result[name] = paramNames;
                }
            }
            catch (Exception e)
            {   /* 725 */
                _logger.Error(e.Message, e);
            }
            /* 728 */
            return result;
        }
        //ORIGINAL LINE: public java.util.Collection<com.intelliden.icos.idc.NativeCommandSet> collectCommandSets(com.intelliden.icos.api.ApiSession session, String startingRealm, String cmdSetName, com.intelliden.icos.idc.VtmosCriterion vtmos, boolean searchSubrealms) throws com.intelliden.common.IntellidenException
        public virtual ICollection<NativeCommandSet> collectCommandSets(ApiSession session, string startingRealm, string cmdSetName, VtmosCriterion vtmos, bool searchSubrealms)
        {   /* 764 */
            ICollection<NativeCommandSet> cmdSets = new ArrayList();
            /* 765 */
            string type = "NativeCommandSet";
            /* 767 */
            ICollection<ResourceContentDescriptor> ncsDescs = getResourceDescriptors(session, cmdSetName, startingRealm, searchSubrealms, type, vtmos);
            ResourceManager rm;
            /* 769 */
            if ((ncsDescs != null) && (ncsDescs.Count > 0))
            {   /* 771 */
                rm = session.resourceManager();
                /* 772 */
                foreach (ResourceContentDescriptor desc in ncsDescs)
                {   /* 774 */
                    NativeCommandSet ncs = (NativeCommandSet)rm.getResource(desc);
                    /* 775 */
                    cmdSets.Add(ncs);
                }
            }
            /* 779 */
            return cmdSets;
        }
        //ORIGINAL LINE: public java.util.Collection<com.intelliden.icos.idc.NativeCommandSet> collectCommandSets(com.intelliden.icos.api.ApiSession session, String startingRealm, com.intelliden.icos.idc.VtmosCriterion vtmos, boolean searchSubrealms) throws com.intelliden.common.IntellidenException
        public virtual ICollection<NativeCommandSet> collectCommandSets(ApiSession session, string startingRealm, VtmosCriterion vtmos, bool searchSubrealms)
        {   /* 808 */
            string cmdSetName = null;
            /* 809 */
            return collectCommandSets(session, startingRealm, cmdSetName, vtmos, searchSubrealms);
        }
        //ORIGINAL LINE: public java.util.Collection<com.intelliden.icos.idc.NativeCommandSet> collectCommandSets(com.intelliden.icos.api.ApiSession session, String startingRealm, boolean searchSubrealms) throws com.intelliden.common.IntellidenException
        public virtual ICollection<NativeCommandSet> collectCommandSets(ApiSession session, string startingRealm, bool searchSubrealms)
        {   /* 834 */
            string cmdSetName = null;
            /* 835 */
            VtmosCriterion vtmos = null;
            /* 836 */
            return collectCommandSets(session, startingRealm, cmdSetName, vtmos, searchSubrealms);
        }
        //ORIGINAL LINE: public java.util.Collection<com.intelliden.icos.idc.NativeCommandSet> collectCommandSets(com.intelliden.icos.api.ApiSession session, String startingRealm, String cmdSetName, boolean searchSubrealms) throws com.intelliden.common.IntellidenException
        public virtual ICollection<NativeCommandSet> collectCommandSets(ApiSession session, string startingRealm, string cmdSetName, bool searchSubrealms)
        {   /* 859 */
            VtmosCriterion vtmos = null;
            /* 860 */
            return collectCommandSets(session, startingRealm, cmdSetName, vtmos, searchSubrealms);
        }
        //ORIGINAL LINE: public java.util.Collection<com.intelliden.icos.idc.NativeCommandSet> collectCommandSets(com.intelliden.icos.api.ApiSession session, String startingRealm) throws com.intelliden.common.IntellidenException
        public virtual ICollection<NativeCommandSet> collectCommandSets(ApiSession session, string startingRealm)
        {   /* 880 */
            string cmdSetName = null;
            /* 881 */
            VtmosCriterion vtmos = null;
            /* 882 */
            bool searchSubrealms = true;
            /* 883 */
            return collectCommandSets(session, startingRealm, cmdSetName, vtmos, searchSubrealms);
        }
        //ORIGINAL LINE: public static java.util.List<String> extractParams(com.intelliden.icos.idc.NativeCommandSet ncs) throws Exception
        public static IList<string> extractParams(NativeCommandSet ncs)
        {   /* 900 */
            List<string> paramNames = new ArrayList();
            /* 902 */
            string ncsCli = ncs.Commands;
            /* 903 */
            List<string> paramValues = new ArrayList();
            /* 905 */
            getParameters(ncsCli, paramNames, paramValues);
            /* 907 */
            return paramNames;
        }
        //ORIGINAL LINE: public static void getParameters(String string, java.util.ArrayList<String> keys, java.util.ArrayList<String> values) throws Exception
        public static void getParameters(string @string, List<string> keys, List<string> values)
        {   /* 913 */
            if (!string.ReferenceEquals(@string, null))
            {
                /* 916 */
                int firstToken = @string.IndexOf("$", StringComparison.Ordinal);
                /* 917 */
                bool valid = false;
                /* 918 */
                while ((!valid) && (firstToken > -1))
                {   /* 920 */
                    if (firstToken > 0)
                    {   /* 922 */
                        if (@string[firstToken - 1] == '\\')
                        {
                            /* 925 */
                            firstToken = @string.IndexOf('$', firstToken + 1);
                        }
                        else
                        {   /* 929 */
                            valid = true;
                        }
                    }
                    else
                    {
                        /* 934 */
                        valid = true;
                    }
                }   /* 937 */
                if (firstToken == -1)
                {
                    /* 940 */
                    return;
                }
                /* 943 */
                int secondToken = @string.IndexOf("$", firstToken + 1, StringComparison.Ordinal);
                /* 944 */
                valid = false;
                /* 945 */
                while ((!valid) && (secondToken > -1))
                {   /* 947 */
                    if (@string[secondToken - 1] == '\\')
                    {
                        /* 950 */
                        secondToken = @string.IndexOf('$', secondToken + 1);
                    }
                    else
                    {   /* 954 */
                        valid = true;
                    }
                }   /* 957 */
                if (secondToken == -1)
                {   /* 959 */
                    throw new Exception("$ mismatch in this native command set: " + @string);
                }
                /* 962 */
                addParameter(@string.Substring(firstToken + 1, secondToken - (firstToken + 1)), keys, values);
                /* 964 */
                getParameters(@string.Substring(secondToken + 1), keys, values);
            }
        }
        public static void addParameter(string propString, List<string> keys, List<string> values)
        {   /* 970 */
            string property = null;
            /* 971 */
            string value = null;
            /* 973 */
            if (!string.ReferenceEquals(propString, null))
            {   /* 975 */
                int equals = propString.IndexOf("=", StringComparison.Ordinal);
                /* 976 */
                if (equals != -1)
                {   /* 978 */
                    property = propString.Substring(0, equals);
                    /* 980 */
                    property = property.Trim();
                    /* 981 */
                    value = propString.Substring(equals + 1);
                }
                else
                {   /* 985 */
                    property = propString.Trim();
                }   /* 987 */
                keys.Add(property);
                /* 988 */
                values.Add(value);
            }
        }
    }
}