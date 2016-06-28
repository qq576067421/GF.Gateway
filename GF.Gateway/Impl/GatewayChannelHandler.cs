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
        private GatewaySession session;

        public GatewayChannelHandler(SessionHandlerFactory factory)
        {
            EntityMgr entity_mgr = null;
            this.session = (GatewaySession)Gateway.Instance.GatewaySessionFactory.createRpcSession(entity_mgr);
            this.session.Init(factory);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            this.session.ChannelActive(context);
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
            this.session.onRecvData(buffer.ToArray());
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
