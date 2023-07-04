using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : SceneState
{
    /// <summary>
    /// ��������
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
    /// �����������֮��ִ�еķ���
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    private void SceneLoaded(Scene scene,LoadSceneMode load)
    {
        uiManager = new UIManager();
        UIManager.singleton.Init();
        UIManager.singleton.OpenUI(UIManager.PANEL_Start);
        Debug.Log($"{sceneName}�����������");

    }
}
