using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzej.BLL
{
    public class ValidationException : Exception
    {
        public List<string> ValidationErrors { get; }

        public ValidationException()
        {
            ValidationErrors = new List<string>();
        }

        public ValidationException(string message) : base(message)
        {
            ValidationErrors = new List<string>();
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            ValidationErrors = new List<string>();
        }

        public ValidationException(IEnumerable<string> validationErrors)
        {
            ValidationErrors = new List<string>(validationErrors);
        }

        public ValidationException(string message, IEnumerable<string> validationErrors) : base(message)
        {
            ValidationErrors = new List<string>(validationErrors);
        }

        public ValidationException(string message, Exception innerException, IEnumerable<string> validationErrors) : base(message, innerException)
        {
            ValidationErrors = new List<string>(validationErrors);
        }
    }

}
