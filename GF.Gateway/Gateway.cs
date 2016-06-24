// Copyright (c) Cragon. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GF.Gateway
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
        MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
        MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
        ServerBootstrap bootstrap = new ServerBootstrap();
        IChannel bootstrapChannel = null;

        // GatewaySettings.Port
        // "ClientConfiguration.xml"
        public async Task Start(string ip, int port, string orleansClientConfigFile)
        {
            //Console.WriteLine("Gateway Start, ThreadName=" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler(LogLevel.INFO))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new LengthFieldPrepender(2));
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast(new GatewayHandler());
                    }));

            bootstrapChannel = await bootstrap.BindAsync(port);

            GrainClient.Initialize(orleansClientConfigFile);
        }

        public async Task Stop()
        {
            try
            {
                GrainClient.Uninitialize();

                await bootstrapChannel.CloseAsync();
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }

        //Console.ReadLine();
        //static void Main(string[] args)
        //{
        //    RunServerAsync().Wait();
        //}
    }
}
