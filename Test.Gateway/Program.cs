// Copyright (c) Cragon. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Test.Gateway
{
    using DotNetty.Codecs;
    using DotNetty.Common.Internal.Logging;
    using DotNetty.Handlers.Tls;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using GF.Gateway;

    class Program
    {
        static async Task RunGatewayAsync()
        {
            Console.WriteLine("Gateway Start, ThreadName=" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            IPAddress host = IPAddress.Parse("127.0.0.1");
            int port = 5882;
            Gateway gateway = new Gateway();
            await gateway.Start("localhost", port, "ClientConfiguration.xml");

            Console.ReadLine();

            await gateway.Stop();
        }

        static void Main() => RunGatewayAsync().Wait();
    }
}
