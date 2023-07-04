using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;


public class TriggerAndCacu : MonoBehaviour
{
    public static TriggerAndCacu sigleton;

    public int mapCount = 4;
    /// <summary>
    /// 引力常量缩放系数
    /// </summary>
    public float gScale = 1;
    /// <summary>
    /// 时间流速系数
    /// </summary>
    public float timeScale = 1;
    /// <summary>
    /// 主角位置
    /// </summary>
    public  Vector3 mainPos;
    /// <summary>
    /// 主角预制体
    /// </summary>
    public GameObject mainObj;
    /// <summary>
    /// 本体
    /// </summary>
    public GameObject _mainObj;
    /// <summary>
    /// 能对主角产生引力的最大距离
    /// </summary>
    public float forceMaxDis = 20;
    /// <summary>
    /// 引力队列
    /// </summary>
    public List<Star> starInRange;
    /// <summary>
    /// 背景星的预制体
    /// </summary>
    public GameObject bgStar;
    /// <summary>
    /// 所有星体的材料
    /// </summary>
    public Material[] materials;
    /// <summary>
    /// 默认背景材料
    /// </summary>
    public Material bgStarMaterial;
    /// <summary>
    /// 对象池
    /// </summary>
    public ObjPool objPool;
    

    /// <summary>
    /// 最终合力向量,包含方向和大小
    /// </summary>
    Vector3 resultantGravit = Vector3.zero;
    /// <summary>
    /// 当前存在的星图队列
    /// </summary>
    public List<MapBlock> existMap;

    /// <summary>
    /// 当前屏幕的边界范围(x,y);x<y
    /// </summary>
    public Vector2 screenXBound,screenYBound;
    /// <summary>
    /// 是否需要新的图
    /// </summary>
    public bool needNewMap = false;
    /// <summary>
    /// 当前是否允许生成新的星图
    /// </summary>
    public bool allowNewMap = false;
    /// <summary>
    /// 新的星图是否已经创建满了，如果已经创建满了，那么将不再允许新的星图创建，直到主图产生切换时刷新
    /// </summary>
    public bool newMapCreated = false;
    /// <summary>
    /// 第一位表示当前存在星图数量（大于0）
    /// </summary>
    public int[] creatSign;
    /// <summary>
    /// 主图，当整个视野都在主图中时，其他图将被回收
    /// </summary>
    public MapBlock nowInside;

    public GameConfig gameConfig;

    /// <summary>
    /// 经过合并计算后的速度值
    /// </summary>
    public float realSpeed;
    /// <summary>
    /// 带方向信息的速度值
    /// </summary>
    public Vector3 speed;
    /// <summary>
    /// 所有引力的合力
    /// </summary>
    public Vector3 mergeForce;
    /// <summary>
    /// 根据增益系数模拟计算的加速度
    /// </summary>
    public Vector3 acc;




    bool alive = true;

    private void Awake()
    {
        if(sigleton == null)
        {
            sigleton = gameObject.GetComponent<TriggerAndCacu>(); 
        }
        //UIManager.singleton.OpenUI(UIManager.PANEL_Loading);
        ResourcesLoad();
        //UIManager.singleton.CloseUI(UIManager.PANEL_Loading);
    }
    /// <summary>
    /// 加载所有资源
    /// </summary>
    void ResourcesLoad()
    {

        bgStarMaterial = Resources.Load<Material>("Marterial/BGStar");
        materials = new Material[8];
        materials[7] = Resources.Load<Material>("Marterial/RedStar_1Pass");
        materials[6] = Resources.Load<Material>("Marterial/BlueGiant_1Pass");
        materials[5] = Resources.Load<Material>("Marterial/GoldStar_1Pass");
        materials[4] = Resources.Load<Material>("Marterial/GoldStar_2Pass");
        materials[3] = Resources.Load<Material>("Marterial/MainStar_1Pass");
        materials[2] = Resources.Load<Material>("Marterial/MainStar_2Pass");
        materials[1] = Resources.Load<Material>("Marterial/RedGiant_2Pass");
        materials[0] = Resources.Load<Material>("Marterial/WhiteDwarf_1");


        bgStar = Resources.Load<GameObject>("Prefabs/StarPrefab/PrefabBGStar");
        mainObj = Resources.Load<GameObject>("Prefabs/StarPrefab/Nimbusist_1");

        GameConfigLoad();
    }

    /// <summary>
    /// 根据配置获取初始值并创见物体
    /// </summary>
    private void GameConfigLoad()
    {
        objPool = ObjPool.GetInstance();
/*        gameConfig = Resources.Load<GameConfig>(ConstGameData.savePath);
        if (gameConfig)
        {
            upG = gameConfig.upG == KeyCode.None ? KeyCode.A : gameConfig.upG;
            downG = gameConfig.downG == KeyCode.None ? KeyCode.A : gameConfig.downG;
            upT = gameConfig.upT == KeyCode.None ? KeyCode.A : gameConfig.upT;
            downT = gameConfig.downT == KeyCode.None ? KeyCode.A : gameConfig.downT;
            timeScale = gameConfig.initialTimeScale;
            gScale = gameConfig.initialGScale;
        }*/
        for(int i= 0; i < mapCount; i++)
        {
            GameObject mapObj = new GameObject("starMap" + i);
            MapBlock map = mapObj.AddComponent<MapBlock>();
            map.MapBlockCrate();
            objPool.ReturnObj(ConstGameData.blockMap, map);
        }
    }

    public void End()
    {
        
        alive = false;
        foreach (MapBlock map in existMap)
        {
            map.RefreshStarPos();
            map.OutOfView = false;
            map.allowDisroy = false;
            map.viewAccrossed = false;
            objPool.ReturnObj(ConstGameData.blockMap, map);
        }
        starInRange = new List<Star>();
        existMap = new List<MapBlock>();
        _mainObj.gameObject.SetActive(false);
        UIManager.singleton.OpenUI(UIManager.PANEL_Died);
    }

    public void ReStart()
    {
        gScale = 1;
        timeScale = 1;
        _mainObj.gameObject.SetActive(true);
        MapBlock mb = objPool.GetObj(ConstGameData.blockMap);
        mb.Place(Vector3.zero);
        nowInside = mb;

        existMap.Add(mb);
        Camera.main.transform.position = mb.gameObject.transform.position + new Vector3(mb.width / 2, mb.height / 2, -30);
        creatSign = new int[4];
        allowNewMap = false;
        newMapCreated = false;
        SignArrReset();
        alive = true;
        speed = new Vector3(3, 3, 0);
    }



    void Start()
    {
        speed = new Vector3(3, 3, 0);
        creatSign = new int[4];
        starInRange = new List<Star>();
        MapBlock mb = objPool.GetObj(ConstGameData.blockMap);
        nowInside = mb;
        existMap = new List<MapBlock>();
        existMap.Add(mb);
        Camera.main.transform.position = mb.gameObject.transform.position + new Vector3(mb.width / 2, mb.height / 2, -30);
        _mainObj = Instantiate(mainObj);
        _mainObj.transform.SetParent(Camera.main.gameObject.transform);
        _mainObj.transform.localPosition = new Vector3(0, 0, 5);

        UIManager.singleton.CloseUI(UIManager.PANEL_Loading);
        UIManager.singleton.OpenUI(UIManager.PANEL_RealTimeData);

        SignArrReset();
    }

    void KeyDown()
    {
        if (Input.GetKey(GameRoot.instance.upG))
        {
            gScale++;
        }
        if (Input.GetKey(GameRoot.instance.downG))
        {
            gScale--;
        }
        if (Input.GetKey(GameRoot.instance.upT))
        {
            timeScale++;
        }
        if (Input.GetKey(GameRoot.instance.downT))
        {
            timeScale--;
        }

    }

    void Setting()
    {
        /*if (Input.GetKeyDown(GameRoot.instance.setting))
        {
            if (!UIManager.singleton.GetUIState(UIManager.PANEL_Setting))
            {
                UIManager.singleton.OpenUI(UIManager.PANEL_Setting);
            }
            string ui = UIManager.singleton.uiStack.Peek();
            if (UIManager.singleton.GetUIState(UIManager.PANEL_Setting) && UIManager.singleton.uiStack.Peek() == UIManager.PANEL_Setting)
            {
                UIManager.singleton.CloseUI(UIManager.PANEL_Setting);
            }

        }*/
    }

    void Update()
    {
        Setting();
        if (alive && !GameRoot.isOnPause)
        {
            KeyDown();
            gameObject.transform.position += Time.deltaTime * speed * (1 + MyTool.LimitValue(timeScale,-95,1000) / 100f);
            mainPos = Camera.main.transform.position;
            screenBoundGet();
            ViewStateRecheck();
            if (allowNewMap)
            {
                OutBoundOperate();
            }


            if (starInRange.Count >= 0)
            {
                foreach (Star star in starInRange)
                {
                    if (star.Crashed())
                    {
                        End();
                    }
                }
                mergeForce =  ResultForceGet(AllForceGet(starInRange));
                acc = Time.deltaTime * MyTool.GetV3SpeedInXOY(mergeForce,0.5f);
                speed += acc;
                //Debug.Log("-----当前合理："+ mergeForce.ToString()+ "-----当前加速度："+acc.ToString());
                //Debug.Log(resultantGravit + "<---resultforce---------------------");
                //Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + resultantGravit, Color.red);
            }
            UIManager.singleton.GetUI(UIManager.PANEL_RealTimeData).Update();
        }
        


    }

    /// <summary>
    /// 当主星图（nowInside）离开视野后，重新检视星图，并刷新状态，更换新的主图【此时最多只可能有两个图同时存在于视野中】
    /// </summary>
    private void ViewStateRecheck()
    {
        //StarMapCheck(nowInside);
        

        if (StarMapCheck(nowInside))
        {
            StarMapCheck(nowInside);
            for(int i = 0; i < existMap.Count; i++)
            {
                StarMapCheck(existMap[i]);
                //Debug.Log("-exist length---------====== viewCheck"+i+"--shuliang--" + existMap.Count+"--"+ existMap[i].allowDisroy);
            }
            MapArrDel();
            SignArrReset();

            if (existMap.Count < 2)
            {
                
                //表明此时视野中仅存在一个星图，即视野不在两星图的交界处
                nowInside = existMap[0];
            }
            else
            {
                //creatSign[0] = 1;
                Vector3 pos0 = existMap[0].gameObject.transform.position;
                Vector3 pos1 = existMap[1].gameObject.transform.position;
                if (pos0.x == pos1.x)
                {
                    if (pos0.y > pos1.y)
                    {
                        nowInside = existMap[1];
                    }
                    else
                    {
                        nowInside = existMap[0];
                    }
                    SignArrWrite(3);

                }
                else
                {
                    if (pos0.x > pos1.x)
                    {
                        nowInside = existMap[1];
                    }
                    else
                    {
                        nowInside = existMap[0];
                    }
                    SignArrWrite(5);
                }
            }
            
        }
    }

    /// <summary>
    /// 对一个星图的标识数据进行修改，（是否可以回收，是否在屏幕内，是否穿过过视野）
    /// </summary>
    /// <param name="map">待检测图</param>
    private bool StarMapCheck(MapBlock map)
    {
        Vector3 mapPos = map.gameObject.transform.position;
        Vector3 cameraPos = Camera.main.transform.position;
        //如果存在屏幕上界小于星图下界或者屏幕下界大于星图上界，则表明该图不在屏幕内
        //且如果该图已经被视野穿过了，则该图是可以销毁的了
        Vector2 xDif = map.xBound - screenXBound;
        Vector2 yDif = map.yBound - screenYBound;

        map.OutOfView = false;
        map.allowDisroy = false;

        if ((map.xRecycle.x > screenXBound.y || map.xRecycle.y < screenXBound.x) ||
            (map.yRecycle.x > screenYBound.y || map.yRecycle.y < screenYBound.x))
        {
            map.OutOfView = true;
        }
        if ((map.xBound.x < screenXBound.x && map.xBound.y > screenXBound.y) || (map.yBound.x < screenYBound.x && map.yBound.y > screenYBound.y))
        {
            map.viewAccrossed = true;
        }
        if (map.viewAccrossed && map.OutOfView)
        {
            map.allowDisroy = true;
        }
        return map.OutOfView && map.viewAccrossed && map.allowDisroy;
    }

    /// <summary>
    /// 当视野到达当前主图的创建范围时，判断需要生成几个星图，并在对应的位置上放置星图
    /// </summary>
    public void OutBoundOperate()
    {
        IsNearMapBound();
        if(creatSign[0] > 0)
        {
            if(creatSign[0] == 1 && !newMapCreated)
            {
                MapCreate(creatSign[0], creatSign[creatSign[0]] - 1);
            }
            if(creatSign[0] > 1)
            {
                if (newMapCreated)
                {
                    if (creatSign[1] > creatSign[2])
                    {
                        MapCreate(2, creatSign[2] - 1);
                    }
                    else
                    {
                        MapCreate(2, creatSign[2] - 2);
                    }

                }
                else
                {
                    if (creatSign[1] > creatSign[2])
                    {
                        MapCreate(2, creatSign[2] - 1);
                    }
                    else
                    {
                        MapCreate(2, creatSign[1] - 1);
                    }
                }
                allowNewMap = false;
            }
        }
    }

    /// <summary>
    /// 判断当前是否需要生成星图，并写入位置标记
    /// </summary>
    public void IsNearMapBound()
    {
        //不需要生成星图时有xDif（-，+）；yDif（-，+），即星图上界大于屏幕上界，星图下界小于屏幕下界
        Vector2 xDif = nowInside.xBound - screenXBound;
        Vector2 yDif = nowInside.yBound - screenYBound;
        if (yDif.x >= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 7))
        {
            //到达当前星图顶部界线，在当前星图上方添加星图
            //Debug.Log("--------上");
            SignArrWrite(7);
        }
        if (yDif.y <= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 3))
        {
            //到达当前星图底部界线，在当前星图下方添加星图
            //Debug.Log("--------下");
            SignArrWrite(3);
        }
        if (xDif.x >= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 1))
        {
            //到达当前星图左侧界线，在当前星图左方方添加星图
            //Debug.Log("--------左");
            SignArrWrite(1);
        }
        if (xDif.y <= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 5))
        {
            //到达当前星图右侧界线，在当前星图右方添加星图
            //Debug.Log("--------右");
            SignArrWrite(5);
        }

    }

    /// <summary>
    /// 对视野外的星图进行回收
    /// </summary>
    void MapArrDel()
    {
        for (int i = 0;i<existMap.Count; i++)
        {
            if (existMap[i].allowDisroy)
            {
                existMap[i].RefreshStarPos();
                existMap[i].OutOfView = false;
                existMap[i].allowDisroy = false;
                existMap[i].viewAccrossed = false;

                objPool.ReturnObj(ConstGameData.blockMap, existMap[i]);
                existMap.Remove(existMap[i]);
                i--;
            }
        }
    }

    /// <summary>
    /// 写入创建标记，表明当前所有存在星图的结构
    /// </summary>
    /// <param name="value"></param>
    private void SignArrWrite(int value)
    {
        creatSign[0]++;
        creatSign[creatSign[0]] = value;
        if (creatSign[0] == 2)
        {
            creatSign[0]++;
        }
    }

    /// <summary>
    /// 判断某个数组中是否包含某个值
    /// </summary>
    /// <param name="arr">目标数组</param>
    /// <param name="val">目标值</param>
    /// <returns></returns>
    public bool ArrContain(int[] arr,int val)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] == val)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 截取一个数组的某一段
    /// </summary>
    /// <param name="arr">目标数组</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="endIndex">终点索引</param>
    /// <returns></returns>
    public int[] ArrCut(int[] arr,int startIndex,int endIndex)
    {
        
        if (startIndex < 0)
        {
            startIndex = 0;
        }
        if (endIndex > arr.Length)
        {
            endIndex = arr.Length;
        }
        int[] resArr = new int[endIndex - startIndex + 1];
        for(int i = 0; i < resArr.Length; i++)
        {
            resArr[i] = arr[startIndex];
            startIndex++;
        }

        return resArr;
    }

    /// <summary>
    /// 重置位置标记数组
    /// </summary>
    private void SignArrReset()
    {
        for (int i = 0; i < creatSign.Length; i++)
        {
            creatSign[i] = 0;
        }
        if (existMap.Count < 2)
        {
            newMapCreated = false;
        }
        else
        {
            newMapCreated = true;
        }

        allowNewMap = true;
    }

    /// <summary>
    /// 获取当前屏幕范围
    /// </summary>
    public void screenBoundGet()
    {
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(-Camera.main.transform.position.z)));
        float left = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        float right = cornerPos.x;
        float top = cornerPos.y;
        float bottom = Camera.main.transform.position.y * 2 - cornerPos.y;
        screenXBound = new Vector2(left, right);
        screenYBound = new Vector2(bottom, top);
    }

    /// <summary>
    /// 创建星图，并将星图拼接起来
    /// </summary>
    /// <param name="count">需要几个星图</param>
    /// <param name="pos">第一个星图放置的位置标记</param>
    private void MapCreate(int count,int pos)
    {
        MapBlock[] newMap = new MapBlock[count];
        for(int i = 0; i < count; i++)
        {

            MapBlock mb = objPool.GetObj(ConstGameData.blockMap);
            newMap[i] = mb;
            existMap.Add(mb);
        }
        nowInside.MapConnect(newMap, pos);
        newMapCreated = true;
    }




    /// <summary>
    /// 计算范围内每个引力的方向和大小并放入一个数组中
    /// </summary>
    /// <param name="AllStars">所有引力范围内的星</param>
    /// <returns></returns>
    private Vector3[] AllForceGet(List<Star> AllStars)
    {
        Vector3[] allGravit = new Vector3[AllStars.Count];
        for(int i = 0;i< AllStars.Count; i++)
        {
            allGravit[i] = AllStars[i].force;
        }

        return allGravit;
    }

    /// <summary>
    /// 计算出一组向量相加的值（包含了引力合力的大小和方向）
    /// </summary>
    /// <param name="allGravit"></param>
    /// <returns></returns>
    private Vector3 ResultForceGet(Vector3[] allGravit)
    {
        resultantGravit = Vector3.zero;
        foreach (Vector3 v3 in allGravit)
        {
            resultantGravit = resultantGravit + v3;
        }
        return resultantGravit;
    }



 /*   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            GameObject obj = collision.gameObject;
            starInRange.Add(obj);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision)
        {
            GameObject obj = collision.gameObject;
            starInRange.Remove(obj);
        }
        
    }*/


}
