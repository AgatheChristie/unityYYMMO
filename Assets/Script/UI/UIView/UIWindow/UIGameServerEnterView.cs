using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameServerEnterView
    : UIWindowViewBase
{

    public Text lblDefaultGameServer;

    public void SetUI(string gameServerName)
    {
        lblDefaultGameServer.text = gameServerName;
    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnSelectGameServer":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGameServerEnterView_btnSelectGameServer);
                break;
            case "btnEnterGame":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGameServerEnterView_btnEnterGame);
                break;
        }
    }
}
