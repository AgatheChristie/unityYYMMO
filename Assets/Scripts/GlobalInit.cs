//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-12-01 22:26:02
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Xml.Linq;


#if UNITY_IPHONE
using UnityEngine.iOS;
#endif

using System.Collections.Generic;


public class GlobalInit : MonoBehaviour
{
    
    public static GlobalInit Instance;
    
    public delegate void OnReceiveProtoHandler(ushort protoCode, byte[] buffer);

    //定义委托
    public OnReceiveProtoHandler OnReceiveProto;


    #region 常量
    /// <summary>
    /// 昵称KEY
    /// </summary>
    public const string MMO_NICKNAME = "MMO_NICKNAME";

    /// <summary>
    /// 密码KEY
    /// </summary>
    public const string MMO_PWD = "MMO_PWD";

    /// <summary>
    /// 账户服务器地址
    /// </summary>
    public static string WebAccountUrl;

    /// <summary>
    /// 渠道号
    /// </summary>
    public static int ChannelId;

    /// <summary>
    /// 内部版本号
    /// </summary>
    public static int InnerVersion;

    #endregion

    void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}