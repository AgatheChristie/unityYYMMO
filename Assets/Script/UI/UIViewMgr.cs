using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewMgr : Singleton<UIViewMgr>
{
  private Dictionary<WindowUIType, ISystemCtrl> m_SystemCtrlDic = new Dictionary<WindowUIType, ISystemCtrl>();


  public UIViewMgr()
  {
    m_SystemCtrlDic.Add(WindowUIType.LogOn,AccountCtrl.Instance);
    m_SystemCtrlDic.Add(WindowUIType.Reg,AccountCtrl.Instance);
    m_SystemCtrlDic.Add(WindowUIType.GameServerEnter,GameServerCtrl.Instance);
    m_SystemCtrlDic.Add(WindowUIType.GameServerSelect,GameServerCtrl.Instance);
  }
  
  public void OpenWindow(WindowUIType type)
  {
    m_SystemCtrlDic[type].OpenView(type);
  }
}
