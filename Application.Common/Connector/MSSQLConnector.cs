using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteEngine.Common
{
    public class MSSQLConnector : DataAccessManager
    {
      

        private static readonly string _ProviderName = "System.Data.SqlClient";
       
    
        public MSSQLConnector(string ConnectionString) : base(_ProviderName,ConnectionString)
        {
        }

        public MSSQLConnector(string HostIP,int Port) : base(_ProviderName)
        {
            base.AddConnectionStringParams("Data Source", HostIP + "," + Port);
        }
        public MSSQLConnector(string HostIP, int Port,string Username,string Password) : base(_ProviderName)
        {
            base.AddConnectionStringParams("Data Source", HostIP + "," + Port);
            base.AddConnectionStringParams("User ID", Username);
            base.AddConnectionStringParams("Password", Password);
        }
        public MSSQLConnector() : base(_ProviderName)
        {
        }


    }
}
