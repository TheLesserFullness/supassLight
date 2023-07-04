using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : SceneState
{
    /// <summary>
    /// 场景名称
    /// </summary>
    readonly string sceneName = "StartScene";
    UIManager uiManager;


    public override void OnEnter()
    {


        //SceneManager.LoadScene(0);
        if(SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(0);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            uiManager = new UIManager();
            UIManager.singleton.Init();
            UIManager.singleton.OpenUI(UIManager.PANEL_Start);
        }

    }

    public override void OnExit()
    {
        UIManager.singleton.CloseAll();
        SceneManager.sceneLoaded -= SceneLoaded;
        
    }

    /// <summary>
    /// 场景加载完毕之后执行的方法
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    private void SceneLoaded(Scene scene,LoadSceneMode load)
    {
        uiManager = new UIManager();
        UIManager.singleton.Init();
        UIManager.singleton.OpenUI(UIManager.PANEL_Start);
        Debug.Log($"{sceneName}场景加载完毕");

    }
}
