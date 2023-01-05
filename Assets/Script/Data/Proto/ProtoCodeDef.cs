//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2018-02-25 22:40:37
//备    注：
//===================================================
using System.Collections;

/// <summary>
/// 协议编号定义
/// </summary>
public class ProtoCodeDef
{
    /// <summary>
    /// 客户端发送 用户登陆 消息
    /// </summary>
    public const ushort Role_Login_C2S = 10000;
    
    /// <summary>
    /// 客户端发送 获取角色列表 消息
    /// </summary>
    public const ushort Role_Get_Role_List_C2S = 10002;

    /// <summary>
    /// 客户端发送 创建角色 消息
    /// </summary>
    public const ushort Role_Create_Role_C2S = 10003;
    
    /// <summary>
    /// 客户端发送 进入游戏 消息
    /// </summary>
    public const ushort Role_Enter_Game_C2S = 10004;
    
    /// <summary>
    /// 客户端发送 删除角色 消息
    /// </summary>
    public const ushort Role_Delete_Role_C2S = 10005;
    
    /// <summary>
    /// 客户端发送 请求本角色信息 消息
    /// </summary>
    public const ushort Role_Myself_Info_C2S = 13001;

}
