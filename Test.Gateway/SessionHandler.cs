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

    public class SessionHandler : GatewaySessionHandler
    {
        public override void OnDefRpcMethod()
        {
            RpcSession.defRpcMethod<string>(100, _onRpcMethod1);
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

        void _onRpcMethod1(string info)
        {
            Console.WriteLine(info);
        }
    }
}
