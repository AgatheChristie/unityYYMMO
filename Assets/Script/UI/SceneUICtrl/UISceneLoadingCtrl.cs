using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneLoadingCtrl  : UIViewBase
{
   public Scrollbar m_Progress;
   public Text m_LblProgress;
   public Image m_SprProgressLight;

   public void SetProgressValue(float value)
   {
      if (m_Progress == null || m_LblProgress == null ) return;
     
      m_Progress.size = value;
      m_LblProgress.text = string.Format("{0}%",(int)(value * 100));
   }

   protected override void BeforeOnDestroy()
   {
      base.BeforeOnDestroy();
      m_Progress = null;
      m_LblProgress = null;
      m_SprProgressLight = null;

   }
}
