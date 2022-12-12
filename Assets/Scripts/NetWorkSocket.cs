using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Text;

public class NetWorkSocket : MonoBehaviour
{
    #region 单例

    private static NetWorkSocket instance;

    public static NetWorkSocket Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetWorkSocket>();
            }

            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.AddComponent<NetWorkSocket>();
                instance = obj.GetComponent<NetWorkSocket>();
            }

            return instance;
        }
    }

    #endregion


    #region 发送消息所需变量

    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

    private Action m_CheckSendQueue;

    #endregion

    #region 接收消息所需变量

    //接收数据包的字节数组缓冲区    
    private byte[] m_ReceiveBuffer = new byte[2048];

    //接收数据包的缓冲数据流
    private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();

    //接收消息的队列
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();

    private int m_ReceiveCount = 0;

    #endregion


    private Socket m_Client;

    private void OnDestroy()
    {
        if (m_Client != null && m_Client.Connected)
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }
    }

    private void Update()
    {
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        byte[] buffer = m_ReceiveQueue.Dequeue();
                        ushort protoCode = 0;
                        byte[] protoContent = new byte[buffer.Length - 2];
                        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
                        {
                            protoCode = ms.ReadUShort();
                            ms.Read(protoContent, 0, protoContent.Length);
                            Debug.Log(protoCode);
                            //临时
                            GlobalInit.Instance.OnReceiveProto(protoCode, protoContent);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }
        }
    }

    public void Connect(string ip, int port)
    {
        //如果socket已经存在并且处于连接中状态则直接返回  
        if (m_Client != null && m_Client.Connected) return;

        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_CheckSendQueue = OnCheckSendQueueCallBack;

            ReceiveMsg();
            Debug.Log("连接成功");
        }
        catch (Exception ex)
        {
            Debug.Log("连接失败=" + ex.Message);
        }
    }

    // 检查队列的委托回调
    private void OnCheckSendQueueCallBack()
    {
        lock (m_SendQueue)
        {
            if (m_SendQueue.Count > 0)
            {
                Send(m_SendQueue.Dequeue());
            }
        }
    }

    //封装数据包 
    private byte[] MakeData(byte[] data)
    {
        byte[] retBuffer = null;
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 2));
            ms.Write(data, 0, data.Length);
            retBuffer = ms.ToArray();
        }

        return retBuffer;
    }

    public void SendMsg(byte[] buffer)
    {
        //得到包体的byte数组 
        //   byte[] data = Encoding.UTF8.GetBytes(msg);
        //得到封装后的数据包
        byte[] sendBuffer = MakeData(buffer);

        lock (m_SendQueue)
        {
            //把数据包加入队列
            m_SendQueue.Enqueue(sendBuffer);
            // 启动委托（执行委托）
            m_CheckSendQueue.BeginInvoke(null, null);
        }
    }

    private void Send(byte[] buffer)
    {
        m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
    }

    private void SendCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);
        OnCheckSendQueueCallBack();
    }

    //================================================

    #region ReceiveMsg 接收数据

    /// <summary>
    /// 接收数据
    /// </summary>
    private void ReceiveMsg()
    {
        //异步接收数据
        m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
    }

    #endregion

    #region ReceiveCallBack 接收数据回调

    /// <summary>
    /// 接收数据回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            // 这里是什么意思
            int len = m_Client.EndReceive(ar);
            Debug.Log("len:" + len);
            if (len > 0)
            {
                //已经接收到数据

                //把接收到数据 写入缓冲数据流的尾部
                m_ReceiveMS.Position = m_ReceiveMS.Length;

                //把指定长度的字节 写入数据流
                m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);
                Debug.Log("m_ReceiveMS的Length:" + m_ReceiveMS.Length);
                //如果缓存数据流的长度>2 说明至少有个不完整的包过来了
                //为什么这里是2 因为我们客户端封装数据包 用的ushort 长度就是2
                // todo 包头是2位的长度和2位的协议
                if (m_ReceiveMS.Length > 2)
                {
                    //进行循环 拆分数据包
                    while (true)
                    {
                        //把数据流指针位置放在0处
                        m_ReceiveMS.Position = 0;

                        //currMsgLen = 包体的长度
                        int currMsgLen = m_ReceiveMS.ReadUShort() - 2;

                        // 这里没有移位置吗？
                        Debug.Log("m_ReceiveMS的Position:" + m_ReceiveMS.Position);

                        //currFullMsgLen 总包的长度=包头长度+包体长度
                        int currFullMsgLen = 2 + currMsgLen;

                        //如果数据流的长度>=整包的长度 说明至少收到了一个完整包
                        if (m_ReceiveMS.Length >= currFullMsgLen)
                        {
                            //至少收到一个完整包
                            //定义包体的byte[]数组
                            byte[] buffer = new byte[currMsgLen];
                            //把数据流指针放到2的位置 也就是包体的位置   这里本来就是2了把？
                            m_ReceiveMS.Position = 2;
                            //把包体读到byte[]数组
                            m_ReceiveMS.Read(buffer, 0, currMsgLen);
                            for (int p = 0; p < buffer.Length; p++)
                            {
                                Debug.Log(string.Format("{0}:{1} ", p, buffer[p]));
                            }

                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(buffer);
                            }
                            //==============处理剩余字节数组===================

                            //剩余字节长度
                            int remainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                            Debug.Log("剩余字节长度:" + remainLen);
                            if (remainLen > 0)
                            {
                                //把指针放在第一个包的尾部
                                m_ReceiveMS.Position = currFullMsgLen;

                                //定义剩余字节数组
                                byte[] remainBuffer = new byte[remainLen];

                                //把数据流读到剩余字节数组
                                m_ReceiveMS.Read(remainBuffer, 0, remainLen);

                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);

                                //把剩余字节数组重新写入数据流
                                m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                remainBuffer = null;
                            }
                            else
                            {
                                //没有剩余字节

                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);

                                break;
                            }
                        }
                        else
                        {
                            //还没有收到完整包
                            break;
                        }
                    }
                }

                //进行下一次接收数据包
                ReceiveMsg();
            }
            else
            {
                //客户端断开连接
                Debug.Log(string.Format("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString()));
            }
        }
        catch
        {
            //客户端断开连接
            Debug.Log(string.Format("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString()));
        }
    }

    #endregion
}