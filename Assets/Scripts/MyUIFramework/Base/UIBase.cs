using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase 
{
    /// <summary>
    /// 是否加载完成的标志位
    /// </summary>
    protected bool isInited;

    /// <summary>
    /// UI名字
    /// </summary>
    protected string uiName;

    /// <summary>
    /// 在关闭的时候是否缓存UI 默认不缓存
    /// </summary>
    protected bool isCatchUI = false;

    /// <summary>
    /// UI的实例化GamObejct
    /// </summary>
    protected GameObject uiGameObject;

    /// <summary>
    /// 设置UI可见性状态
    /// </summary>
    protected bool active = false;

    /// <summary>
    /// 加载完成的回调
    /// </summary>
    protected GameObjectLoadedCallBack m_callBack;

    /// <summary>
    /// UI的资源全路径
    /// </summary>
    protected string uiFullPath = "";

    /// <summary>
    /// UILayerType UI层类型
    /// </summary>
    protected UILayerType uiLayerType;

    /// <summary>
    /// UI的加载方式
    /// </summary>
    protected UILoadType uiLoadType;

    /// <summary>
    /// 获取或设置UI名字，只需要传预制体的名字即可
    /// </summary>
    public string UIName
    {
        get { return uiName; }
        set
        {
            //uiName = value.EndsWith(PathUtils.UI_Prefab_SUFFIX) ? uiName = value : uiName = value + PathUtils.UI_Prefab_SUFFIX;
            //uiFullPath = PathUtils.UI_MainPath + "/" + uiName;
            if (value.EndsWith(PathUtils.UI_Prefab_SUFFIX))
            {
                uiName = value;
            }
            else
            {
                uiFullPath = PathUtils.UI_MainPath + "/" + value;
                uiName = value + PathUtils.UI_Prefab_SUFFIX;
            }
        }
    }

    /// <summary>
    /// 设置或获取UI是否缓存
    /// </summary>
    public bool IsCatchUI
    {
        get { return isCatchUI; }
        set
        {
            isCatchUI = value;
        }
    }

    public GameObject UIGameObject
    {
        get { return uiGameObject; }
        set { uiGameObject = value; }
    }

    /// <summary>
    /// 获取或设置UI的显隐状态，设置并调用对应的方法
    /// </summary>
    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            if (uiGameObject != null)
            {
                uiGameObject.SetActive(value);
                if (uiGameObject.activeSelf)
                {
                    OnShow();
                }
                else
                {
                    OnHide();
                }
            }
        }
    }

    /// <summary>
    /// 获取UI是否已经初始化过了
    /// </summary>
    public bool IsInited { get { return isInited; } }



    /// <summary>
    /// UI基类构造函数
    /// </summary>
    /// <param name="name">UI预制体的名字</param>
    /// <param name="layerType">UI需要放到哪个层级</param>
    /// <param name="loadType">UI是否异步加载</param>
    protected UIBase(string name, UILayerType layerType, UILoadType loadType = UILoadType.SyncLoad)
    {
        UIName = name;
        uiLayerType = layerType;
        uiLoadType = loadType;
    }

    /// <summary>
    /// UI初始化
    /// </summary>
    public virtual void Init()
    {

        if (uiLoadType == UILoadType.SyncLoad)
        {
            Debug.LogWarning($"{uiName}页面加载，加载路径{uiFullPath}");
            GameObject obj = ObjectManager.singleton.InstantiateGameObeject(uiFullPath);
            OnGameObjectLoaded(obj);
        }
        else
        {
            GameObject obj = ObjectManager.singleton.InstantiateGameObeject(uiFullPath);
            OnGameObjectLoaded(obj);
        }
    }

    /// <summary>
    /// 加载物体并对部分数据初始化
    /// </summary>
    /// <param name="uiObj"></param>
    private void OnGameObjectLoaded(GameObject prefabObj)
    {
        if (prefabObj == null)
        {
            Debug.LogError($"UI加载失败，请检查路径是否正确：{uiFullPath}");
            return;
        }
        SetPanetByLayerType(uiLayerType, prefabObj);
        isInited = true;
        uiGameObject.transform.localPosition = Vector3.zero;
        uiGameObject.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 资源回收
    /// </summary>
    public virtual void Uninit()
    {
        isInited = false;
        active = false;
        if (isCatchUI)
        {
            //资源并加入到资源池
            ObjectManager.singleton.ReleaseObject(uiGameObject);
        }
        else
        {
            //彻底清除Object资源
            ObjectManager.singleton.ReleaseObject(uiGameObject, true);
        }
    }

    /// <summary>
    /// UI显示时调用此函数
    /// </summary>
    protected abstract void OnShow();

    /// <summary>
    /// UI隐藏时调用此函数
    /// </summary>
    protected abstract void OnHide();

    public virtual void Update()
    {

    }

    public virtual void LateUpdate(float deltaTime)
    {

    }


    public virtual void OnLogOut()
    {
        if (IsCatchUI)
        {
            ObjectManager.singleton.ReleaseObject(this.UIGameObject);
        }
        else
        {
            ObjectManager.singleton.ReleaseObjectComopletly(this.uiGameObject);
        }
    }

    /// <summary>
    /// 将UI放到指定层级
    /// </summary>
    /// <param name="layerType"></param>
    protected void SetPanetByLayerType(UILayerType layerType,GameObject prefabObj)
    {
        switch (uiLayerType)
        {
            case UILayerType.Upper:
                uiGameObject= GameObject.Instantiate(prefabObj, UIManager.singleton.TransUpper);
                break;
            case UILayerType.Normal:
                uiGameObject = GameObject.Instantiate(prefabObj, UIManager.singleton.TransNormal);
                break;
            case UILayerType.Top:
                uiGameObject = GameObject.Instantiate(prefabObj, UIManager.singleton.TransTop);
                break;
        }
    }
}

//GameObject Loaded CallBack 物体加载回掉
public delegate void GameObjectLoadedCallBack(GameObject obj);

/// <summary>
/// UI层级
/// </summary>
public enum UILayerType
{
    Upper,
    Normal,
    Top,
}

/// <summary>
/// 加载UI的方式
/// </summary>
public enum UILoadType
{
    SyncLoad,
    AsyncLoad,
}
