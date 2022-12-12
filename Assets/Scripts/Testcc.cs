using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;


                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    //  obj.hideFlags = HideFlags.HideAndDontSave; //隐藏实例化的new game object
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}

public class Testcc : MonoBehaviour
{
    // 接受数据包的字节数组缓冲区
    private byte[] m_ReceiveBuffer = new byte[10240];

    // 接受数据包的缓冲数据流
    private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();


    // private void ReceiveCallBack(IAsyncResult ar)
    // {
    //     int len = m_Socket.EndReceive(ar);
    //     if (len > 0)
    //     {
    //         // 已经接收到数据 
    //         //把接收到数据写入缓冲数据流的尾部  
    //         m_ReceiveMS.Position = m_ReceiveMS.Length;
    //         // 把指定长度的字节写入数据流
    //         m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);
    //
    //         // 假设只有一条消息 这个就是我们的消息
    //         byte[] buffer = m_ReceiveMS.ToArray();
    //     }
    //     else
    //     {
    //         
    //     }
    // }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}