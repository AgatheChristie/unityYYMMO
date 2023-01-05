using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{
    public SceneType CurrentSceneType { get; private set; }

    public void LoadToLogOn()
    {
        CurrentSceneType = SceneType.LogOn;

        SceneManager.LoadScene("Scene_Loading");
    }

    public void LoadToSelectRole()
    {
        CurrentSceneType = SceneType.SelectRole;
        SceneManager.LoadScene("Scene_Loading");
    }
    
    public void LoadToCity()
    {
        CurrentSceneType = SceneType.City;
        SceneManager.LoadScene("Scene_Loading");
    }
}