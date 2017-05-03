using System;
using System.Collections;
namespace ExecutionEngine.Common.Connect
{
    using ARException = com.remedy.arsys.api.ARException;
    using ARServerUser = com.remedy.arsys.api.ARServerUser;
    using Entry = com.remedy.arsys.api.Entry;
    using EntryCriteria = com.remedy.arsys.api.EntryCriteria;
    using EntryFactory = com.remedy.arsys.api.EntryFactory;
    using EntryID = com.remedy.arsys.api.EntryID;
    using EntryItem = com.remedy.arsys.api.EntryItem;
    using EntryKey = com.remedy.arsys.api.EntryKey;
    using EntryListCriteria = com.remedy.arsys.api.EntryListCriteria;
    using EntryListInfo = com.remedy.arsys.api.EntryListInfo;
    using Field = com.remedy.arsys.api.Field;
    using FieldCriteria = com.remedy.arsys.api.FieldCriteria;
    using FieldFactory = com.remedy.arsys.api.FieldFactory;
    using NameID = com.remedy.arsys.api.NameID;
    using QualifierInfo = com.remedy.arsys.api.QualifierInfo;
    using StatusInfo = com.remedy.arsys.api.StatusInfo;
    using Log = com.resolve.util.Log;
    using StringUtils = com.resolve.util.StringUtils;
    using Application.Utility.Logging;
    public class RemedyConnect
    {
        private ILogger _logger = new CrucialLogger();
        internal ARServerUser session = null;
        internal static bool debug = false;
        public RemedyConnect(string host, string username, string password)
        {   
            connect(host, 0, username, password);
        }
        public RemedyConnect(string host, int port, string username, string password)
        {   
            connect(host, port, username, password);
        }
        public static bool Debug
        {
            set
            {     
                value = value;
            }
        }
        public virtual void connect(string host, int port, string username, string password)
        {  
            if ((!string.ReferenceEquals(host, null)) && (!string.ReferenceEquals(username, null)) && (!string.ReferenceEquals(password, null)))
            {  
                _logger.Debug("Connecting to AR Server at: " + host);
                this.session = new ARServerUser();
                this.session.Server = host;
                if (port != 0)
                {   
                    this.session.Port = port;
                } 
                this.session.User = new com.remedy.arsys.api.AccessNameID(username);
                this.session.Password = new com.remedy.arsys.api.AccessNameID(password);
                try
                {  
                    this.session.verifyUser(new com.remedy.arsys.api.VerifyUserCriteria());
                }
                catch (ARException e)
                {   
                    _logger.Warn("Error verifying user: " + e);
                    this.session.clear();
                    this.session = null;
                }
                _logger.Debug("Connected to AR Server.");
            }
        }
        public virtual void close()
        {   
            if (this.session != null)
            {   
                this.session.clear();
            }
            _logger.Info("AR System objects closed");
        }
        public virtual string create(string form, string fieldValueStr)
        {   
            IDictionary fieldValues = StringUtils.stringToMap(fieldValueStr, "=", "&");
             return create(form, fieldValues);
        }
        public virtual string create(string form, IDictionary fieldValues)
        {   
            string result = "";
            try
            {   
                Entry entry = (Entry)EntryFactory.Factory.newInstance();
                entry.Context = this.session;
                entry.SchemaID = new NameID(form);
                setEntryItemValues(entry, fieldValues);
                entry.create();
                result = entry.EntryID.ToString();
                EntryFactory.Factory.releaseInstance(entry);
                _logger.Trace("Created entry id: " + result);
            }
            catch (ARException e)
            {   ARExceptionHandler(e, "Problem while creating entry: ");
            }
            return result;
        }
        private void setEntryItemValues(Entry entry, IDictionary fieldValues)
        {   
            ArrayList entries = new ArrayList();
            for (IEnumerator i = fieldValues.SetOfKeyValuePairs().GetEnumerator(); i.MoveNext();)
            {
                DictionaryEntry rec = (DictionaryEntry)i.Current;
                string key = (string)rec.Key;
                string value = (string)rec.Value;
                if (StringUtils.isNumeric(key))
                {  
                    entries.Add(new EntryItem(new com.remedy.arsys.api.FieldID(long.Parse(key)), new com.remedy.arsys.api.Value(value)));
                }
            }
            EntryItem[] entryItems = new EntryItem[entries.Count];
            entries.toArray(entryItems);
            entry.EntryItems = entryItems;
        }
        public virtual string update(string form, string entryId, string fieldValueStr)
        {   
            IDictionary fieldValues = StringUtils.stringToMap(fieldValueStr, "=", "&");
            return update(form, entryId, fieldValues);
        }
        public virtual string update(string form, string entryId, IDictionary fieldValues)
        {  
            string result = null;
            try
            {  
                EntryKey entryKey = new EntryKey(new NameID(form), new EntryID(entryId));
                Entry entry = EntryFactory.findByKey(this.session, entryKey, null);
                setEntryItemValues(entry, fieldValues);
                entry.store();
                EntryFactory.Factory.releaseInstance(entry);
                result = entryId;
                _logger.Debug("Record: " + entryId + " successfully updated");
            }
            catch (ARException e)
            { 
                ARExceptionHandler(e, "Problem while modifying record: ");
            }
            return result;
        }
         public virtual IList find(string form, string criteria)
        {   /* 191 */
            IList result = new ArrayList();
            try
            {   /* 197 */
                QualifierInfo qualifier = prepareQualifierInfo(form, criteria);
                /* 198 */
                if (qualifier != null)
                {
                    /* 201 */
                    EntryListCriteria listCriteria = new EntryListCriteria();
                    /* 202 */
                    listCriteria.SchemaID = new NameID(form);
                    /* 203 */
                    listCriteria.Qualifier = qualifier;
                    /* 206 */
                    int? nMatches = new int?(0);
                    /* 207 */
                    EntryListInfo[] entryInfo = EntryFactory.findEntryListInfos(this.session, listCriteria, null, false, nMatches);
                    /* 209 */
                           _logger.Trace("Query returned " + nMatches + " matches.");
                    if (nMatches.Value > 0)
                    {
                        /* 218 */
                        for (int i = 0; i < entryInfo.Length; i++)
                        {   /* 220 */
                            result.Add(entryInfo[i].EntryID.ToString());
                            /* 221 */
                                  _logger.Trace(entryInfo[i].EntryID.ToString() + ":" + new string(entryInfo[i].Description));
                        }
                    }   /* 227 */
                    EntryFactory.Factory.releaseInstance(entryInfo);
                }
            }
            catch (ARException e)
            {   /* 232 */
                ARExceptionHandler(e, "Problem while querying by qualifier: ");
            }
            /* 235 */
            return result;
        }
        internal virtual QualifierInfo prepareQualifierInfo(string form, string criteria)
        {   /* 240 */
            QualifierInfo result = null;
            /* 243 */
            FieldCriteria fCrit = new FieldCriteria();
            /* 244 */
            fCrit.RetrieveAll = true;
            /* 247 */
            com.remedy.arsys.api.FieldListCriteria fListCrit = new com.remedy.arsys.api.FieldListCriteria(new NameID(form), new com.remedy.arsys.api.Timestamp(0L), 511);
            /* 250 */
            Field[] formFields = FieldFactory.findObjects(this.session, fListCrit, fCrit);
            /* 253 */
            result = com.remedy.arsys.api.Util.ARGetQualifier(this.session, criteria, formFields, null, 0);
            /* 256 */
            FieldFactory.Factory.releaseInstance(formFields);
            /* 258 */
            return result;
        }
         public virtual IDictionary retrieve(string form, string entryId)
        {   
            return retrieve(form, entryId, null);
        }
         public virtual IDictionary retrieve(string form, string entryId, string fieldList)
        {   /* 268 */
            IDictionary result = new Hashtable();
            /* 270 */
            _logger.Debug("Retrieving record with entry ID:" + entryId);
            try
            {   /* 275 */
                EntryCriteria entryCriteria = null;
                /* 276 */
                if (!string.ReferenceEquals(fieldList, null))
                {   /* 278 */
                    bool hasFields = false;
                    /* 280 */
                    string[] fieldIds = fieldList.Split("&", true);
                    /* 282 */
                    com.remedy.arsys.api.EntryListFieldInfo[] entryListFieldList = new com.remedy.arsys.api.EntryListFieldInfo[fieldIds.Length];
                    /* 283 */
                    for (int i = 0; i < fieldIds.Length; i++)
                    {   /* 285 */
                        string id = fieldIds[i];
                        /* 287 */
                        if (!StringUtils.isEmpty(id))
                        {   /* 289 */
                            long? fieldId = Convert.ToInt64(long.Parse(id.Trim()));
                            /* 290 */
                            entryListFieldList[i] = new com.remedy.arsys.api.EntryListFieldInfo(new com.remedy.arsys.api.FieldID(fieldId.Value));
                            /* 292 */
                            hasFields = true;
                        }
                    }
                    /* 297 */
                    if (hasFields)
                    {   /* 299 */
                        entryCriteria = new EntryCriteria();
                        /* 300 */
                        entryCriteria.EntryListFieldInfo = entryListFieldList;
                    }
                }
                /* 305 */
                EntryKey entryKey = new EntryKey(new NameID(form), new EntryID(entryId));
                /* 306 */
                Entry entry = EntryFactory.findByKey(this.session, entryKey, entryCriteria);
                /* 308 */
                EntryItem[] entryItemList = entry.EntryItems;
                /* 309 */
                if (entryItemList == null)
                {   /* 311 */
                    _logger.Debug("No data found for entry ID:" + entryId);
                }
                else
                {   /* 315 */
                    FieldCriteria fCrit = new FieldCriteria();
                    /* 316 */
                    fCrit.RetrieveAll = true;
                    /* 318 */
                    _logger.Trace("Number of fields: " + entryItemList.Length);
                    /* 319 */
                    for (int i = 0; i < entryItemList.Length; i++)
                    {
                        /* 322 */
                        com.remedy.arsys.api.FieldID id = entryItemList[i].FieldID;
                        /* 323 */
                        Field field = FieldFactory.findByKey(this.session, new com.remedy.arsys.api.FieldKey(new NameID(form), id), fCrit);
                        /* 326 */
                        string value = entryItemList[i].Value.ToString();
                        /* 327 */
                        if (string.ReferenceEquals(value, null))
                        {   /* 329 */
                            value = "";
                        }   /* 331 */
                        else if (!StringUtils.isAsciiPrintable(value))
                        {   /* 333 */
                            value = "value not printable";
                        }   /* 335 */
                        result[field.Name.ToString()] = value;
                        /* 337 */
                        _logger.Trace(field.Name.ToString());
                        /* 338 */
                        _logger.Trace(": " + entryItemList[i].Value);
                        /* 339 */
                        _logger.Trace(" ID: " + id);
                        /* 340 */
                        _logger.Trace(" Field type: " + field.DataType.toInt());
                        /* 341 */
                        _logger.Trace("");
                        /* 356 */
                        FieldFactory.Factory.releaseInstance(field);
                    }
                }   /* 359 */
                EntryFactory.Factory.releaseInstance(entry);
            }
            catch (ARException e)
            {   /* 363 */
                ARExceptionHandler(e, "Problem while querying by entry id: ");
            }
            /* 366 */
            return result;
        }
       public virtual string print(string form, string entryId)
        {   /* 371 */
            return print(form, entryId, null);
        }
        public virtual string print(string form, string entryId, string fieldList)
        {   /* 376 */
            string result = "";
            /* 378 */
            IDictionary entry = retrieve(form, entryId, fieldList);
            /* 380 */
            for (IEnumerator i = entry.SetOfKeyValuePairs().GetEnumerator(); i.MoveNext();)
            {   /* 382 */
                DictionaryEntry field = (DictionaryEntry)i.Current;
                /* 383 */
                string key = (string)field.Key;
                /* 384 */
                string value = (string)field.Value;
                /* 386 */
                result = result + key + "=" + value + "\n";
            }   /* 388 */
            result = result + "\n\n";
            /* 390 */
            return result;
        }
        public virtual string delete(string form, string entryId)
        {   /* 395 */
            string result = null;
            try
            {   /* 400 */
                EntryKey entryKey = new EntryKey(new NameID(form), new EntryID(entryId));
                /* 401 */
                Entry entry = EntryFactory.findByKey(this.session, entryKey, null);
                /* 404 */
                entry.remove();
                /* 405 */
                EntryFactory.Factory.releaseInstance(entry);
                /* 408 */
                result = entryId;
                /* 410 */
                _logger.Trace("Record: " + entryId + " successfully deleted");
            }
            catch (ARException e)
            {   /* 414 */
                ARExceptionHandler(e, "Problem while querying by entry id: ");
            }
            /* 417 */
            return result;
        }
        public virtual void printStatusList(StatusInfo[] statusList)
        {   /* 422 */
            if ((statusList == null) || (statusList.Length == 0))
            {   /* 424 */
                _logger.Trace("Status List is empty.");
                /* 425 */
                return;
            }
            /* 428 */
            _logger.Debug("Message type: ");
            /* 429 */
            switch (statusList[0].MessageType)
            {
                case 0:
                    /* 432 */
                    _logger.Trace("Note");
                    /* 433 */
                    break;
                case 1:
                    /* 435 */
                    _logger.Trace("Warning");
                    /* 436 */
                    break;
                case 2:
                    /* 438 */
                    _logger.Trace("Error");
                    /* 439 */
                    break;
                case 3:
                    /* 441 */
                    _logger.Trace("Fatal Error");
                    /* 442 */
                    break;
                default:
                    /* 444 */
                    _logger.Trace("Unknown (" + statusList[0].MessageType + ")");
                    break;
            }
            /* 448 */
            _logger.Trace("Status List:");
            /* 449 */
            for (int i = 0; i < statusList.Length; i++)
            {   /* 451 */
                _logger.Trace(statusList[i].MessageText);
                /* 452 */
                _logger.Trace(statusList[i].AppendedText);
            }
        }
        public virtual IDictionary createFieldValues(string log)
        {  
            IDictionary result = new Hashtable();
            Pattern pattern = Pattern.compile(".*\\((\\d+)\\) = (.+)");
            string[] lines = log.Split("\n", true);
            for (int i = 0; i < lines.Length; i++)
            {   
                string line = lines[i];
                Matcher matcher = pattern.matcher(line);
                if (matcher.matches())
                {   
                    string key = matcher.group(1);
                    string value = matcher.group(2);
                    if (!StringUtils.isEmpty(key))
                    {   
                        if (value.Equals("\"\""))
                        {   
                            value = "";
                        }   
                        result[key] = value;
                    }
                }
            }   
            _logger.Trace("fieldValues: " + result);
            return result;
        }
        public virtual void ARExceptionHandler(ARException e, string errMessage)
        {  
            _logger.Error("ARExceptionHandler: " + errMessage, e);
            throw e;
        }
    }   
}