// Copyright (c) Cragon. All rights reserved.

namespace GF.Gateway
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Codecs;
    using DotNetty.Common.Internal.Logging;
    using DotNetty.Handlers.Logging;
    using DotNetty.Handlers.Tls;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;
    using Orleans;
    using GF.Unity.Common;

    public abstract class SessionHandler
    {
        public RpcSession RpcSession { get; set; }

        public abstract void OnDefRpcMethod();

        public abstract void OnSocketError(object rec, SocketErrorEventArgs args);

        public abstract void OnSocketConnected(object client, EventArgs args);

        public abstract void OnSocketClosed(object client, EventArgs args);
    }

    public abstract class SessionHandlerFactory
    {
        public abstract SessionHandler CreateSessionHandler();
    }
}
