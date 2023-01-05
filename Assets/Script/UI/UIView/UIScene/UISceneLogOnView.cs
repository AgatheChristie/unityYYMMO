//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-04-21 22:33:01
//备    注：登录场景UI视图
//===================================================

using System;
using UnityEngine;
using System.Collections;

public class UISceneLogOnView : UISceneViewBase
{
    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine(OpenLogOnWindow());
    }

    private IEnumerator OpenLogOnWindow()
    {
        yield return new WaitForSeconds(0.2f);
        AccountCtrl.Instance.QuickLogOn();
    }
}