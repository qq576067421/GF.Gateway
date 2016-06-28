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

    public class Gateway
    {
        public GatewayRpcSessionFactory GatewayRpcSessionFactory { get; private set; }
        public static Gateway Instance { get; private set; }

        GatewayRunner GatewayRunner = new GatewayRunner();

        public Gateway()
        {
            Instance = this;
            GatewayRpcSessionFactory = new GatewayRpcSessionFactory();
        }

        public Task Start(IPAddress ip_address, int port, string orleansClientConfigFile)
        {
            return GatewayRunner.Start(ip_address, port, orleansClientConfigFile);
        }

        public Task Stop()
        {
            return GatewayRunner.Stop();
        }
    }
}
