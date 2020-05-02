using Kinvitech.Services.Interfaces;
using Kinvitech.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services
{
    /// <summary>
    /// Provides event data publisher.  Subscribers attach themselves to this event to listen to message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Publisher<T> : IPublisher<T>
    {
        /// <summary>
        /// Defined data publisher event
        /// </summary>
        public event EventHandler<MessageArgument<T>> DataPublisher;

        /// <summary>
        /// Publishes data to subscribers
        /// </summary>
        /// <param name="data"></param>
        public void PublishData(T data)
        {
            MessageArgument<T> message = (MessageArgument<T>)Activator.CreateInstance(typeof(MessageArgument<T>), new object[] { data });
            OnDataPublisher(message);
        }

        /// <summary>
        /// Handles publish data, if the handler is not null
        /// </summary>
        /// <param name="args"></param>
        private void OnDataPublisher(MessageArgument<T> args)
        {
            DataPublisher?.Invoke(this, args);
        }
    }
}
