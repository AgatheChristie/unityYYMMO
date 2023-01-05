using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AccountCtrl : SystemCtrlBase<AccountCtrl>, ISystemCtrl
{
    private UILogOnView m_LogOnView;
    private UIRegView m_RegView;

    private bool m_IsAutoLogOn;

    public AccountCtrl()
    {
        AddEventListener(ConstDefine.UILogOnView_btnLogOn, LogOnViewBtnLogOnClick);
        AddEventListener(ConstDefine.UILogOnView_btnToReg, LogOnViewBtnToRegClick);
        AddEventListener(ConstDefine.UIRegView_btnReg, RegViewBtnRegClick);
        AddEventListener(ConstDefine.UIRegView_btnToLogOn, RegViewBtnToLogOnClick);
      
        
    }

   
    #region 四个点击事件
    private void RegViewBtnToLogOnClick(object[] param)
    {
        m_RegView.CloseAndOpenNext(WindowUIType.LogOn);
    }
   
    private void RegViewBtnRegClick(object[] param)
    {
        if (string.IsNullOrEmpty(m_RegView.txtUserName.text))
        {
            ShowMessage("注册提示", "请输入用户名", okAction: () => { Debug.Log("您点击了确定按钮"); });
            return;
        }

        if (string.IsNullOrEmpty(m_RegView.txtPwd.text))
        {
            ShowMessage("注册提示", "请输入密码");
            return;
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 0;
        dic["UserName"] = m_RegView.txtUserName.text;
        dic["Pwd"] = m_RegView.txtPwd.text;
        dic["ChannelId"] = 0;
        // NetWorkTttp.Instance.SendData()
        OnRegCallBack();
    }


    private void OnRegCallBack()
    {
        Debug.Log("注册成功");

        RetAccountEntity entity = new RetAccountEntity()
        {
            Id = 1,
            UserName = "大狗",
            Pwd = "123456",
            YuanBao = 9999,
            LastServerId = 1,
            LastServerName = "与民同乐",
            LastServerIP = "127.0.0.1",
            LastServerPort = 7788, 
            LastServerRunStatus = 1
        };
        GlobalInit.Instance.CurrAccount = entity;
        SetCurrSelectGameServer(entity);
        Stat.Reg(entity.Id, m_RegView.txtUserName.text);

        PlayerPrefs.SetInt(ConstDefine.LogOn_AccountId, entity.Id);
        PlayerPrefs.SetString(ConstDefine.LogOn_AccountUserName, m_RegView.txtUserName.text);
        PlayerPrefs.SetString(ConstDefine.LogOn_AccountPwd, m_RegView.txtPwd.text);

        m_RegView.CloseAndOpenNext(WindowUIType.GameServerEnter);
    }
    
    private void LogOnViewBtnToRegClick(object[] param)
    {
        m_LogOnView.CloseAndOpenNext(WindowUIType.Reg);
    }

    private void LogOnViewBtnLogOnClick(object[] param)
    {
        if (string.IsNullOrEmpty(m_LogOnView.txtUserName.text))
        {
            ShowMessage("登录提示", "请输入用户名");
            return;
        }

        if (string.IsNullOrEmpty(m_LogOnView.txtPwd.text))
        {
            ShowMessage("登录提示", "请输入密码");
            return;
        }

        m_IsAutoLogOn = false;
        string daw = DeviceUtil.DeviceIdentifier;
        string daw2 = DeviceUtil.DeviceModel;

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 1;
        dic["UserName"] = m_LogOnView.txtUserName.text;
        dic["Pwd"] = m_LogOnView.txtPwd.text;
        // NetWorkTttp.Instance.SendData()

        Debug.Log("DeviceIdentifier:" + daw);
        Debug.Log("DeviceModel:" + daw2);
        OnLogOnCallBack();
    }
    
    private void OnLogOnCallBack()
    {
        Debug.Log("登录成功");
        
        RetAccountEntity entity = new RetAccountEntity()
        {
            Id = 1,
            UserName = "大狗",
            Pwd = "123456",
            YuanBao = 9999,
            LastServerId = 1,
            LastServerName = "与民同乐",
            LastServerIP = "127.0.0.1",
            LastServerPort = 7788, 
            LastServerRunStatus = 1
        };
        GlobalInit.Instance.CurrAccount = entity;
        SetCurrSelectGameServer(entity);
        string userName = string.Empty;
        if (m_IsAutoLogOn)
        {
            userName = PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName);
            UIViewMgr.Instance.OpenWindow(WindowUIType.GameServerEnter);
        }
        else
        {
            PlayerPrefs.SetInt(ConstDefine.LogOn_AccountId, entity.Id);
            PlayerPrefs.SetString(ConstDefine.LogOn_AccountUserName, m_LogOnView.txtUserName.text);
            PlayerPrefs.SetString(ConstDefine.LogOn_AccountPwd, m_LogOnView.txtPwd.text);

            userName = m_LogOnView.txtUserName.text;
            m_LogOnView.CloseAndOpenNext(WindowUIType.GameServerEnter);
        }

        Stat.LogOn(entity.Id, userName);
    }
    #endregion
    

    
    private void SetCurrSelectGameServer(RetAccountEntity entity)
    {
        RetGameServerEntity currGameServerEntity = new RetGameServerEntity();
        currGameServerEntity.Id = entity.LastServerId;
        currGameServerEntity.Name = entity.LastServerName;
        currGameServerEntity.Ip = entity.LastServerIP;
        currGameServerEntity.Port = entity.LastServerPort;

        GlobalInit.Instance.CurrSelectGameServer = currGameServerEntity;
    }
  

    public void QuickLogOn()
    {
        if (!PlayerPrefs.HasKey(ConstDefine.LogOn_AccountId))
        {
            Debug.Log("QuickLogOn is OpenView");
        
            GameServerCtrl.Instance.OpenView(WindowUIType.GameServerEnter);
        }
        else
        {
            Debug.Log("QuickLogOn not OpenView");
            // 自动登录  
            m_IsAutoLogOn = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Type"] = 1;
            dic["UserName"] = PlayerPrefs.GetString(ConstDefine.LogOn_AccountUserName);
            dic["Pwd"] = PlayerPrefs.GetString(ConstDefine.LogOn_AccountPwd);
            // NetWorkTttp.Instance.SendData()
        }
    }


    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.LogOn:
                OpenLogOnView();
                break;
            case WindowUIType.Reg:
                OpenRegView();
                break;
        }
    }

    public void OpenLogOnView()
    {
        m_LogOnView = UIViewUtil.Instance.OpenWindow(WindowUIType.LogOn).GetComponent<UILogOnView>();
    }

    public void OpenRegView()
    {
        m_RegView = UIViewUtil.Instance.OpenWindow(WindowUIType.Reg).GetComponent<UIRegView>();
    }

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListener(ConstDefine.UILogOnView_btnLogOn, LogOnViewBtnLogOnClick);
        RemoveEventListener(ConstDefine.UILogOnView_btnToReg, LogOnViewBtnToRegClick);
        RemoveEventListener(ConstDefine.UIRegView_btnReg, RegViewBtnRegClick);
        RemoveEventListener(ConstDefine.UIRegView_btnToLogOn, RegViewBtnToLogOnClick);
    }
    #endregion
    
    
}