//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2018-02-25 22:40:38
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;


public struct Role_Login_S2CProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Login_S2C; } }

    public ushort IsSuccess;

    public static Role_Login_S2CProto GetProto(byte[] buffer)
    {
        Role_Login_S2CProto proto = new Role_Login_S2CProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.IsSuccess = ms.ReadUShort();
        }
        return proto;
    }
}