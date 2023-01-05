using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerUIMgr : Singleton<LayerUIMgr>
{
   private int m_UIViewLayer = 50;

   public void Reset()
   {
      m_UIViewLayer = 50;
   }

   public void CheckOpenWindow()
   {
      m_UIViewLayer --;
      if (UIViewUtil.Instance.OpenWindowCount == 0)
      {
         Reset();
      }
   }
   public void SetLayer(GameObject obj)
   {
      m_UIViewLayer ++;
      Canvas m_Canvas = obj.GetComponent<Canvas>();
      m_Canvas.sortingOrder = m_UIViewLayer;
   }
}
