using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase 
{
    /// <summary>
    /// �Ƿ������ɵı�־λ
    /// </summary>
    protected bool isInited;

    /// <summary>
    /// UI����
    /// </summary>
    protected string uiName;

    /// <summary>
    /// �ڹرյ�ʱ���Ƿ񻺴�UI Ĭ�ϲ�����
    /// </summary>
    protected bool isCatchUI = false;

    /// <summary>
    /// UI��ʵ����GamObejct
    /// </summary>
    protected GameObject uiGameObject;

    /// <summary>
    /// ����UI�ɼ���״̬
    /// </summary>
    protected bool active = false;

    /// <summary>
    /// ������ɵĻص�
    /// </summary>
    protected GameObjectLoadedCallBack m_callBack;

    /// <summary>
    /// UI����Դȫ·��
    /// </summary>
    protected string uiFullPath = "";

    /// <summary>
    /// UILayerType UI������
    /// </summary>
    protected UILayerType uiLayerType;

    /// <summary>
    /// UI�ļ��ط�ʽ
    /// </summary>
    protected UILoadType uiLoadType;

    /// <summary>
    /// ��ȡ������UI���֣�ֻ��Ҫ��Ԥ��������ּ���
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
    /// ���û��ȡUI�Ƿ񻺴�
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
    /// ��ȡ������UI������״̬�����ò����ö�Ӧ�ķ���
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
    /// ��ȡUI�Ƿ��Ѿ���ʼ������
    /// </summary>
    public bool IsInited { get { return isInited; } }



    /// <summary>
    /// UI���๹�캯��
    /// </summary>
    /// <param name="name">UIԤ���������</param>
    /// <param name="layerType">UI��Ҫ�ŵ��ĸ��㼶</param>
    /// <param name="loadType">UI�Ƿ��첽����</param>
    protected UIBase(string name, UILayerType layerType, UILoadType loadType = UILoadType.SyncLoad)
    {
        UIName = name;
        uiLayerType = layerType;
        uiLoadType = loadType;
    }

    /// <summary>
    /// UI��ʼ��
    /// </summary>
    public virtual void Init()
    {

        if (uiLoadType == UILoadType.SyncLoad)
        {
            Debug.LogWarning($"{uiName}ҳ����أ�����·��{uiFullPath}");
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
    /// �������岢�Բ������ݳ�ʼ��
    /// </summary>
    /// <param name="uiObj"></param>
    private void OnGameObjectLoaded(GameObject prefabObj)
    {
        if (prefabObj == null)
        {
            Debug.LogError($"UI����ʧ�ܣ�����·���Ƿ���ȷ��{uiFullPath}");
            return;
        }
        SetPanetByLayerType(uiLayerType, prefabObj);
        isInited = true;
        uiGameObject.transform.localPosition = Vector3.zero;
        uiGameObject.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// ��Դ����
    /// </summary>
    public virtual void Uninit()
    {
        isInited = false;
        active = false;
        if (isCatchUI)
        {
            //��Դ�����뵽��Դ��
            ObjectManager.singleton.ReleaseObject(uiGameObject);
        }
        else
        {
            //�������Object��Դ
            ObjectManager.singleton.ReleaseObject(uiGameObject, true);
        }
    }

    /// <summary>
    /// UI��ʾʱ���ô˺���
    /// </summary>
    protected abstract void OnShow();

    /// <summary>
    /// UI����ʱ���ô˺���
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
    /// ��UI�ŵ�ָ���㼶
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

//GameObject Loaded CallBack ������ػص�
public delegate void GameObjectLoadedCallBack(GameObject obj);

/// <summary>
/// UI�㼶
/// </summary>
public enum UILayerType
{
    Upper,
    Normal,
    Top,
}

/// <summary>
/// ����UI�ķ�ʽ
/// </summary>
public enum UILoadType
{
    SyncLoad,
    AsyncLoad,
}
