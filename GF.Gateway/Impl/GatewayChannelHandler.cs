// Copyright (c) Cragon. All rights reserved.

namespace GF.Gateway
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using GF.Unity.Common;

    public class GatewayChannelHandler : ChannelHandlerAdapter
    {
        public GatewayRpcSession GatewayRpcSession { get; private set; }

        GatewaySessionHandler handler;

        public GatewayChannelHandler(GatewaySessionHandler handler)
        {
            this.handler = handler;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            EntityMgr entity_mgr = null;
            GatewayRpcSession = (GatewayRpcSession)Gateway.Instance.GatewayRpcSessionFactory.createRpcSession(entity_mgr);
            GatewayRpcSession.GatewaySessionHandler = this.handler;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            GatewayRpcSession.onRecvData(buffer.ToArray());

            //context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);

            context.CloseAsync();
        }
    }
}
