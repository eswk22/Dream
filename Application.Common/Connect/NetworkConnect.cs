namespace ExecutionEngine.Common.Connect
{
    using InetAddresses = com.google.common.net.InetAddresses;
    using Log = com.resolve.util.Log;
    using StringUtils = com.resolve.util.StringUtils;
    using Application.Utility.Logging;
    using ExtendedResolver = org.xbill.DNS.ExtendedResolver;
    using Message = org.xbill.DNS.Message;
    using Name = org.xbill.DNS.Name;
    using Record = org.xbill.DNS.Record;
    using Resolver = org.xbill.DNS.Resolver;
    using ReverseMap = org.xbill.DNS.ReverseMap;
    using System.IO;

    public class NetworkConnect
    {
        private static ILogger _logger = new CrucialLogger();
        public static bool pingByIP(string hostIP, int timeout)
        {
            bool result = false;
            try
            {
                if (StringUtils.isBlank(hostIP))
                {
                    throw new ConnectException("Host IP must be provided");
                }
                if (timeout <= 0)
                {
                    throw new ConnectException("TIMEOUT must be a positive integer. Provided: " + timeout);
                }
                if (InetAddresses.isInetAddress(hostIP))
                {
                    InetAddress inet = InetAddresses.forString(hostIP);
                    _logger.Trace("Sending Ping Request to " + inet);
                    result = inet.isReachable(timeout * 1000);
                }
            }
            catch (UnknownHostException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            return result;
        }
        public static bool pingByName(string hostName, int timeout)
        {
            bool result = false;
            try
            {
                if (StringUtils.isBlank(hostName))
                {
                    throw new ConnectException("Host Name must be provided");
                }
                if (timeout <= 0)
                {
                    throw new ConnectException("TIMEOUT must be a positive integer. Provided: " + timeout);
                }
                InetAddress inet = InetAddress.getByName(hostName);
                _logger.Trace("Sending Ping Request to " + inet);
                result = inet.isReachable(timeout * 1000);
            }
            catch (UnknownHostException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            return result;
        }
        public static string reverseDNS(string hostIP)
        {
            string result = null;
            try
            {
                if (StringUtils.isBlank(hostIP))
                {
                    throw new ConnectException("Host IP must be provided");
                }
                Resolver resolver = new ExtendedResolver();
                Name name = ReverseMap.fromAddress(hostIP);
                int type = 12;
                int dclass = 1;
                Record rec = Record.newRecord(name, type, dclass);
                Message query = Message.newQuery(rec);
                Message response = resolver.send(query);
                Record[] answers = response.getSectionArray(1);
                if (answers.Length == 0)
                {
                    result = hostIP;
                }
                else
                {
                    result = answers[0].rdataToString();
                }
            }
            catch (UnknownHostException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            return result;
        }
    }
}