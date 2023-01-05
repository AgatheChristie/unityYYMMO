//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-04-21 22:19:00
//备    注：所有UI视图的基类
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIViewBase : MonoBehaviour
{
    public Action OnShow;
    
    void Awake()
    {
        OnAwake();
      //  Debug.Log("Start "+Time.frameCount);
    }

    void Start()
    {
        Button[] btnArr = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btnArr.Length; i++)
        {
            EventTriggerListener.Get(btnArr[i].gameObject).onClick = BtnClick;
        }
       // Debug.Log("Start "+Time.frameCount);
        OnStart();
        if (OnShow != null)
        {
            //   执行委托
            OnShow();
        }
    }

    void OnDestroy()
    {
        BeforeOnDestroy();
    }

    private void BtnClick(GameObject go)
    {
        OnBtnClick(go);
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void BeforeOnDestroy() { }
    protected virtual void OnBtnClick(GameObject go) { }
}