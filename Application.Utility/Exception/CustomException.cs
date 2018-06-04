using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class CustomException : System.Exception
    {
        private int _key;
        private string _message; 
        public int? ReferenceKey
        {
            get
            {
                return _key;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _message;
            }
        }
        public CustomException()
        {
        }

        public CustomException(string message)
        : base(message)
        {
            _message = message;
        }

        public CustomException(int key, string message)
       : base(message)
        {
            _key = key;
            _message = message;
        }

        public CustomException(string title, string message)
       : base(message)
        {
            _message = message;
        }

        public CustomException(string message, Exception inner)
        : base(message, inner)
        {
            _message = message;
        }
    }
}
