//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-04-24 07:34:46
//备    注：
//===================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILogOnView : UIWindowViewBase
{
    public InputField txtUserName;
    public InputField txtPwd;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnLogOn":
                UIDispatcher.Instance.Dispatch(ConstDefine.UILogOnView_btnLogOn);
                break;
            case "btnToReg":
                UIDispatcher.Instance.Dispatch(ConstDefine.UILogOnView_btnToReg);
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