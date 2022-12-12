using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMMOMemory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetWorkSocket.Instance.Connect("127.0.0.1", 7788);

        GlobalInit.Instance.OnReceiveProto = OnReceiveProtoCallBack;
        
        
        
        /*
       TestProto proto = new TestProto();
       proto.Id = 1;
       proto.Name = "风格和";
       proto.Type = 0;
       proto.Price = 99;
       byte[] buffer = null;
 
       buffer = proto.ToArray();
       Debug.Log("buffer=" + buffer.Length);
       
       TestProto proto2 = TestProto.GetProto(buffer);
       Debug.Log("Name=" + proto2.Name);
        */
    }
    // 委托回调
    private void OnReceiveProtoCallBack(ushort protoCode, byte[] buffer)
    {
     Debug.Log("protoCode="+protoCode);
     if (protoCode == ProtoCodeDef.Role_Login_S2C)
     {
         Role_Login_S2CProto proto = Role_Login_S2CProto.GetProto(buffer);
         Debug.Log("IsSuccess="+proto.IsSuccess);
         
     }
     
     
    }

    // Update is called once per frame
    void Update()
    {
        
        #region 草稿
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Send(10000,"InputA");
            //发消息
            using (MMO_MemoryStream ms = new MMO_MemoryStream())
            {
                ms.WriteUShort(10000);
                ms.WriteInt(255);
                ms.WriteInt(22222);
                ms.WriteUTF8String("afagvf");
                ms.WriteUTF8String("vf1565");
                NetWorkSocket.Instance.SendMsg(ms.ToArray());
            }
        }
       else if (Input.GetKeyDown(KeyCode.B))
        {
           // Send(10003,"InputB");
            using (MMO_MemoryStream ms = new MMO_MemoryStream())
            {
                ms.WriteUShort(10003);
                ms.WriteByte(1);
                ms.WriteByte(1);
                ms.WriteByte(1);
                ms.WriteUTF8String("asd5229");
                NetWorkSocket.Instance.SendMsg(ms.ToArray());
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < 10; i++)
            {
                Send("InputC:"+i);
            }
         
        }
        
        */
        #endregion
        if (Input.GetKeyDown(KeyCode.A))
        {
            Role_Login_C2SProto proto = new Role_Login_C2SProto();
            proto.AccId = 11123;
            proto.TsTamp = 2222;
            proto.Ticket = "qodqw4dq65s4d";
            proto.AccName = "nishizhu01";
         
            NetWorkSocket.Instance.SendMsg(proto.ToArray());
        }
    }
}