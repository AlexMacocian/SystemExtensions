using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions
{
    /// <summary>
    /// Custom implementation of an exception.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of ItemNotFoundException.
        /// </summary>
        public ItemNotFoundException()
        {

        }
        /// <summary>
        /// Initializes a new instance of ItemNotFoundException with a specified error.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ItemNotFoundException(string message) : base(message)
        {

        }
        /// <summary>
        /// Initializes a new instance of ItemNotFoundException with a specified error and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null in case there is no inner exception.</param>
        public ItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
