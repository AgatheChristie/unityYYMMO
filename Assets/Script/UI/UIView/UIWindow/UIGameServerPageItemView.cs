//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-05-29 14:31:41
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIGameServerPageItemView : MonoBehaviour
{
    /// <summary>
    /// 页码
    /// </summary>
    private int m_PageIndex;

    /// <summary>
    /// 名称
    /// </summary>
    [SerializeField]
    private Text m_GameServerPageName;

    public Action<int> OnGameServerPageClick;

    void Start () 
	{
        GetComponent<Button>().onClick.AddListener(gameServerPageClick);
	}
    
    private void gameServerPageClick()
    {
        if (OnGameServerPageClick != null)
        {
            OnGameServerPageClick(m_PageIndex);
        }
    }

    public void SetUI(RetGameServerPageEntity entity)
    {
        m_PageIndex = entity.PageIndex;
        m_GameServerPageName.text = entity.Name;
    }
    
    void OnDestroy()
    {
        m_GameServerPageName = null;
        OnGameServerPageClick = null;
    }
}