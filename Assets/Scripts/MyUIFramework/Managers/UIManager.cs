using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储所有UI信息，并可以创建或者销毁UI
/// </summary>
public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// 存储所有UI信息的字典，每一个UI信息都会对应一个GameObject
    /// </summary>
    private Dictionary<string, UIBase> uiDict;

    /// <summary>
    /// 标识UI打开时机
    /// </summary>
    public Stack<string> uiStack;

    /// <summary>
    /// 摄像机UICamera
    /// </summary>
    //private Camera uiCamera;

    private GameObject uiRoot;
    private Transform transNormal;
    private Transform transUpper;
    private Transform transTop;



    //以下是一些基础的层级位置信息
    public GameObject UIRoot { get { return uiRoot; } }
    public Transform TransNormal { get { return transNormal; } }
    public Transform TransUpper { get { return transUpper; } }
    public Transform TransTop { get { return transTop; } }


    public UIManager() : base() { }


    public override bool Init()
    {
        return InitUIInfo() && UIRegister();
    }

    /// <summary>
    /// 初始化一些基础物体
    /// </summary>
    /// <returns></returns>
    public bool InitUIInfo()
    {
        //ObjectManager objMng = new ObjectManager();
        GameObject ObjectManager = new GameObject("ObjectManager");
        ObjectManager.AddComponent<ObjectManager>();
        uiDict = new Dictionary<string, UIBase>();
        uiStack = new Stack<string>();
        //uiRoot = ObjectManager.singleton.InstantiateGameObeject(PathUtils.UI_Root_PATH);
        uiRoot = GameObject.Find("UIRoot");
        if (uiRoot == null)
        {
            Debug.Log("初始化UIManager 失败了~");
            return false;
        }
        uiRoot.SetActive(true);
        transNormal = uiRoot.transform.Find("CanvasMain");
        transUpper = uiRoot.transform.Find("CanvasPopOut_1");
        transTop = uiRoot.transform.Find("CanvasPopOut_2");
        transTop.GetComponent<Canvas>().sortingOrder = 200;
        GameObject.DontDestroyOnLoad(uiRoot);
        return true;
    }

    public const string PANEL_Setting = "PanelSetting.prefab";
    public const string PANEL_Start = "PanelStart.prefab";
    public const string PANEL_KeySet = "PanelKeySet.prefab";
    public const string PANEL_Loading = "PanelLoding.prefab";
    public const string PANEL_Died = "PanelDied.prefab";
    public const string PANEL_RealTimeData = "PanelRealTimeData.prefab";

    /// <summary>
    /// 在C#层实现逻辑的UI进行注册注册 
    /// </summary>
    /// <returns></returns>
    private bool UIRegister()
    {
        uiDict.Add(PANEL_Start, new StartPanel());
        uiDict.Add(PANEL_Setting, new SettingPanel());
        uiDict.Add(PANEL_KeySet, new KeySetPanel());
        uiDict.Add(PANEL_Loading, new LoadingPanel());
        uiDict.Add(PANEL_Died, new DiedPanel());
        uiDict.Add(PANEL_RealTimeData, new RealTimeDataPanel());
        return true;
    }


    public override void UnInit()
    {
        if (uiRoot)
        {
            ObjectManager.singleton.ReleaseObjectComopletly(uiRoot);
            uiRoot = null;
            transNormal = null;
            transUpper = null;
        }
    }

    public override void OnLogOut()
    {
        base.OnLogOut();
    }


    /// <summary>
    /// 打开一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase OpenUI(string uiName)
    {
        UIBase UIBase = GetUI(uiName);
        if (UIBase == null)
        {
            Debug.LogError($"无法打开{uiName}，UIDic里面没有这个UI信息 UIName：" + uiName);
            return null;
        }

        if (!UIBase.IsInited)
        {
            UIBase.Init();
        }
        if (UIBase.Active)
        {
            return UIBase;
        }
        UIBase.Active = true;
        uiStack.Push(uiName);
        return UIBase;
    }

    /// <summary>
    /// 关闭一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    public UIBase CloseUI(string uiName)
    {
        UIBase UIBase = GetUI(uiName);
        if (UIBase == null)
        {
            Debug.LogError($"无法关闭{uiName}，UIDic里面没有这个UI信息 UIName：" + uiName);
            return UIBase;
        }

        if (UIBase.IsInited)
        {

            UIBase.Active = false;

            //UIBase.Uninit();
        }
        if (uiStack.Count > 0)
        {
            uiStack.Pop();
        }
        
        return UIBase;
    }

    /// <summary>
    /// 获取一个UI的接口
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase GetUI(string uiName)
    {
        UIBase UIBase = null;
        uiDict.TryGetValue(uiName, out UIBase);
        return UIBase;
    }

    /// <summary>
    /// 根据名字获取UI状态
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public bool GetUIState(string uiName)
    {
        return GetUI(uiName).Active;
    }

    public T GetUI<T>(string uiName) where T : UIBase
    {
        UIBase UIBase = null;
        if (uiDict.TryGetValue(uiName, out UIBase))
        {
            if (UIBase is T)
            {
                return (T)UIBase;
            }
        }
        return null;
    }

    /// <summary>
    /// 关闭所有UI的接口
    /// </summary>
    public void CloseAll()
    {
        foreach (KeyValuePair<string, UIBase> pair in uiDict)
        {
            CloseUI(pair.Key);
        }
    }

    /// <summary>
    /// 注销方法
    /// </summary>
    public void OnLogout()
    {
        foreach (var UIBase in uiDict.Values)
        {
            UIBase.OnLogOut();
        }
/*        if (uiCamera)
        {
            uiCamera.enabled = false;
        }*/
    }
}
