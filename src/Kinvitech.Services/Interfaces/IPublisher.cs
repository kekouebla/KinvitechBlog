using Kinvitech.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Interfaces
{
    /// <summary>
    /// Defines a publihser responsible for publishing message of different types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPublisher<T>
    {
        /// <summary>
        /// Method that will handle an event when the evvent provides data
        /// </summary>
        event EventHandler<MessageArgument<T>> DataPublisher;

        /// <summary>
        /// Method to publish data to subscribers
        /// </summary>
        /// <param name="data"></param>
        void PublishData(T data);
    }
}
