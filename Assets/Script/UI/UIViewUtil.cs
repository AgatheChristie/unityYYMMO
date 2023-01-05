using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class UIViewUtil : Singleton<UIViewUtil>
{
    private Dictionary<WindowUIType, UIWindowViewBase> m_DicWindow = new Dictionary<WindowUIType, UIWindowViewBase>();


    public int OpenWindowCount
    {
        get { return m_DicWindow.Count; }
    }

    // todo 这里的继承获取 UIWindowBase windowBase = obj.GetComponent<UIWindowBase>(); 可以回看
    public GameObject OpenWindow(WindowUIType type,Action OnShow = null)
    {
        if (type == WindowUIType.None) return null;
        
        GameObject obj = null;
        //如果窗口不存在 则
        if (!m_DicWindow.ContainsKey(type) || m_DicWindow[type] == null)
        {
            // 枚举的名称要和预设的名称对应   
            obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIWindow,
                string.Format("pan_{0}", type.ToString()),
                cache: true);
            if (obj == null) return null;
            UIWindowViewBase windowBase = obj.GetComponent<UIWindowViewBase>();
            if (windowBase == null) return null;
            if (OnShow != null)
            {
                // 监听委托
                windowBase.OnShow = OnShow;
            }
            //不可以添加重复的建  这样会报错
            //m_DicText.Add(1, "afawf");    
            // m_DicText.Add(1, "afawf222");
            //m_DicWindow.Add(type, windowBase); 
            
            m_DicWindow[type] = windowBase;
            
            windowBase.CurrentUIType = type;
            Transform transParent = null;
            
            
            switch (windowBase.containerType)
            {
                case WindowUIContainerType.Center:
                    transParent = UISceneCtrl.Instance.CurrentUIScene.Container_Center;
                    break;
            }

            obj.transform.parent = transParent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(false);
            obj.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            StartShowWindow(windowBase, true);
        }
        else
        {
            obj = m_DicWindow[type].gameObject;
        }

        LayerUIMgr.Instance.SetLayer(obj);
        return obj;
    }

    public void CloseWindow(WindowUIType type)
    {
        if (m_DicWindow.ContainsKey(type))
        {
            StartShowWindow(m_DicWindow[type], false);
        }
    }

    private void StartShowWindow(UIWindowViewBase windowBase, bool isOpen)
    {
        switch (windowBase.showStyle)
        {
            case WindowShowStyle.Normal:
                ShowNormal(windowBase, isOpen);
                break;
            case WindowShowStyle.CenterToBig:
                ShowCenterToBig(windowBase, isOpen);
                break;
            case WindowShowStyle.FromTop:
                ShowFromDir(windowBase, 0, isOpen);
                break;
            case WindowShowStyle.FromDown:
                ShowFromDir(windowBase, 1, isOpen);
                break;
            case WindowShowStyle.FromLeft:
                ShowFromDir(windowBase, 2, isOpen);
                break;
            case WindowShowStyle.FromRight:
                ShowFromDir(windowBase, 3, isOpen);
                break;
        }
    }

    private void DestroyWindow(UIWindowViewBase windowBase)
    {
        m_DicWindow.Remove(windowBase.CurrentUIType);
        UnityEngine.Object.Destroy(windowBase.gameObject);
    }

    private void ShowNormal(UIWindowViewBase windowBase, bool isOpen)
    {
        if (isOpen) windowBase.gameObject.SetActive(true);
        else DestroyWindow(windowBase);
    }

    private void ShowCenterToBig(UIWindowViewBase windowBase, bool isOpen)
    {
        if (isOpen) windowBase.gameObject.SetActive(true);
        windowBase.transform.localScale = Vector3.zero;
        windowBase.transform.DOScale(Vector3.one, windowBase.duration)
            .Pause()
            .SetAutoKill(false)
            .SetEase(GlobalInit.Instance.UIAnimationCurve)
            .OnRewind(() => { DestroyWindow(windowBase); });
        if (isOpen)
            windowBase.transform.DOPlayForward();
        else
            windowBase.transform.DOPlayBackwards();
    }

    private void ShowFromDir(UIWindowViewBase windowBase, int dirType, bool isOpen)
    {
        if (isOpen) windowBase.gameObject.SetActive(true);
        Vector3 from = Vector3.zero;
        switch (dirType)
        {
            case 0:
                from = new Vector3(0, 1000, 0);
                break;
            case 1:
                from = new Vector3(0, -1000, 0);
                break;
            case 2:
                from = new Vector3(-1400, 0, 0);
                break;
            case 3:
                from = new Vector3(1400, 0, 0);
                break;
        }

        windowBase.transform.localPosition = from;
        windowBase.transform.DOLocalMove(Vector3.zero, windowBase.duration)
            .Pause()
            .SetAutoKill(false)
            .SetEase(GlobalInit.Instance.UIAnimationCurve)
            .OnRewind(() => { DestroyWindow(windowBase); });
        if (isOpen)
            windowBase.transform.DOPlayForward();
        else
            windowBase.transform.DOPlayBackwards();
    }
}