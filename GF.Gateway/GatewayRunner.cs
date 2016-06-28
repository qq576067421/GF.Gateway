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

    public class GatewayRunner
    {
        //GatewayRpcSessionFactory GatewayRpcSessionFactory
        MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup(4);
        MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup(4);
        ServerBootstrap bootstrap = new ServerBootstrap();
        IChannel bootstrapChannel = null;

        public async Task Start(IPAddress ip_address, int port, string orleansClientConfigFile)
        {
            bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler(LogLevel.INFO))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new LengthFieldPrepender(
                            ByteOrder.LittleEndian, 2, 0, false));
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(
                            ByteOrder.LittleEndian, ushort.MaxValue, 0, 2, 0, 2, true));
                        pipeline.AddLast(new GatewayHandler());
                    }));

            bootstrapChannel = await bootstrap.BindAsync(ip_address, port);

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
    }
}
