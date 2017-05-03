namespace ExecutionEngine.Common.Connect
{
    public class DBHost
    {
        public string host;
        public int port;
        public DBHost(string hostName, int defaultPort)
        {
            this.host = hostName;
            this.port = defaultPort;
        }
    }
}