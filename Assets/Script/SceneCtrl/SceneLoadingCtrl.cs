using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingCtrl : MonoBehaviour
{
    [SerializeField] 
    private UISceneLoadingCtrl m_UILoadingCtrl;

    private AsyncOperation m_Async = null;

    private int m_CurrProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        DelegateDefine.Instance.OnSceneLoadOk += OnSceneLoadOk;
        LayerUIMgr.Instance.Reset();
        StartCoroutine(LoadingScene());
    }

    private void OnSceneLoadOk()
    {
        if (m_UILoadingCtrl != null)
        {
            Destroy(m_UILoadingCtrl.gameObject);
        }
    }


    private void OnDestroy()
    {
        DelegateDefine.Instance.OnSceneLoadOk -= OnSceneLoadOk;
    }

    private IEnumerator LoadingScene()
    {
        string strSceneName = string.Empty;
        switch (SceneMgr.Instance.CurrentSceneType)
        {
            case SceneType.LogOn:
                strSceneName = "Scene_LogOn";
                break;
            case SceneType.SelectRole:
                strSceneName = "Scene_SelectRole";
                break;
            case SceneType.City:
                strSceneName = "GameScene_CunZhuang";
                break;
        }
        m_Async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(strSceneName);
        m_Async.allowSceneActivation = false;
        while (!m_Async.isDone)
        {
            m_UILoadingCtrl.SetProgressValue(m_Async.progress);
            if (m_Async.progress >= 0.9f)
            {
               // Debug.Log("m_CurrProgress:"+m_Async.progress);
                m_UILoadingCtrl.SetProgressValue(1);
                m_Async.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        /*
        int toProgress = 0;
        if (m_Async.progress < 0.9f)
        {
            toProgress = (int)m_Async.progress * 100;
        }
        else
        {
            toProgress = 100;
        }

        if (m_CurrProgress < toProgress)
        {
            m_CurrProgress++;
        }
        else
        {
            m_Async.allowSceneActivation = true;
        }
        Debug.Log("m_CurrProgress:"+m_CurrProgress);
        m_UILoadingCtrl.SetProgressValue(m_CurrProgress * 0.01f);
        */
    }
}