//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-05-24 22:28:37
//备    注：
//===================================================

/// <summary>
/// 区服实体
/// </summary>
public class RetGameServerEntity
{
    /// <summary>
    /// 区分编号
    /// </summary>
    public int Id;

    /// <summary>
    /// 区服状态
    /// </summary>
    public int RunStatus;

    /// <summary>
    /// 是否推荐
    /// </summary>
    public bool IsCommand;

    /// <summary>
    /// 是否新服
    /// </summary>
    public bool IsNew;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name;

    /// <summary>
    /// IP
    /// </summary>
    public string Ip;

    /// <summary>
    /// 端口号
    /// </summary>
    public int Port;
}