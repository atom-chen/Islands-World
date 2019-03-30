/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  socke,封装c# socketNumber据传输协议
  *Others:  
  *History:
*********************************************************************************
*/
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Text;
using System.Threading;

namespace Coolape
{
    public delegate void NetCallback(USocket socket, object obj);
    public delegate void OnReceiveCallback(USocket socket, byte[] bytes, int len);

    public class USocket
    {
        public string host;
        public int port;
        public Socket mSocket;
        public static int mTmpBufferSize = 64 * 1024;
        public byte[] mTmpBuffer = null;
        private IPEndPoint ipe;
        private NetCallback onConnectStateChgCallback;
        private OnReceiveCallback onReceiveCallback;
        private int failTimes = 0;
        private bool isClosed = false;
        public bool isActive = false;
        public Timer timeoutCheckTimer;

        public USocket(string host, int port)
        {
            init(host, port);
        }

        public void init(string host, int port)
        {
            isClosed = false;
            this.host = host;
            this.port = port;
            IPAddress ip = null;
            try
            {
                // is ip xx.xx.xx.xx
                ip = IPAddress.Parse(host);
            }
            catch (Exception e)
            {
                // may be is dns
                try
                {
                    IPAddress[] address = Dns.GetHostAddresses(host);
                    if (address.Length > 0)
                    {
                        ip = address[0];
                    }
                }
                catch (Exception e2)
                {
                    Debug.LogError(e2);
                    return;
                }
            }
            if (ip == null)
            {
                Debug.LogError("Get ip is null");
                return;
            }
            ipe = new IPEndPoint(ip, port);

            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                Debug.LogWarning("is ipv6");
                mSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                Debug.LogWarning("is ipv4");
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            if (mTmpBuffer == null)
            {
                mTmpBuffer = new byte[mTmpBufferSize];
            }
        }

        // 异步模式//////////////////////////////////////////////////////////////////
        // 异步模式//////////////////////////////////////////////////////////////////
        //public bool IsConnectionSuccessful = false;
        public int timeoutMSec = 5000; //毫秒
        public int maxTimeoutTimes = 3;
        Timer connectTimeout = null;

        public void connectAsync(NetCallback onConnectStateChgCallback)
        {
            try
            {
                isActive = false;
                this.onConnectStateChgCallback = onConnectStateChgCallback;
                if (ipe == null)
                {
                    onConnectStateChg(false);
                    return;
                }

                mSocket.BeginConnect(ipe, (AsyncCallback)connectCallback, this);
                if (connectTimeout != null)
                {
                    connectTimeout.Dispose();
                    connectTimeout = null;
                }
                connectTimeout = TimerEx.schedule((TimerCallback)connectTimeOut, null, timeoutMSec);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void connectTimeOut(object orgs)
        {
            if (isActive)
            {
            }
            else
            {
                onConnectStateChg(false);
            }
        }

        protected void onConnectStateChg(bool connected)
        {
            if (isClosed)
            {
                return;
            }
            if (!connected && isActive)
            {
                close();
            }
            if (onConnectStateChgCallback != null)
            {
                onConnectStateChgCallback(this, connected);
            }
        }

        private void connectCallback(IAsyncResult ar)
        {
            // 从stateobject获取socket.
            USocket client = (USocket)ar.AsyncState;
            if (client.mSocket.Connected)
            {
                // 完成连接.
                client.mSocket.EndConnect(ar);
                client.isActive = true;
                client.failTimes = 0;
                client.onConnectStateChg(true);
            }
            else
            {
                client.onConnectStateChg(false);
            }
            if (connectTimeout != null)
            {
                connectTimeout.Dispose();
                connectTimeout = null;
            }
        }

        public void close()
        {
            try
            {
                isClosed = true;
                isActive = false;
                failTimes = 0;
                if (mSocket != null)
                {
                    mSocket.Close();
                }
                mSocket = null;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void ReceiveAsync(OnReceiveCallback callback)
        {
            onReceiveCallback = callback;
            ReceiveAsync();
        }

        private void ReceiveAsync()
        {
            try
            {
                // 从远程target接收Number据.
                mSocket.BeginReceive(mTmpBuffer, 0, mTmpBufferSize, 0,
                    (AsyncCallback)ReceiveCallback, this);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            USocket client = (USocket)ar.AsyncState;
            try
            {
                if (client.timeoutCheckTimer != null)
                {
                    client.timeoutCheckTimer.Dispose();
                    client.timeoutCheckTimer = null;
                }
                if (client.isActive)
                {
                    client.failTimes = 0;
                    //从远程设备读取Number据
                    int bytesRead = client.mSocket.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        //Debug.Log("receive len==" + bytesRead);
                        // 有Number据，存储.
                        onReceiveCallback(client, client.mTmpBuffer, bytesRead);
                    }
                    else if (bytesRead < 0)
                    {
                        client.onConnectStateChg(false);
                    }
                    else
                    {
                        // 所有Number据读取完毕.
                        Debug.Log("receive zero=====" + bytesRead);
                        client.onConnectStateChg(false);
                        return;
                    }

                    // 继续读取.
                    if (client.mSocket.Connected)
                    {
                        client.ReceiveAsync();
                    }
                }
                else
                {
                    client.onConnectStateChg(false);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void SendAsync(byte[] data)
        {
            try
            {
                if (data == null)
                    return;
                // isOpen始发送Number据到远程设备.
                if (this.timeoutCheckTimer == null)
                {
                    this.timeoutCheckTimer = TimerEx.schedule((TimerCallback)sendTimeOut, null, timeoutMSec);
                }

                mSocket.BeginSend(data, 0, data.Length, 0,
                    (AsyncCallback)SendCallback, this);
            }
            catch (Exception e)
            {
                Debug.LogError("socket:" + e);
                onConnectStateChg(false);
            }
        }

        public void sendTimeOut(object orgs)
        {
            failTimes++;
            if (failTimes > maxTimeoutTimes)
            {
                onConnectStateChg(false);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            USocket client = (USocket)ar.AsyncState;
            // 完成Number据发送.
            int bytesSent = client.mSocket.EndSend(ar);
            if (bytesSent <= 0) //发送失败
            {
                onConnectStateChg(false);
            }
            client.failTimes = 0;
        }

    }
}