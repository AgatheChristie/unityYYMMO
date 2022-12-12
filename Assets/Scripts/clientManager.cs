using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Text;
using Common;

public class clientManager 
{
  
    private const string IP = "127.0.0.1";
    private const int PORT = 7788;
    private Message msg = new Message();
    private Socket clientSocket;
    private void OnDestroy()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }

    public void Connect()
    {
        //如果socket已经存在并且处于连接中状态则直接返回  
        if (clientSocket != null && clientSocket.Connected) return;

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(IP), PORT));
            Start();
            Debug.Log("连接成功");
        }
        catch (Exception e)
        {
            Debug.Log("连接失败=" + e.Message);
        }
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
    
  
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int count = clientSocket.EndReceive(ar);
           msg.ReadMessage(count,OnProcessDataCallBack);
           Start();
        }
        catch (Exception e)
        {
            Debug.Log("ReceiveCallBack err:" + e.Message);
        }
    }

    private void OnProcessDataCallBack(ActionCode arg1, string arg2)
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
   private void Start()
    {   
        clientSocket.BeginReceive(msg.Data, msg.StartIndex,msg.RemainSize,SocketFlags.None, ReceiveCallBack, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
