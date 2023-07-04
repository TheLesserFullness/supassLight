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
    /// 根据路径获取一个物体并实例化
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject InstantiateGameObeject(string path)
    {
        GameObject obj = Resources.Load<GameObject>(path);
        if(obj == null)
        {
            Debug.LogError($"路径{path}错误，未找到物体");
            return null;
        }
        return obj;
    }

    /// <summary>
    /// 将需要保存的资源放置到资源池中
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseObject(GameObject obj)
    {
        Destroy(obj);
    }

    /// <summary>
    /// 将不需要保存的物体直接释放
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
