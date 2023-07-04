using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// 管理全局的一些东西
/// </summary>
public class GameRoot : MonoBehaviour
{
    public static GameRoot instance { get; private set; }
    /// <summary>
    /// 场景管理器
    /// </summary>
    public SceneSystem sceneSystem { get; private set; }

    public static bool isOnPause;
    public KeyCode upG = KeyCode.Q;
    public KeyCode downG = KeyCode.A;
    public KeyCode upT = KeyCode.E;
    public KeyCode downT = KeyCode.D;
    public KeyCode setting = KeyCode.Escape;
    string[] key = { "upG", "downG", "upT", "downT" };
    KeyCode[] keyInMemory;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        sceneSystem = new SceneSystem();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        keyInMemory = new KeyCode[key.Length];
        sceneSystem.SetScene(new StartScene());
        GetKeySet();
    }

    private void OnGUI()
    {
        if(UIManager.singleton.GetUIState(UIManager.PANEL_KeySet) && Input.anyKeyDown)
        {
            Event e = Event.current;
            if(e!= null && e.isKey && e.keyCode != KeyCode.None)
            {
                KeyCode currentKey = e.keyCode;
                KeySetPanel keySet = (KeySetPanel)UIManager.singleton.GetUI(UIManager.PANEL_KeySet);
                int btnIndex = keySet.nowIndex;
                if (keySet.nowIndex >= 0)
                {
                    switch (keySet.nowIndex)
                    {
                        case 0:
                            upG = currentKey;
                            break;
                        case 1:
                            downG = currentKey;
                            break;
                        case 2:
                            upT = currentKey;
                            break;
                        case 3:
                            downT = currentKey;
                            break;
                    }
                }
                keySet.Update();
                //SetKey();
            }
        }
    }

    void GetKeySet()
    {
        for(int i = 0; i < key.Length; i++)
        {
            int str = PlayerPrefs.GetInt(key[i]);
            keyInMemory[i] = (KeyCode)str;
        }
        upG = keyInMemory[0] == default(KeyCode) ? KeyCode.Q : keyInMemory[0];
        downG = keyInMemory[1] == default(KeyCode) ? KeyCode.A : keyInMemory[1];
        upT = keyInMemory[2] == default(KeyCode) ? KeyCode.E : keyInMemory[2];
        downT = keyInMemory[3] == default(KeyCode) ? KeyCode.D : keyInMemory[3];
        
    }
    void SetKey()
    {
        PlayerPrefs.SetInt(key[0], MyTool.GetKeyIntValue(upG));
        PlayerPrefs.SetInt(key[1], MyTool.GetKeyIntValue(downG));
        PlayerPrefs.SetInt(key[2], MyTool.GetKeyIntValue(upT));
        PlayerPrefs.SetInt(key[3], MyTool.GetKeyIntValue(downT));
    }

    private void OnDestroy()
    {
        SetKey();
    }


    /// <summary>
    /// 暂停
    /// </summary>
    public void OnPause()
    {
        isOnPause = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public void UnPause()
    {
        isOnPause = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
