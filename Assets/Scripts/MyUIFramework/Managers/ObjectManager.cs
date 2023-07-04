using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
     static ObjectManager _singleton;


    public static ObjectManager singleton
    {
        get
        {
/*            if (_singleton == null)
            {
                GameObject obj = new GameObject();
                _singleton = obj.GetComponent<ObjectManager>();
            }*/

            return _singleton;
        }
    }
    private void Awake()
    {
        if (_singleton == null)
        {
            _singleton = gameObject.GetComponent<ObjectManager>();
        }
    }

    /// <summary>
    /// ����·����ȡһ�����岢ʵ����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject InstantiateGameObeject(string path)
    {
        GameObject obj = Resources.Load<GameObject>(path);
        if(obj == null)
        {
            Debug.LogError($"·��{path}����δ�ҵ�����");
            return null;
        }
        return obj;
    }

    /// <summary>
    /// ����Ҫ�������Դ���õ���Դ����
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseObject(GameObject obj)
    {
        Destroy(obj);
    }

    /// <summary>
    /// ������Ҫ���������ֱ���ͷ�
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="bl"></param>
    public void ReleaseObject(GameObject obj, bool bl)
    {
        Destroy(obj);
    }

    public void ReleaseObjectComopletly(GameObject obj)
    {
        Destroy(obj);
    }
}
