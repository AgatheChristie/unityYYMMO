using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameServerCtrl : SystemCtrlBase<GameServerCtrl>, ISystemCtrl
{
    private UIGameServerEnterView m_GameServerEnterView;
    private UIGameServerSelectView m_GameServerSelectView;


    private Dictionary<int, List<RetGameServerEntity>> m_GameServerDic =
        new Dictionary<int, List<RetGameServerEntity>>();

    private int m_CurrClickPageIndex = 0;
    private bool m_IsBusy = false;

    public GameServerCtrl()
    {
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.Role_Login_C2S, RoleLogOnReturn);

        AddEventListener(ConstDefine.UIGameServerEnterView_btnSelectGameServer,
            GameServerEnterViewBtnSelectGameServerClick);
        AddEventListener(ConstDefine.UIGameServerEnterView_btnEnterGame, GameServerEnterViewBtnEnterGameClick);

        NetWorkSocket.Instance.OnConnectOk = OnConnectOkCallBack;
    }

    private void OnConnectOkCallBack()
    {
        UpdateLastLogOnServer(GlobalInit.Instance.CurrAccount, GlobalInit.Instance.CurrSelectGameServer);
        SceneMgr.Instance.LoadToSelectRole();
    }

    private void GameServerEnterViewBtnSelectGameServerClick(object[] p)
    {
        //点击 选择区服就打开视图
        m_GameServerSelectView = UIViewUtil.Instance
            .OpenWindow(WindowUIType.GameServerSelect, () =>
            {
                if (GlobalInit.Instance.CurrSelectGameServer != null)
                {
                    m_GameServerSelectView.SetSelectGameServerUI(GlobalInit.Instance.CurrSelectGameServer);
                }

                GetGameServerPage();
            })
            .GetComponent<UIGameServerSelectView>();

        m_GameServerSelectView.OnPageClick = OnPageClick;
        m_GameServerSelectView.OnGameServerClick = OnGameServerClick;
    }

    private void OnGetGameServerCallBack(int pageIndex)
    {
        m_IsBusy = false;
        List<RetGameServerEntity> lst = new List<RetGameServerEntity>();
        lst.Add(new RetGameServerEntity()
        {
            Id = 1, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 1) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 2, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 2) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 3, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 3) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 4, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 4) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 5, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 5) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 6, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 6) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 7, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 7) + "服", Ip = "127.0.0.1", Port = 7788
        });
        lst.Add(new RetGameServerEntity()
        {
            Id = 8, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
            Name = "群雄争霸" + (pageIndex * 8) + "服", Ip = "127.0.0.1", Port = 7788
        });
        // lst.Add(new RetGameServerEntity()
        // {
        //     Id = 9, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
        //     Name = "群雄争霸" + (pageIndex * 9) + "服", Ip = "127.0.0.1", Port = 7788
        // });
        // lst.Add(new RetGameServerEntity()
        // {
        //     Id = 10, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
        //     Name = "群雄争霸" + (pageIndex * 10) + "服", Ip = "127.0.0.1", Port = 7788
        // });
        // lst.Add(new RetGameServerEntity()
        // {
        //     Id = 11, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
        //     Name = "群雄争霸" + (pageIndex * 11) + "服", Ip = "127.0.0.1", Port = 7788
        // });
        // lst.Add(new RetGameServerEntity()
        // {
        //     Id = 12, RunStatus = Random.Range(0, 3), IsCommand = true, IsNew = true,
        //     Name = "群雄争霸" + (pageIndex * 12) + "服", Ip = "127.0.0.1", Port = 7788
        // });
        m_GameServerDic[m_CurrClickPageIndex] = lst;
        if (m_GameServerSelectView != null)
        {
            m_GameServerSelectView.SetGameServerUI(lst);
        }
    }

    private void OnGetGameServerPageCallBack()
    {
        List<RetGameServerPageEntity> lst = new List<RetGameServerPageEntity>();
        lst.Add(new RetGameServerPageEntity() { Name = "31-40服", PageIndex = 4 });
        lst.Add(new RetGameServerPageEntity() { Name = "21-30服", PageIndex = 3 });
        lst.Add(new RetGameServerPageEntity() { Name = "11-20服", PageIndex = 2 });
        lst.Add(new RetGameServerPageEntity() { Name = "1-10服", PageIndex = 1 });
        if (m_GameServerSelectView != null)
        {
            lst.Insert(0, new RetGameServerPageEntity() { Name = "推荐区服", PageIndex = 0 });
            m_GameServerSelectView.SetGameServerPageUI(lst);

            GetGameServer(0);
        }
    }

    private void OnGameServerClick(RetGameServerEntity obj)
    {  
        m_GameServerSelectView.Close();
        GlobalInit.Instance.CurrSelectGameServer = obj;
        if (m_GameServerEnterView != null)
        {
            m_GameServerEnterView.SetUI(GlobalInit.Instance.CurrSelectGameServer.Name);
        }
    }

    private void OnPageClick(int pageIndex)
    {
        GetGameServer(pageIndex);
    }

    private void GetGameServerPage()
    {
        //获取页签
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 0;
        // NetWorkTttp.Instance.SendData()
        OnGetGameServerPageCallBack();
    }

    private void GetGameServer(int pageIndex)
    {
        if (m_GameServerDic.ContainsKey(pageIndex))
        {
            if (m_GameServerSelectView != null)
            {  
                m_GameServerSelectView.SetGameServerUI(m_GameServerDic[pageIndex]);
            }

            return;
        }

        m_CurrClickPageIndex = pageIndex;
        if (m_IsBusy == true)
        {
            return;
        }

        m_IsBusy = true;
        //获取区服
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 0;
        dic["pageIndex"] = pageIndex;
        // NetWorkTttp.Instance.SendData()

        OnGetGameServerCallBack(pageIndex);
    }

    private void UpdateLastLogOnServer(RetAccountEntity currAccount, RetGameServerEntity currGameServer)
    {
        if (currAccount == null)
        {
            return;
        }

        if (currGameServer == null)
        {
            return;
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["Type"] = 2;
        dic["userId"] = currAccount.Id;
        dic["lastServerId"] = currGameServer.Id;
        dic["lastServerName"] = currGameServer.Name;
        // NetWorkTttp.Instance.SendData()

        OnUpdateLastLogOnServerCallBack();
    }

    private  void OnUpdateLastLogOnServerCallBack()
    {
    }

    private void GameServerEnterViewBtnEnterGameClick(object[] p)
    {
        NetWorkSocket.Instance.Connect(
            GlobalInit.Instance.CurrSelectGameServer.Ip,
            GlobalInit.Instance.CurrSelectGameServer.Port);
        TCPRoleLogOn();
    }

    public void OpenView(WindowUIType type)
    {
        switch (type)
        {
            case WindowUIType.GameServerEnter:
                OpenGameServerEnterView();
                break;
            case WindowUIType.GameServerSelect:
                OpenGameServerSelectView();
                break;
        }
    }


    private void OpenGameServerEnterView()
    {
        m_GameServerEnterView = UIViewUtil.Instance.OpenWindow(WindowUIType.GameServerEnter,
                () => { m_GameServerEnterView.SetUI(Qdwww()); })
            .GetComponent<UIGameServerEnterView>();
    }

    private string Qdwww()
    {
        if (GlobalInit.Instance.CurrSelectGameServer != null)
        {
            return GlobalInit.Instance.CurrSelectGameServer.Name;
        }
        else
        {
            MakeRetGameServerEntity();
            return GlobalInit.Instance.CurrSelectGameServer.Name;
        }
    }

    private void MakeRetGameServerEntity()
    {
        GlobalInit.Instance.CurrSelectGameServer = new RetGameServerEntity()
        {
            Id = 1,
            RunStatus = 1,
            IsCommand = true,
            IsNew = true,
            Name = "沉鱼落雁",
            Ip = "127.0.0.1",
            Port = 7788,
        };
    }

    private void OpenGameServerSelectView()
    {
    }

    public override void Dispose()
    {
        base.Dispose();
        RemoveEventListener(ConstDefine.UIGameServerEnterView_btnSelectGameServer,
            GameServerEnterViewBtnSelectGameServerClick);
        RemoveEventListener(ConstDefine.UIGameServerEnterView_btnEnterGame, GameServerEnterViewBtnEnterGameClick);
    }

    //==================================================================
    private void TCPRoleLogOn()
    {
        Role_Login_C2SProto proto = new Role_Login_C2SProto();
        proto.AccId = 1123;
        proto.TsTamp = 2222;
        proto.Ticket = "qodqw4dq65s4d";
        proto.AccName = "A8264";

        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    private void RoleLogOnReturn(byte[] p)
    {
        Role_Login_S2CProto proto = Role_Login_S2CProto.GetProto(p);
        Debug.Log("RoleLogOn IsSuccess:" + proto.IsSuccess);
    }
}