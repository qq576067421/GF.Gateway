// Copyright (c) Cragon. All rights reserved.

namespace Test.Gateway
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
    using GF.Unity.Common;
    using GF.Gateway;

    public class GatewaySessionHandler : SessionHandler
    {
        public override void OnDefRpcMethod()
        {
            RpcSession.defRpcMethod<int, string>(2, _onRpcMethod1);
        }

        public override void OnSocketError(object rec, SocketErrorEventArgs args)
        {
        }

        public override void OnSocketConnected(object client, EventArgs args)
        {
        }

        public override void OnSocketClosed(object client, EventArgs args)
        {
        }

        void _onRpcMethod1(int v, string info)
        {
            Console.WriteLine(v);
            Console.WriteLine(info);

            this.RpcSession.rpc(1, 200, "Hi");
        }
    }

    public class GatewaySessionHandlerFactory : SessionHandlerFactory
    {
        public override SessionHandler CreateSessionHandler()
        {
            return new GatewaySessionHandler();
        }
    }
}
