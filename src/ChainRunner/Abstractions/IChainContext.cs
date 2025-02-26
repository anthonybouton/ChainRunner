using System;
using System.Collections;
using System.Collections.Generic;

namespace ChainRunner
{
    public interface IChainContext
    {
        /// <summary>
        /// Gets the data available on this chain.
        /// </summary>
        IChainDataCollection Data { get; }
    }
}