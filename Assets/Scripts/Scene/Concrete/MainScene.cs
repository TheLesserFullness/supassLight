using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : SceneState
{

    /// <summary>
    /// ��������
    /// </summary>
    readonly string sceneName = "Main";
    UIManager uiManager;


    public override void OnEnter()
    {

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            uiManager = new UIManager();
            UIManager.singleton.Init();
            //panelManager.Push();
            UIManager.singleton.OpenUI(UIManager.PANEL_Loading);
        }
        //TriggerAndCacu tr = Camera.main.GetComponent<TriggerAndCacu>();
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
    private void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        uiManager = new UIManager();
        UIManager.singleton.Init();
        //panelManager.Push();
        UIManager.singleton.OpenUI(UIManager.PANEL_Loading);
        //Debug.Log($"{sceneName}�����������");

    }
}
