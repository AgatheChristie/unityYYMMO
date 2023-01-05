//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2018-02-25 22:40:38
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;


public struct Role_Myself_Info_S2CProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Myself_Info_C2S; } }

    public int Scene;  //地图唯一ID
    public ushort X; //X坐标
    public ushort Y; //Y坐标
    public int Id; //用户ID
    public int Hp; //气血
    public int HpLim; //气血上线
    public int Mp; // 内息
    public int MpLim; //内息上线
    public byte Sex; //性别
    public byte Lv; // 等级
    public int Exp; //经验
    public int ExpLim; //经验上限
    public byte Career; //职业
    public string NickName; //玩家名
    public ushort Att; //攻击
    public ushort Def; //防御
    public ushort Forza; //力量
    public ushort Agile; //敏捷 
    public ushort Wit; //智力
    public ushort Hit; //命中
    public ushort Dodge; //躲避
    public ushort Crit; //暴击
    public ushort Ten; //坚韧
    public ushort GuildId; //帮派id
    public string GuildName; //帮派名
    public byte GuildPosition; //帮派职位
    public byte Realm; //阵营
    public int Gold; //金币
    public int Silver; //银币
    public int Coin; //铜币
    public byte AttArea; //攻击距离
    public int Spirit; //灵力
    public ushort Speed; //移动速度
    public ushort AttSpeed; //攻击速度
    public int E1; //武器
    public int E2; //装备
    public int E3; //坐骑
    
    
    
    public static Role_Myself_Info_S2CProto GetProto(byte[] buffer)
    {
        Role_Myself_Info_S2CProto proto = new Role_Myself_Info_S2CProto();
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream(buffer))
        {
            proto.Scene = ms.ReadInt();
            proto.X = ms.ReadUShort();
            proto.Y = ms.ReadUShort();
            proto.Id = ms.ReadInt();
            proto.Hp = ms.ReadInt();
            proto.HpLim = ms.ReadInt();
            proto.Mp = ms.ReadInt();
            proto.MpLim = ms.ReadInt();
            proto.Sex = (byte)ms.ReadByte();
            proto.Lv = (byte)ms.ReadByte();
            proto.Exp = ms.ReadInt();
            proto.ExpLim = ms.ReadInt();
            proto.Career = (byte)ms.ReadByte();
            proto.NickName = ms.ReadUTF8String();
            proto.Att = ms.ReadUShort();
            proto.Def = ms.ReadUShort();
            proto.Forza = ms.ReadUShort();
            proto.Agile = ms.ReadUShort();
            proto.Wit = ms.ReadUShort();
            proto.Hit = ms.ReadUShort();
            proto.Dodge = ms.ReadUShort();
            proto.Crit = ms.ReadUShort();
            proto.Ten = ms.ReadUShort();
            proto.GuildId = ms.ReadUShort();
            proto.GuildName = ms.ReadUTF8String();
            proto.GuildPosition = (byte)ms.ReadByte();
            proto.Realm = (byte)ms.ReadByte();
            proto.Gold = ms.ReadInt();
            proto.Silver = ms.ReadInt();
            proto.Coin = ms.ReadInt();
            proto.AttArea = (byte)ms.ReadByte();
            proto.Spirit = ms.ReadInt();
            proto.Speed = ms.ReadUShort();
            proto.AttSpeed = ms.ReadUShort();
            proto.E1 = ms.ReadInt();
            proto.E2 = ms.ReadInt();
            proto.E3 = ms.ReadInt();
        
        }
        return proto;
    }
}