using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteEngine.Common
{
    /// <summary>
    /// Provides data provider agnostic queries and commands.
    /// </summary>
    public class DataAccessManager : IDisposable
    {
        #region IDisposable Implementation
        public void Dispose()
        {
            // Does nothing; included for future using statement support.
        }
        #endregion
        private DbProviderFactory ProviderFactory { get; set; }
        /// <summary>
        /// Gets or sets the external data store provider name (ex. System.Data.SqlClient).
        /// </summary>
        private string ProviderName { get; set; }
        /// <summary>
        /// Gets or sets the external data store connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        private DbConnectionStringBuilder ConnectionStringBuilder { get; set; }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="providerName">The data provider name (ex. System.Data.SqlClient).</param>
        /// <param name="connectionString">An appropriate connection string for the data provider.</param>
        public DataAccessManager(string providerName, string connectionString)
        {
            ProviderName = providerName;
            ConnectionString = connectionString;
            ProviderFactory = DbProviderFactories.GetFactory(ProviderName);
        }

        public DataAccessManager(string providerName)
        {
            ProviderName = providerName;
            ProviderFactory = DbProviderFactories.GetFactory(ProviderName);
        }

        /// <summary>
        /// Add Connection string parameters
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void AddConnectionStringParams(string Key,object Value)
        {
            if(ConnectionStringBuilder == null)
            {
                ConnectionStringBuilder = ProviderFactory.CreateConnectionStringBuilder();
            }
            ConnectionStringBuilder.Add(Key, Value);
        }

        #region Commands
        /// <summary>
        /// Selects a DataTable from the DbProvider.
        /// </summary>
        /// <param name="commandText">The select command text to execute.</param>
        /// <param name="args">Parameter definitions for the command.</param>
        /// <returns>A DataTable containing records selected from the DbProvider.</returns>
        public DataTable Select(string commandText, params DatabaseParameter[] args)
        {
            var result = new DataTable();
            try
            {
                using (var connection = GetConnection())
                {
                    using (var command = ProviderFactory.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = commandText;
                        foreach (var arg in args)
                        {
                            AddParameter(command, arg);
                        }
                        using (var adapter = ProviderFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = command;
                            adapter.Fill(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Provider: " + ProviderName + Environment.NewLine + "CommandText: " + commandText, ex);
            }
            return result;
        }
        /// <summary>
        /// Executes a non-query command on the DbProvider.
        /// </summary>
        /// <param name="commandText">The non-query command text to execute.</param>
        /// <param name="args">Parameter definitions for the command.</param>
        public void ExecuteCommand(string commandText, params DatabaseParameter[] args)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    using (var command = ProviderFactory.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = commandText;
                        foreach (var arg in args)
                        {
                            AddParameter(command, arg);
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Provider: " + ProviderName + Environment.NewLine + "CommandText: " + commandText, ex);
            }
        }
        #endregion
        #region Validation
        /// <summary>
        /// Sanitizes an identifier for the provider by quoting it.
        /// </summary>
        /// <param name="identifier">The identifier to sanitize.</param>
        /// <returns>The sanitized identifier.</returns>
        public string QuotedIdentifier(string identifier)
        {
            using (var builder = ProviderFactory.CreateCommandBuilder())
            {
                return builder.QuoteIdentifier(identifier);
            }
        }
        /// <summary>
        /// Checks a table name to verify that it exists in the DbProvider.
        /// </summary>
        /// <param name="tableName">Name of the table to verify.</param>
        /// <param name="isQuoted">True if the table name is already quoted.</param>
        /// <returns>True if the table exists, false otherwise.</returns>
        public bool TableExists(string tableName, bool isQuoted)
        {
            using (var connection = GetConnection())
            {
                using (var tables = connection.GetSchema("Tables"))
                {
                    string newTable = isQuoted ? tableName : QuotedIdentifier(tableName);
                    foreach (DataRow row in tables.Rows)
                    {
                        string existingTable = QuotedIdentifier(row["TABLE_NAME"].ToString());
                        if (existingTable == newTable)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
        #endregion
        #region Miscellaneous Helpers
        /// <summary>
        /// Retrieves an open connection from the provider factory.
        /// </summary>
        /// <returns>An open DbConnection.</returns>
        private DbConnection GetConnection()
        {
            var connection = ProviderFactory.CreateConnection();
            if (ConnectionString == null)
                ConnectionString = ConnectionStringBuilder.ConnectionString;
            connection.ConnectionString = ConnectionString;
            connection.Open();
            return connection;
        }
        /// <summary>
        /// Adds the provided database parameter to the provided command.
        /// </summary>
        /// <param name="command">The command the parameter will be added to.</param>
        /// <param name="parameter">The parameter settings.</param>
        private void AddParameter(DbCommand command, DatabaseParameter parameter)
        {
            var p = command.CreateParameter();
            p.ParameterName = parameter.Name;
            p.Value = parameter.Value;
            p.DbType = parameter.Type;
            command.Parameters.Add(p);
        }
        #endregion
    }

}
