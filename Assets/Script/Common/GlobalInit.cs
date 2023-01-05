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
using System.Net.NetworkInformation;
using UnityEngine.tvOS;
using UnityEngine.UI;


public class GlobalInit : MonoBehaviour
{
    public static GlobalInit Instance;

    public delegate void OnReceiveProtoHandler(ushort protoCode, byte[] buffer);

    
    /// <summary>
    /// T4M
    /// </summary>
    public Shader T4MShader;
    
    //定义委托
    public OnReceiveProtoHandler OnReceiveProto;

    public AnimationCurve UIAnimationCurve = new AnimationCurve(new Keyframe(0, 0, 0, 1f), new Keyframe(1f, 1f, 1f, 0));
   
    [HideInInspector] public long ServerTime = 0;


    [HideInInspector] public RetAccountEntity CurrAccount;
    [HideInInspector] public RetGameServerEntity CurrSelectGameServer;
    
    [HideInInspector]
    public long CurrServerTime
    {
        get
        {
          //  return ServerTime + (long)RealTime.time;
            return ServerTime + (long)Time.time;
          //  return ServerTime + (long)6666;
        }
    }
  

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
    /// 连接服务器端IP地址
    /// </summary>
    public const string SocketIP = "127.0.0.1";

    /// <summary>
    /// 连接服务器端口号
    /// </summary>
    public const ushort Port = 7788;

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

    private void Start()
    {
        // NetWorkHttp.Instance.SendData
        OnGetTimeCallBack();
    }

    private void OnGetTimeCallBack()
    {
        ServerTime = CVUtil.GetTimestamp();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.F))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("已经清除本地数据");
        }
    }
}