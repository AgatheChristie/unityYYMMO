//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2018-02-25 22:40:38
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;


public struct Role_Get_Role_List_S2CProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Get_Role_List_C2S; } }

    public ushort RoleCount;
    public List<RoleItem> RoleList;

    public struct RoleItem
    {
        public int RoleId;
        public ushort RoleStatus;
        public ushort RoleCareer;
        public ushort RoleSex;
        public ushort RoleLevel;
        public string RoleNickName;
    }
    
    public static Role_Get_Role_List_S2CProto GetProto(byte[] buffer)
    {
        Role_Get_Role_List_S2CProto proto = new Role_Get_Role_List_S2CProto();
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream(buffer))
        {
            proto.RoleCount = ms.ReadUShort();
            proto.RoleList = new List<RoleItem>();
            for (int i = 0; i < proto.RoleCount; i++)
            {
                RoleItem _Role = new RoleItem();
                _Role.RoleId = ms.ReadInt();
                _Role.RoleStatus = ms.ReadUShort();
                _Role.RoleCareer = ms.ReadUShort();
                _Role.RoleSex = ms.ReadUShort();
                _Role.RoleLevel = ms.ReadUShort();
                _Role.RoleNickName = ms.ReadUTF8String();
              
                proto.RoleList.Add(_Role);
            }
        }
        return proto;
    }
    
    
    
}