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
    /// ������������ϵ��
    /// </summary>
    public float gScale = 1;
    /// <summary>
    /// ʱ������ϵ��
    /// </summary>
    public float timeScale = 1;
    /// <summary>
    /// ����λ��
    /// </summary>
    public  Vector3 mainPos;
    /// <summary>
    /// ����Ԥ����
    /// </summary>
    public GameObject mainObj;
    /// <summary>
    /// ����
    /// </summary>
    public GameObject _mainObj;
    /// <summary>
    /// �ܶ����ǲ���������������
    /// </summary>
    public float forceMaxDis = 20;
    /// <summary>
    /// ��������
    /// </summary>
    public List<Star> starInRange;
    /// <summary>
    /// �����ǵ�Ԥ����
    /// </summary>
    public GameObject bgStar;
    /// <summary>
    /// ��������Ĳ���
    /// </summary>
    public Material[] materials;
    /// <summary>
    /// Ĭ�ϱ�������
    /// </summary>
    public Material bgStarMaterial;
    /// <summary>
    /// �����
    /// </summary>
    public ObjPool objPool;
    

    /// <summary>
    /// ���պ�������,��������ʹ�С
    /// </summary>
    Vector3 resultantGravit = Vector3.zero;
    /// <summary>
    /// ��ǰ���ڵ���ͼ����
    /// </summary>
    public List<MapBlock> existMap;

    /// <summary>
    /// ��ǰ��Ļ�ı߽緶Χ(x,y);x<y
    /// </summary>
    public Vector2 screenXBound,screenYBound;
    /// <summary>
    /// �Ƿ���Ҫ�µ�ͼ
    /// </summary>
    public bool needNewMap = false;
    /// <summary>
    /// ��ǰ�Ƿ����������µ���ͼ
    /// </summary>
    public bool allowNewMap = false;
    /// <summary>
    /// �µ���ͼ�Ƿ��Ѿ��������ˣ�����Ѿ��������ˣ���ô�����������µ���ͼ������ֱ����ͼ�����л�ʱˢ��
    /// </summary>
    public bool newMapCreated = false;
    /// <summary>
    /// ��һλ��ʾ��ǰ������ͼ����������0��
    /// </summary>
    public int[] creatSign;
    /// <summary>
    /// ��ͼ����������Ұ������ͼ��ʱ������ͼ��������
    /// </summary>
    public MapBlock nowInside;

    public GameConfig gameConfig;

    /// <summary>
    /// �����ϲ��������ٶ�ֵ
    /// </summary>
    public float realSpeed;
    /// <summary>
    /// ��������Ϣ���ٶ�ֵ
    /// </summary>
    public Vector3 speed;
    /// <summary>
    /// ���������ĺ���
    /// </summary>
    public Vector3 mergeForce;
    /// <summary>
    /// ��������ϵ��ģ�����ļ��ٶ�
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
    /// ����������Դ
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
    /// �������û�ȡ��ʼֵ����������
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
                //Debug.Log("-----��ǰ����"+ mergeForce.ToString()+ "-----��ǰ���ٶȣ�"+acc.ToString());
                //Debug.Log(resultantGravit + "<---resultforce---------------------");
                //Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + resultantGravit, Color.red);
            }
            UIManager.singleton.GetUI(UIManager.PANEL_RealTimeData).Update();
        }
        


    }

    /// <summary>
    /// ������ͼ��nowInside���뿪��Ұ�����¼�����ͼ����ˢ��״̬�������µ���ͼ����ʱ���ֻ����������ͼͬʱ��������Ұ�С�
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
                
                //������ʱ��Ұ�н�����һ����ͼ������Ұ��������ͼ�Ľ��紦
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
    /// ��һ����ͼ�ı�ʶ���ݽ����޸ģ����Ƿ���Ի��գ��Ƿ�����Ļ�ڣ��Ƿ񴩹�����Ұ��
    /// </summary>
    /// <param name="map">�����ͼ</param>
    private bool StarMapCheck(MapBlock map)
    {
        Vector3 mapPos = map.gameObject.transform.position;
        Vector3 cameraPos = Camera.main.transform.position;
        //���������Ļ�Ͻ�С����ͼ�½������Ļ�½������ͼ�Ͻ磬�������ͼ������Ļ��
        //�������ͼ�Ѿ�����Ұ�����ˣ����ͼ�ǿ������ٵ���
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
    /// ����Ұ���ﵱǰ��ͼ�Ĵ�����Χʱ���ж���Ҫ���ɼ�����ͼ�����ڶ�Ӧ��λ���Ϸ�����ͼ
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
    /// �жϵ�ǰ�Ƿ���Ҫ������ͼ����д��λ�ñ��
    /// </summary>
    public void IsNearMapBound()
    {
        //����Ҫ������ͼʱ��xDif��-��+����yDif��-��+��������ͼ�Ͻ������Ļ�Ͻ磬��ͼ�½�С����Ļ�½�
        Vector2 xDif = nowInside.xBound - screenXBound;
        Vector2 yDif = nowInside.yBound - screenYBound;
        if (yDif.x >= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 7))
        {
            //���ﵱǰ��ͼ�������ߣ��ڵ�ǰ��ͼ�Ϸ������ͼ
            //Debug.Log("--------��");
            SignArrWrite(7);
        }
        if (yDif.y <= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 3))
        {
            //���ﵱǰ��ͼ�ײ����ߣ��ڵ�ǰ��ͼ�·������ͼ
            //Debug.Log("--------��");
            SignArrWrite(3);
        }
        if (xDif.x >= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 1))
        {
            //���ﵱǰ��ͼ�����ߣ��ڵ�ǰ��ͼ�󷽷������ͼ
            //Debug.Log("--------��");
            SignArrWrite(1);
        }
        if (xDif.y <= 0 && !ArrContain(ArrCut(creatSign, 1, 3), 5))
        {
            //���ﵱǰ��ͼ�Ҳ���ߣ��ڵ�ǰ��ͼ�ҷ������ͼ
            //Debug.Log("--------��");
            SignArrWrite(5);
        }

    }

    /// <summary>
    /// ����Ұ�����ͼ���л���
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
    /// д�봴����ǣ�������ǰ���д�����ͼ�Ľṹ
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
    /// �ж�ĳ���������Ƿ����ĳ��ֵ
    /// </summary>
    /// <param name="arr">Ŀ������</param>
    /// <param name="val">Ŀ��ֵ</param>
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
    /// ��ȡһ�������ĳһ��
    /// </summary>
    /// <param name="arr">Ŀ������</param>
    /// <param name="startIndex">��ʼ����</param>
    /// <param name="endIndex">�յ�����</param>
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
    /// ����λ�ñ������
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
    /// ��ȡ��ǰ��Ļ��Χ
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
    /// ������ͼ��������ͼƴ������
    /// </summary>
    /// <param name="count">��Ҫ������ͼ</param>
    /// <param name="pos">��һ����ͼ���õ�λ�ñ��</param>
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
    /// ���㷶Χ��ÿ�������ķ���ʹ�С������һ��������
    /// </summary>
    /// <param name="AllStars">����������Χ�ڵ���</param>
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
    /// �����һ��������ӵ�ֵ�����������������Ĵ�С�ͷ���
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
