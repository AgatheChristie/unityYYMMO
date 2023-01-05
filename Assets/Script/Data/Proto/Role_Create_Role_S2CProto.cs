//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2018-02-25 22:40:38
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;


public struct Role_Create_Role_S2CProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Create_Role_C2S; } }

    public ushort IsSuccess;
    public int PlayerId;

    
    public static Role_Create_Role_S2CProto GetProto(byte[] buffer)
    {
        Role_Create_Role_S2CProto proto = new Role_Create_Role_S2CProto();
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream(buffer))
        {
            proto.IsSuccess = ms.ReadUShort();
            proto.PlayerId = ms.ReadInt();
        }
        return proto;
    }
    
    
    
}