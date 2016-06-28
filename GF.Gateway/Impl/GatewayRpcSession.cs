// Copyright (c) Cragon. All rights reserved.

namespace GF.Gateway
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using GF.Unity.Common;

    public class GatewayRpcSession : RpcSession
    {
        private GatewaySessionHandler value;

        public GatewaySessionHandler GatewaySessionHandler
        {
            get { return value; }
            set
            {
                value.RpcSession = this;
                value.OnDefRpcMethod();
            }
        }

        public GatewayRpcSession(EntityMgr entity_mgr)
        {
            //mSocket.OnSocketReceive += _onSocketReceive;
            //mSocket.OnSocketConnected += _onSocketConnected;
            //mSocket.OnSocketClosed += _onSocketClosed;
            //mSocket.OnSocketError += _onSocketError;
        }

        public override bool isConnect()
        {
            return true;

            //return mSocket.IsConnected;
        }

        public override void connect(string ip, int port)
        {
            //if (mSocket != null) mSocket.connect(ip, port);
        }

        public override void send(ushort method_id, byte[] data)
        {
            //if (mSocket != null)
            //{
            //    mSocket.send(method_id, data);
            //}
        }

        public override void close()
        {
            //if (mSocket != null) mSocket.close();
        }

        public override void update(float elapsed_tm)
        {
            //if (mSocket != null) mSocket.update(elapsed_tm);
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

    public class GatewayRpcSessionFactory : RpcSessionFactory
    {
        public override RpcSession createRpcSession(EntityMgr entity_mgr)
        {
            return new GatewayRpcSession(entity_mgr);
        }
    }
}
