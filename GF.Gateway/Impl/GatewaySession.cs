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
        private SessionHandlerFactory factory;
        private Dictionary<IChannelHandlerContext, SessionHandler> mapChannel
            = new Dictionary<IChannelHandlerContext, SessionHandler>();

        public GatewaySession(EntityMgr entity_mgr)
        {
            //mSocket.OnSocketReceive += _onSocketReceive;
            //mSocket.OnSocketConnected += _onSocketConnected;
            //mSocket.OnSocketClosed += _onSocketClosed;
            //mSocket.OnSocketError += _onSocketError;
        }

        public void Init(SessionHandlerFactory factory)
        {
            this.factory = factory;
        }

        public void ChannelActive(IChannelHandlerContext context)
        {
            var handler = this.factory.CreateSessionHandler();
            mapChannel[context] = handler;

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
            //if (mSocket != null)
            //{
            //    mSocket.send(method_id, data);
            //}

            //context.WriteAsync(message);
        }

        public override void close()
        {
            //if (mSocket != null) mSocket.close();
        }

        public override void update(float elapsed_tm)
        {
        }

        public void onRecvData(byte[] data)
        {
            _onSocketReceive(data, data.Length);
        }

        void _onSocketReceive(byte[] data, int len)
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
