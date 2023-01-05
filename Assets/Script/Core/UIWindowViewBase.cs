//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-04-21 22:27:41
//备    注：
//===================================================

using UnityEngine;
using System.Collections;
using System;

public class UIWindowViewBase : UIViewBase
{
    [SerializeField] public WindowUIContainerType containerType = WindowUIContainerType.Center;
    [SerializeField] public WindowShowStyle showStyle = WindowShowStyle.Normal;
    [SerializeField] public float duration = 1f;
    [HideInInspector] public WindowUIType CurrentUIType;

    private WindowUIType m_NextOpenType;

    // public Action<WindowUIType> OnViewClose;
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name.Equals("btnClose", StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }

    public virtual void Close()
    {
        UIViewUtil.Instance.CloseWindow(CurrentUIType);
    }

    public virtual void CloseAndOpenNext(WindowUIType nextType)
    {
        this.Close();
        m_NextOpenType = nextType;

        // if (OnViewClose != null)
        // {
        //     OnViewClose(nextType);
        // }
    }

    protected override void BeforeOnDestroy()
    {
        LayerUIMgr.Instance.CheckOpenWindow();
        if (m_NextOpenType != WindowUIType.None)
        {
            UIViewMgr.Instance.OpenWindow(m_NextOpenType);
        }
    }
}