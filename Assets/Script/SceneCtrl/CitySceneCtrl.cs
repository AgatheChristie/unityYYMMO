using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySceneCtrl : MonoBehaviour
{
    
    
    [SerializeField]
    private Transform m_PlayerBornPos;
    
    
    
    
    private void Awake()
    {
       // SceneUIMgr.Instance.LoadSceneUI(SceneUIMgr.SceneUIType.MainCity);
       
       // if (FingerEvent.Instance != null)
       // {
       //     FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
       //     FingerEvent.Instance.OnZoom += OnZoom;
       //     FingerEvent.Instance.OnPlayerClick += OnPlayerClick;
       // }
       
       
    }
   

    private void Start()
    {
        
        if (DelegateDefine.Instance.OnSceneLoadOk != null)
        {
            DelegateDefine.Instance.OnSceneLoadOk();
        }
        if (GlobalInit.Instance == null) return;

    }
}
