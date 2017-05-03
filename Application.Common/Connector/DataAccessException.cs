using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;

namespace ExecuteEngine.Common
{


        /// <summary>
        /// Thrown when a data access request fails.
        /// </summary>
        [Serializable]
        internal class DataAccessException : Exception
        {
            /// <summary>
            /// Creates a new instance.
            /// </summary>
            public DataAccessException() { }
            /// <summary>
            /// Creates a new instance initialized with the specified message string.
            /// </summary>
            /// <param name="message">The error message that explains the reason for the exception.</param>
            public DataAccessException(string message) : base(message) { }
            /// <summary>
            /// Creates a new instance initialized with the specified message string and inner exception.
            /// </summary>
            /// <param name="message">The error message that explains the reason for the exception.</param>
            /// <param name="inner">The exception that is the cause of the current exception.</param>
            public DataAccessException(string message, Exception inner) : base(message, inner) { }
            /// <summary>
            /// Creates a new instance initialized with serialization data.
            /// </summary>
            /// <param name="info">The object that holds the serialized object data.</param>
            /// <param name="context">The contextual information about the source or destination.</param>
            protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }

}
