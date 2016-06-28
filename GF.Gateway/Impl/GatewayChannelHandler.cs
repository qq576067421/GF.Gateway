// Copyright (c) Cragon. All rights reserved.

namespace GF.Gateway
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using GF.Unity.Common;

    public class GatewayChannelHandler : ChannelHandlerAdapter
    {
        private SessionHandlerFactory factory;
        private Dictionary<IChannelHandlerContext, GatewaySession> mapSession
            = new Dictionary<IChannelHandlerContext, GatewaySession>();

        public GatewayChannelHandler(SessionHandlerFactory factory)
        {
            this.factory = factory;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            var handler = this.factory.CreateSessionHandler();
            var session = (GatewaySession)Gateway.Instance.GatewaySessionFactory.createRpcSession(null);

            session.ChannelActive(context, handler);

            mapSession[context] = session;
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

            GatewaySession session = null;
            if (mapSession.TryGetValue(context, out session))
            {
                session.onRecvData(buffer.ToArray());
            }
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
