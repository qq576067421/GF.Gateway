// Copyright (c) Cragon. All rights reserved.

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
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using GF.Gateway;

    class Program
    {
        static async Task RunGatewayAsync()
        {
            EbLog.NoteCallback = Console.WriteLine;
            EbLog.WarningCallback = Console.WriteLine;
            EbLog.ErrorCallback = Console.WriteLine;

            IPAddress host = IPAddress.Parse("192.168.0.10");
            int port = 5882;
            Gateway gateway = new Gateway();
            GatewaySessionHandlerFactory factory = new GatewaySessionHandlerFactory();
            await gateway.Start(host, port, "ClientConfiguration.xml", factory);

            Console.WriteLine("Gateway Start ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("按回车键退出。。。");
            Console.ReadLine();

            await gateway.Stop();

            Console.WriteLine("Gateway Stop");
        }

        static void Main() => RunGatewayAsync().Wait();
    }
}
