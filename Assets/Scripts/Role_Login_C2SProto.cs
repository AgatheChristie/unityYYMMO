//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2018-02-25 22:40:38
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;


public struct Role_Login_C2SProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Login_C2S; } }

    public int AccId; 
    public int TsTamp; 
    public string AccName; 
    public string Ticket; 
  

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(AccId);
            ms.WriteInt(TsTamp);
            ms.WriteUTF8String(AccName);
            ms.WriteUTF8String(Ticket);
            return ms.ToArray();
        }
    }
    
}