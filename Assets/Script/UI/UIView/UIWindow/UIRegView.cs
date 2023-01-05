//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-04-24 07:36:27
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIRegView : UIWindowViewBase
{
    public InputField txtUserName;
    public InputField txtPwd;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnReg":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIRegView_btnReg);
                break;
            case "btnToLogOn":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIRegView_btnToLogOn);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        txtUserName = null;
        txtPwd = null;
    }
}