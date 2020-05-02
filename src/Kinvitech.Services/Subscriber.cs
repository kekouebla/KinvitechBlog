using Kinvitech.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services
{
    /// <summary>
    /// Implements the subscriber for capturing the interested message type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Subscriber<T>
    {
        /// <summary>
        /// Gets or sets the publisher of the interested message type
        /// </summary>
        public IPublisher<T> Publisher { get; private set; }

        /// <summary>
        /// Initializes the subscriber with the publisher
        /// </summary>
        /// <param name="publisher"></param>
        public Subscriber(IPublisher<T> publisher)
        {
            Publisher = publisher;
        }
    }
}
