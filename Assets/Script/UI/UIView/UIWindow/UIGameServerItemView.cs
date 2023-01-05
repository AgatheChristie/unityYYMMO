//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-05-30 20:40:21
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIGameServerItemView : MonoBehaviour
{
    /// <summary>
    /// 服务器状态
    /// </summary>
    [SerializeField]
    private Sprite[] m_GameServerStatus;

    /// <summary>
    /// 当前的服务器状态
    /// </summary>
    [SerializeField]
    private Image m_CurrGameServerStatus;

    /// <summary>
    /// 服务器名称
    /// </summary>
    [SerializeField]
    private Text m_GameServerName;

    private RetGameServerEntity m_CurrGameServerData;

    public Action<RetGameServerEntity> OnGameServerClick;

    void Start () 
	{
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(GameServerClick);
        }
	}

    private void GameServerClick()
    {
        if (OnGameServerClick != null) OnGameServerClick(m_CurrGameServerData);
    }

    public void SetUI(RetGameServerEntity entity)
    {
        m_CurrGameServerData = entity;
        m_CurrGameServerStatus.overrideSprite = m_GameServerStatus[entity.RunStatus];
        m_GameServerName.text = entity.Name;
    }

    void OnDestroy()
    {
        m_GameServerStatus.SetNull();
        m_CurrGameServerStatus = null;
        m_GameServerName = null;
        m_CurrGameServerData = null;
        OnGameServerClick = null;
    }
}