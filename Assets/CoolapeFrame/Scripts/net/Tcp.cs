/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  tcp
  *Others:  
  *History:
*********************************************************************************
*/

using UnityEngine;
using System.Collections;
using System.IO;
using XLua;

namespace Coolape
{
    public delegate void TcpDispatchDelegate(object data, Tcp tcp);
    public class Tcp : MonoBehaviour
    {
        public string host;
        public int port;
        public bool connected = false;
        public bool serializeInMainThread = true;
        //是否连接
        public bool isStopping = false;
        const int MaxReConnectTimes = 0;
        public static int __maxLen = 1024 * 1024;

        System.Threading.Timer timer;
        public USocket socket;
        int reConnectTimes = 0;
        public const string CONST_Connect = "connectCallback";
        public const string CONST_OutofNetConnect = "outofNetConnect";
        TcpDispatchDelegate mDispatcher;
        byte[] tmpBuffer = new byte[__maxLen];

        public virtual void init(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        /// <summary>
        /// Init the specified host, port and dispatcher.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <param name="port">Port.</param>
        /// <param name="dispatcher">Dispatcher,当接收到数据并解析成功后将调用该方法.</param>
        public virtual void init(string host, int port, TcpDispatchDelegate dispatcher)
        {
            mDispatcher = dispatcher;
            this.host = host;
            this.port = port;
        }

        public void connect()
        {
            connect(null);
        }
        public void connect(object obj)
        {
            if (socket != null)
            {
                stop();
            }
            isStopping = false;
            socket = new USocket(host, port);

#if UNITY_EDITOR
            Debug.Log("connect ==" + host + ":" + port);
#endif
            //异步连接
            socket.connectAsync(onConnectStateChg);
        }

        /// <summary>
        /// Ons the connect state chg. 当连接状态发生变化时
        /// </summary>
        /// <param name="s">S.</param>
        /// <param name="result">Result. 其实是bool类型，
        /// 当为true表示连接成功，false时表示没有连接成功或连接断开</param>
        public virtual void onConnectStateChg(USocket s, object result)
        {
            if ((bool)result)
            {
#if UNITY_EDITOR
                Debug.Log("connectCallback    success");
#endif
                connected = true;
                reConnectTimes = 0;
                socket.ReceiveAsync(onReceive);
                enqueueData(CONST_Connect);
            }
            else
            {
                Debug.LogWarning("connectCallback    fail" + host + ":" + port + "," + isStopping);
                connected = false;
                if (!isStopping)
                {
                    outofNetConnect();
                }
            }
        }

        public void outofNetConnect()
        {
            if (isStopping)
                return;
            if (reConnectTimes < MaxReConnectTimes)
            {
                reConnectTimes++;
                if (timer != null)
                {
                    timer.Dispose();
                }
                timer = TimerEx.schedule(connect, null, 5000);
            }
            else
            {
                if (timer != null)
                {
                    timer.Dispose();
                }
                timer = null;
                outofLine(socket, null);
            }
        }

        public void outofLine(USocket s, object obj)
        {
            if (!isStopping)
            {
                stop();
                //CLMainBase.self.onOffline();
                enqueueData(CONST_OutofNetConnect);
            }
        }

        public virtual void stop()
        {
            isStopping = true;
            connected = false;
            if (socket != null)
            {
                socket.close();
            }
            socket = null;
        }

        //==========================================
        public void send(object obj)
        {
            if (socket == null)
            {
                Debug.LogWarning("Socket is null");
                return;
            }
            object ret = packMessage(obj);

            if (ret == null || isStopping || !connected)
            {
                return;
            }
            socket.SendAsync(ret as byte[]);
        }

        public object packMessage(object obj)
        {
            try
            {
                return encodeData(obj);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        MemoryStream os = new MemoryStream();
        MemoryStream os2 = new MemoryStream();

        /// <summary>
        /// Encodes the data.数据组包准备发送
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="obj">Object.</param>
        public virtual byte[] encodeData(object obj)
        {
            os.Position = 0;
            os2.Position = 0;

            B2OutputStream.writeObject(os, obj);
            int len = (int)os.Position;
            B2OutputStream.writeInt(os2, len);
            os2.Write(os.ToArray(), 0, len);
            int pos = (int)os2.Position;
            byte[] result = new byte[pos];
            os2.Position = 0;
            os2.Read(result, 0, pos);
            return result;
        }

        //==========================================
        MemoryStreamPool memorystreamPool = new MemoryStreamPool();
        public void onReceive(USocket s, byte[] bytes, int len)
        {
            MemoryStream buffer = memorystreamPool.borrowObject();
            buffer.Write(bytes, 0, len);
            buffer.SetLength(len);
            enqueueData(buffer);
        }

        object netData = null;
        MemoryStream memoryBuff = null;
        MemoryStream receivedBuffer = new MemoryStream();
        public IEnumerator wrapBuffer2Unpack()
        {
            yield return null;
            while (receivedDataQueue.Count > 0)
            {
                netData = receivedDataQueue.Dequeue();
                if (netData != null)
                {
                    if (netData is string)
                    {
                        if (mDispatcher != null)
                        {
                            mDispatcher(netData, this);
                        }
                        continue;
                    }
                    memoryBuff = netData as MemoryStream;
                    receivedBuffer.Write(memoryBuff.ToArray(), 0, (int)(memoryBuff.Length));
                    memorystreamPool.returnObject(memoryBuff);
                }
            }
            if (receivedBuffer.Length > 0)
            {
                receivedBuffer.SetLength(receivedBuffer.Position);
                unpackMsg(receivedBuffer);
            }
        }

        public void unpackMsg(MemoryStream buffer)
        {
            bool isLoop = true;
            object o = null;
            long usedLen = 0;
            while (isLoop)
            {
                long totalLen = buffer.Length;
                if (totalLen > 2)
                {
                    usedLen = 0;
                    o = parseRecivedData(buffer);
                    usedLen = buffer.Position;
                    if (usedLen > 0)
                    {
                        int leftLen = (int)(totalLen - usedLen);
                        if (leftLen > 0)
                        {
                            buffer.Read(tmpBuffer, 0, leftLen);
                            buffer.Position = 0;
                            buffer.Write(tmpBuffer, 0, leftLen);
                            buffer.SetLength(leftLen);
                        }
                        else
                        {
                            buffer.Position = 0;
                            buffer.SetLength(0);
                            isLoop = false;
                        }
                    }
                    else
                    {
                        //buffer.Position = totalLen;
                        isLoop = false;
                    }

                    if (o != null && mDispatcher != null)
                    {
                        mDispatcher(o, this);
                    }
                }
                else
                {
                    isLoop = false;
                }
            }
        }

        /// <summary>
        /// Parses the recived data.解析接收的数据，解析成功后发送给dispatcher
        /// </summary>
        /// <returns>The recived data.</returns>
        /// <param name="buffer">Buffer.</param>
        public virtual object parseRecivedData(MemoryStream buffer)
        {
            object ret = null;
            long oldPos = buffer.Position;
            buffer.Position = 0;
            long tatolLen = buffer.Length;
            long needLen = B2InputStream.readInt(buffer);
            if (needLen <= 0 || needLen > __maxLen)
            {
                // 网络Number据错误。断isOpen网络
                outofLine(this.socket, false);
                //this.stop();
                return null;
            }
            long usedLen = buffer.Position;
            if (usedLen + needLen <= tatolLen)
            {
                ret = B2InputStream.readObject(buffer);
            }
            else
            {
                //说明长度不够
                buffer.Position = oldPos;
            }
            return ret;
        }

        //======================================================================
        //======================================================================
        //======================================================================
        public Queue receivedDataQueue = new Queue();

        public void enqueueData(object obj)
        {
            receivedDataQueue.Enqueue(obj);
            if (!serializeInMainThread)
            {
                StartCoroutine(wrapBuffer2Unpack());
            }
        }

        public virtual void Update()
        {
            if (serializeInMainThread && receivedDataQueue.Count > 0)
            {
                StartCoroutine(wrapBuffer2Unpack());
            }
        }
    }
}
