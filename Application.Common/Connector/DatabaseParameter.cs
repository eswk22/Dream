using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteEngine.Common
{
    /// <summary>
    /// Represents parameter information for a command into the DataAccessManager.
    /// </summary>
    public class DatabaseParameter
    {
        /// <summary>
        /// Gets or sets the SQL command parameter name of the parameter.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Gets or sets the expected type of the parameter value.
        /// </summary>
        public DbType Type { get; set; }
        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="name">The name of the database parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <remarks>
        /// The expected type is defaulted, which may create performance problems with implict typing in the database.
        /// </remarks>
        public DatabaseParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="name">The name of the database parameter (eg. @MyParameter or :MyParameter)</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="type">The expected type of the parameter.</param>
        public DatabaseParameter(string name, object value, DbType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }

}
