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

    public class GatewayHandler : ChannelHandlerAdapter
    {
        GatewayRpcSession GatewayRpcSession { get; set; }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("ChannelActive ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);

            EntityMgr entity_mgr = null;
            GatewayRpcSession = (GatewayRpcSession)Gateway.Instance.GatewayRpcSessionFactory.createRpcSession(entity_mgr);

            GatewayRpcSession.defRpcMethod<string>(100, _onRpcMethod1);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("ChannelInactive ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            Console.WriteLine("ChannelRegistered ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Console.WriteLine("ChannelUnregistered ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            GatewayRpcSession.onRecvData(buffer.ToArray());

            //if (buffer != null)
            //{
            //    Console.WriteLine("Received from client! ManagedThreadId=" + Thread.CurrentThread.ManagedThreadId);
            //    //Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
            //}

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

        void _onRpcMethod1(string info)
        {
            Console.WriteLine(info);
        }
    }
}
