using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool
{



    private Dictionary<string, List<MapBlock>> pool;

    private Dictionary<string, MapBlock> blockObj;

    private static ObjPool instance;
    private ObjPool()
    {
        pool = new Dictionary<string, List<MapBlock>>();
        blockObj = new Dictionary<string, MapBlock>();
    }

    public static ObjPool GetInstance()
    {
        if(instance == null)
        {
            instance = new ObjPool();
        }
        return instance;
    }


    public MapBlock GetObj(string blockClass)
    {
        MapBlock resMap;
        if (pool.ContainsKey(blockClass))
        {
            if(pool[blockClass].Count > 0)
            {
                resMap = pool[blockClass][0];
                resMap.gameObject.SetActive(true);
                pool[blockClass].Remove(resMap);
                return resMap;
            }
        }

        //表示对象池中无对象了，或者没有此类的对象池
        GameObject obj = new GameObject("starMap");
        resMap = obj.AddComponent<MapBlock>();
        resMap.MapBlockCrate();
        return resMap;
    }

    public void ReturnObj(string blockClass,MapBlock obj)
    {
        obj.gameObject.SetActive(false);
        if (pool.ContainsKey(blockClass))
        {
            pool[blockClass].Add(obj);
        }
        else
        {
            pool.Add(blockClass, new List<MapBlock>() { obj });
        }
    }

}
