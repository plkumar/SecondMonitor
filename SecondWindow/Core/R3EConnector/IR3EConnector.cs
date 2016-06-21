﻿using SecondWindow.Core.R3EConnector.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecondWindow.Core.R3EConnector
{
    public class R3EDataEventArgs : EventArgs
    {
        
        public R3EDataEventArgs(R3ESharedData data)
        {
            this.Data = data;
        }

        public R3ESharedData Data
        {
            get;
            set;
        }
    }

    public interface IR3EConnector
    {
        event EventHandler<R3EDataEventArgs> DataLoaded;
        event EventHandler<EventArgs> ConnectedEvent;
        event EventHandler<EventArgs> Disconnected;

        bool IsConnected
        {
            get;            
        }
        int TickTime
        {
            get;
            set;
        }
        bool TryConnect();
        void AsynConnect();

    }
}