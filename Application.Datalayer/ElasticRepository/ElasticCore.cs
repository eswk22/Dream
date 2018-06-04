using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAL.ElasticRepository
{
    public class ElasticCore : IElasticCore
    {

        public string connectionString;

        readonly Lazy<ElasticClient> _client;

        public ElasticCore()
        {
            connectionString = GetDefaultConnectionString();
            _client = new Lazy<ElasticClient>(getSession);
        }
        public ElasticCore(string ConnectionString)
        {
            connectionString = ConnectionString;
            _client = new Lazy<ElasticClient>(getSession);
        }
        public ElasticClient Client
        {
            get
            {
                return _client.Value;
            }
        }
        private const string DefaultConnectionstringName = "ElasticServersSettings";

        public static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DefaultConnectionstringName].ConnectionString;
        }



        /// <summary>
        /// Providing automatic failover support
        /// </summary>
        /// <returns></returns>
        ElasticClient getSession()
        {
            var nodes = new List<Uri>();

            foreach (var uri in connectionString.Split(',')) //write in web.config as "http://machine1:9200, http://machine2:9200, http://machine3:9200"
            {
                nodes.Add(new Uri(uri));
            }

            var setting = new ConnectionSettings(new SniffingConnectionPool(nodes));//new Uri(connectionString.Value));

            return new ElasticClient(setting);
        }

    }//End of class MXSearchClient
}
