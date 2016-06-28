// Copyright (c) Cragon. All rights reserved.

namespace GF.Gateway
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using GF.Unity.Common;

    public class GatewaySession : RpcSession
    {
        private IChannelHandlerContext context;
        private SessionHandler handler;

        public GatewaySession(EntityMgr entity_mgr)
        {
            //mSocket.OnSocketConnected += _onSocketConnected;
            //mSocket.OnSocketClosed += _onSocketClosed;
            //mSocket.OnSocketError += _onSocketError;
        }

        public void ChannelActive(IChannelHandlerContext context, SessionHandler handler)
        {
            this.context = context;
            this.handler = handler;

            handler.RpcSession = this;
            handler.OnDefRpcMethod();
        }

        public void ChannelInactive(IChannelHandlerContext context)
        {
        }

        public override bool isConnect()
        {
            return true;
        }

        public override void connect(string ip, int port)
        {
        }

        public override void send(ushort method_id, byte[] data)
        {
            IByteBuffer msg = PooledByteBufferAllocator.Default.Buffer(256);
            msg.WriteBytes(BitConverter.GetBytes(method_id));
            if (data != null) msg.WriteBytes(data);

            context.WriteAndFlushAsync(msg);
        }

        public override void close()
        {
            context.CloseAsync();
        }

        public override void update(float elapsed_tm)
        {
        }

        public void onRecvData(byte[] data)
        {
            ushort method_id = BitConverter.ToUInt16(data, 0);

            byte[] buf = null;
            if (data.Length > sizeof(ushort))
            {
                ushort data_len = (ushort)(data.Length - sizeof(ushort));
                buf = new byte[data_len];
                Array.Copy(data, sizeof(ushort), buf, 0, data_len);
            }
            else buf = new byte[0];

            onRpcMethod(method_id, buf);
        }

        void _onSocketError(object rec, SocketErrorEventArgs args)
        {
            if (OnSocketError != null)
            {
                OnSocketError(this, args);
            }
        }

        void _onSocketConnected(object client, EventArgs args)
        {
            if (OnSocketConnected != null)
            {
                OnSocketConnected(this, args);
            }
        }

        void _onSocketClosed(object client, EventArgs args)
        {
            if (OnSocketClosed != null)
            {
                OnSocketClosed(this, args);
            }
        }
    }

    public class GatewaySessionFactory : RpcSessionFactory
    {
        public override RpcSession createRpcSession(EntityMgr entity_mgr)
        {
            return new GatewaySession(entity_mgr);
        }
    }
}
