using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �洢����UI��Ϣ�������Դ�����������UI
/// </summary>
public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// �洢����UI��Ϣ���ֵ䣬ÿһ��UI��Ϣ�����Ӧһ��GameObject
    /// </summary>
    private Dictionary<string, UIBase> uiDict;

    /// <summary>
    /// ��ʶUI��ʱ��
    /// </summary>
    public Stack<string> uiStack;

    /// <summary>
    /// �����UICamera
    /// </summary>
    //private Camera uiCamera;

    private GameObject uiRoot;
    private Transform transNormal;
    private Transform transUpper;
    private Transform transTop;



    //������һЩ�����Ĳ㼶λ����Ϣ
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
    /// ��ʼ��һЩ��������
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
            Debug.Log("��ʼ��UIManager ʧ����~");
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
    /// ��C#��ʵ���߼���UI����ע��ע�� 
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
    /// ��һ��UI�Ľӿ�
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIBase OpenUI(string uiName)
    {
        UIBase UIBase = GetUI(uiName);
        if (UIBase == null)
        {
            Debug.LogError($"�޷���{uiName}��UIDic����û�����UI��Ϣ UIName��" + uiName);
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
    /// �ر�һ��UI�Ľӿ�
    /// </summary>
    /// <param name="uiName"></param>
    public UIBase CloseUI(string uiName)
    {
        UIBase UIBase = GetUI(uiName);
        if (UIBase == null)
        {
            Debug.LogError($"�޷��ر�{uiName}��UIDic����û�����UI��Ϣ UIName��" + uiName);
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
    /// ��ȡһ��UI�Ľӿ�
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
    /// �������ֻ�ȡUI״̬
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
    /// �ر�����UI�Ľӿ�
    /// </summary>
    public void CloseAll()
    {
        foreach (KeyValuePair<string, UIBase> pair in uiDict)
        {
            CloseUI(pair.Key);
        }
    }

    /// <summary>
    /// ע������
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
