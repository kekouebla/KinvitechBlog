using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Models
{
    /// <summary>
    /// Represents message that is published by a publisher and captured by an interested subscriber
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageArgument<T> : EventArgs
    {
        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public T Message { get; private set; }

        /// <summary>
        /// Initializes the message
        /// </summary>
        /// <param name="message"></param>
        public MessageArgument(T message)
        {
            Message = message;
        }
    }
}
