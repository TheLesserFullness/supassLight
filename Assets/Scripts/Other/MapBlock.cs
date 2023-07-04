using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MapBlock : MonoBehaviour
{
    /// <summary>
    /// 期望的Star之间的平均间距
    /// </summary>
    int starDis;
    /// <summary>
    /// 实际的根据星图配置计算出的平均间距
    /// </summary>
    float realStarDis;
    /// <summary>
    /// 此星图块是否是背景块
    /// </summary>
    bool isBG = false;
    /// <summary>
    /// 星图数组的长宽
    /// </summary>
    int arrW, arrH;
    /// <summary>
    /// 浮动范围为【-floatRange*starDis，floatRange*starDis】,表示浮动floatRange倍的平均间距
    /// </summary>
    float floatRange;


    /// <summary>
    /// 此星图是否已在视野外
    /// </summary>
    public bool OutOfView;
    /// <summary>
    /// 是否可以回收此图
    /// </summary>
    public bool allowDisroy;
    /// <summary>
    /// 屏幕范围是否已经经过它了
    /// </summary>
    public bool viewAccrossed;
    /// <summary>
    /// 初始化产生的星组
    /// </summary>
    public Star[][] starMap;
    /// <summary>
    /// 此地图块中的Star数量
    /// </summary>
    public int starAmount;
    /// <summary>
    /// 地图块放置的位置
    /// </summary>
    public Vector3 blockPos;
    /// <summary>
    /// 星图的边界
    /// </summary>
    public Vector2 xBound, yBound;
    /// <summary>
    /// map回收范围
    /// </summary>
    public Vector2 xRecycle, yRecycle;
    /// <summary>
    /// 星图宽度，单位为“单位长度”，在unity中实际值需要再乘 unit 即可
    /// </summary>
    public float width;
    public float height;
    /// <summary>
    /// //每一个单位长度实际值，星图长宽乘此值即为星图在unity中的实际长度
    /// </summary>
    public float unit;
    /// <summary>
    /// 回收范围相比于星图实际范围向外延申的值，默认取5%
    /// </summary>
    public float judgeFloat = 0.05f;
    /// <summary>
    /// 拼接偏移,用于避免交界处大量星体重合
    /// </summary>
    public float connectOffset;
    /// <summary>
    /// 创建新的星图的范围，默认为0
    /// </summary>
    public float createFloat = 0;
    /// <summary>
    /// 星图顶点数组，从0~3依次是原点顺时针走，
    /// </summary>
    public Vector3[] mapVertex;
    /// <summary>
    /// 星图拼接的位置数据
    /// </summary>
    public Vector3[] connectPoint;
    /// <summary>
    /// 四种恒星等级的数量比例，xyzw对应一二三四级
    /// </summary>
    public Vector4 countRatio;
    /// <summary>
    /// 每种等级的星的初始scale，xyzw对应一二三四级
    /// </summary>
    public Vector4 starScale;
    /// <summary>
    /// 是否显示判断范围
    /// </summary>
    public bool showJudgeRect = true;
    /// <summary>
    /// 默认材质
    /// </summary>
    public Material bgStarMaterial;
    /// <summary>
    /// 存储所有三级和四级星在星图中的索引
    /// </summary>
    public List<Star> level3_4;
    /// <summary>
    /// 存储星图中被吞噬掉的星
    /// </summary>
    public List<Star> diedStars;
    /// <summary>
    /// 如果此图是背景块，则创建的星放入此处（无Star组件，不产生引力）
    /// </summary>
    public GameObject[][] starMapBG;


    /// <summary>
    /// 是否生成背景，如果已经生成，则表示是否显示背景
    /// </summary>
    public bool getBG = false;
    /// <summary>
    /// 星图的单个背景块的大小参数，即背景块的长宽为星图大小的1/singleBGsize
    /// </summary>
    public int singleBGsize;
    /// <summary>
    /// 单个背景块中星的数量
    /// </summary>
    public int bgStarCount;
    /// <summary>
    /// 背景块在Z轴上的偏移距离
    /// </summary>
    public float zOffset;
    /// <summary>
    /// 当前星图的所有背景块
    /// </summary>
    public MapBlock[] blockBG;
    /// <summary>
    /// 所有背景块放置的点位
    /// </summary>
    public Vector3[] blockBGPos;
    /// <summary>
    /// 背景块的差异数据，（星数量浮动上下限）
    /// </summary>
    public Vector2 blockDif;


    Vector3[] spos,epos;
    

    public MapBlock(int mapHeight, int w_hRatio, int unitValue, int starAvgDis, int floatRangeLimit)
    {
        starDis = starAvgDis;
        floatRange = floatRangeLimit / 10;
        height = mapHeight;
        width = mapHeight * w_hRatio;
        unit = unitValue;
        starAmount = (int)width * (int)width * w_hRatio / (starDis * starDis);
        arrW = Mathf.FloorToInt(width / starDis);
        arrH = Mathf.FloorToInt(height / starDis);
        starMap = new Star[arrW][];
        for (int i = 0; i < arrW; i++)
        {
            starMap[i] = new Star[arrH];
        }
    }

    private void Start()
    {
/*        singleBGsize = 3;
        bgStarCount = 450;
        zOffset = -5;
        level3_4 = new List<Star>();*/
        MapBoundGet();
        if (!isBG)
        {
            //GetOrigPos();
        }
        showJudgeRect = true;
    }
    private void Update()
    {

        if (!isBG)
        {
            MapBoundGet();
        }

        /*if (blockBG == null || blockBG.Length == 0)
        {
            if (getBG)
                BGBlockCreate(singleBGsize, bgStarCount);
        }
        else
        {
            if (blockBG.Length > 0 && blockBG[0] != null)
            {
                if (getBG)
                {
                    if (blockBG[0].gameObject.activeInHierarchy == false)
                    {
                        BGStateCg(getBG);
                    }
                }
                else
                {
                    if (blockBG[0].gameObject.activeInHierarchy == true)
                    {
                        BGStateCg(getBG);
                    }
                }
            }
        }*/
    }

    public void MapBlockCrate()
    {
        MapBlockCrate(190, 1, 1, 3, 5, 0.05f);
    }

    /// <summary>
    /// 常规星图生成，带默认材质
    /// </summary>
    /// <param name="mapHeight">星图高度</param>
    /// <param name="w_hRatio">星图宽高比</param>
    /// <param name="unitValue">星图单位长度</param>
    /// <param name="starAvgDis">星图中平均星间距</param>
    /// <param name="floatRangeLimit">位置浮动范围</param>
    /// <param name="material">默认材料</param>
    public void MapBlockCrate(int mapHeight, int w_hRatio, float unitValue, int starAvgDis, int floatRangeLimit,Material material)
    {
        singleBGsize = 2;
        bgStarCount = 500;
        zOffset = -5;
        level3_4 = new List<Star>();
        starDis = starAvgDis;
        floatRange = floatRangeLimit / 10;
        height = mapHeight * unitValue;
        width = mapHeight * w_hRatio * unitValue;
        unit = unitValue;
        countRatio = new Vector4(45, 35, 15, 5);
        starScale = new Vector4(0.6f, 1f, 1.5f, 2f);
        connectOffset = 0.014f;
        bgStarMaterial = material;
        starAmount = mapHeight * mapHeight * w_hRatio / (starDis * starDis);
        arrW = Mathf.FloorToInt(mapHeight * w_hRatio / starDis);
        arrH = Mathf.FloorToInt(mapHeight / starDis);
        realStarDis = (float)width / (float)(arrH - 1);
        starMap = new Star[arrW][];
        for (int i = 0; i < arrW; i++)
        {
            starMap[i] = new Star[arrH];
        }
        starCreate();
        mapVertex = new Vector3[4];
        connectPoint = new Vector3[8];
        judgeFloat = 0.05f;
        showJudgeRect = true;
        MapBoundGet();
        BGBlockCreate(singleBGsize, bgStarCount);
    }
    /// <summary>
    /// 常规星图生成，带回收范围浮动
    /// </summary>
    /// <param name="mapHeight">星图高度</param>
    /// <param name="w_hRatio">星图宽高比</param>
    /// <param name="unitValue">星图单位长度，与星图宽高相乘即为星图在unity中实际值</param>
    /// <param name="starAvgDis">平均星间距</param>
    /// <param name="floatRangeLimit">位置浮动范围</param>
    /// <param name="judgeRate">回收范围浮动</param>
    public void MapBlockCrate(int mapHeight, int w_hRatio, float unitValue, int starAvgDis, int floatRangeLimit,float judgeRate)
    {
        singleBGsize = 2;
        bgStarCount = 500;
        zOffset = -5;
        level3_4 = new List<Star>();
        starDis = starAvgDis;
        floatRange = floatRangeLimit / 10;
        height = mapHeight * unitValue;
        width = mapHeight * w_hRatio * unitValue;
        unit = unitValue;
        countRatio = new Vector4(45, 35, 15, 5);
        starScale = new Vector4(0.6f, 1f, 1.5f, 2f);
        connectOffset = 0.014f;
        starAmount = mapHeight * mapHeight * w_hRatio / (starDis * starDis);
        arrW = Mathf.FloorToInt(mapHeight * w_hRatio / starDis);
        arrH = Mathf.FloorToInt(mapHeight / starDis);
        realStarDis = (float)width / (float)(arrH - 1);

        starMap = new Star[arrW][];
        for (int i = 0; i < arrW; i++)
        {
            starMap[i] = new Star[arrH];
        }
        starCreate();
        mapVertex = new Vector3[4];
        connectPoint = new Vector3[8];
        MapBoundGet();
        judgeFloat = Mathf.Abs(judgeRate);
        showJudgeRect = true;
        BGBlockCreate(singleBGsize, bgStarCount);

    }
    /// <summary>
    /// 星图生成，但是背景块
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <param name="starAmount"></param>
    /// <param name="bgSize"></param>
    public void MapBlockCrate(float bgHeight, float bgWidth, int arrLength,MapBlock fatherMap,MapBlock self)
    {
        zOffset = -5f;
        self.width = bgWidth;
        self.height = bgHeight;
        self.starAmount = arrLength * arrLength;
        self.realStarDis = height / (arrLength - 1);
        self.isBG = true;
        self.starMapBG = new GameObject[arrLength][];
        for (int i = 0; i < arrLength; i++)
        {
            self.starMapBG[i] = new GameObject[arrLength];
        }
        BGCreate(self);
        self.mapVertex = new Vector3[4];
        self.connectPoint = new Vector3[8];
    }
    /// <summary>
    /// 创建背景块
    /// </summary>
    /// <param name="bgSize"></param>
    /// <param name="starAmount"></param>
    public void BGBlockCreate(int bgSize,int starAmount)
    {
        blockBG = new MapBlock[bgSize * bgSize];
        //Debug.Log(blockBG.Length + "背景数组=======");
        float bgHeight = height / bgSize;
        float bgWidth = width / bgSize;
        int arrLength;
        GetBGPos(bgWidth, bgHeight);
        GameObject bgObj;
        MapBlock mapBG;
        for(int i = 0; i < blockBG.Length; i++)
        {
            //starAmount = (int)(starAmount * (1 + MyTool.randomInt(-10, -5, 5, 10) / 100f));
            arrLength = (int)Mathf.Sqrt(starAmount * (1 + MyTool.randomInt(-10, -5, 5, 10) / 100f));
            bgObj = new GameObject("bgBlock" + i);
            bgObj.transform.SetParent(this.gameObject.transform);
            bgObj.gameObject.transform.position = blockBGPos[i];
            mapBG = bgObj.AddComponent<MapBlock>();
            mapBG.MapBlockCrate(bgHeight, bgWidth, arrLength,this,mapBG);
            blockBG[i] = mapBG;
        }
        
    }

    public void GetBGPos(float xOffset,float yOffset)
    {
        blockBGPos = new Vector3[singleBGsize * singleBGsize];
        for (int i = 0; i < singleBGsize; i++)
        {
            for (int j = 0; j < singleBGsize; j++)
            {
                blockBGPos[i * singleBGsize + j] = new Vector3(i * xOffset, j * yOffset, -zOffset);
            }
        }
    }

    public void BGStateCg(bool state)
    {
        foreach(MapBlock bgMap in blockBG)
        {
            bgMap.gameObject.SetActive(state);
        }
    }


    /// <summary>
    /// 星图生成
    /// </summary>
    /// <param name="mapHeight">星图高度</param>
    /// <param name="w_hRatio">星图宽高比</param>
    /// <param name="unitValue">星图单位长度，与星图宽高相乘即为星图在unity中实际值</param>
    /// <param name="starAvgDis">平均星间距</param>
    /// <param name="floatRangeLimit">位置浮动范围</param>
    /// <param name="judgeRate">回收范围浮动</param>
    public void Remake(int mapHeight, int w_hRatio, int unitValue, int starAvgDis, int floatRangeLimit, float judgeRate)
    {
        DestroyAll();
        //DestoryBG();
        MapBlockCrate(mapHeight, w_hRatio, unitValue, starAvgDis, floatRangeLimit, judgeRate);
    }

    /// <summary>
    /// 原地刷新初始星组的位置，主要用于星图的回收利用
    /// </summary>
    public void RefreshStarPos()
    {
        level3_4 = null;
        level3_4 = new List<Star>();

        foreach (Star s in diedStars)
        {
            s.Born();
        }

        for(int i = 0; i < starMap.Length; i++)
        {
            for(int j = 0; j < starMap[i].Length; j++)
            {
                starMap[i][j].gameObject.transform.localPosition = GetPosFromIndex(i, j);
                Processing_1(starMap[i][j]);
            }
        }
        Processing_2();
        Processing_3();
    }

    /// <summary>
    /// 销毁当前星图中的所有星
    /// </summary>
    public void DestroyAll()
    {

        for (int i = 0; i < starMap.Length; i++)
        {
            for (int j = 0; j < starMap[i].Length; j++)
            {
                if (starMap[i][j])
                {
                    DestroyImmediate(starMap[i][j].gameObject);
                }
            }
        }

    }

    public void DestoryBG()
    {
        for(int i = 0; i < blockBG.Length; i++)
        {
            if (blockBG[i])
            {
                // DestroyImmediate(blockBG[i].gameObject);
                blockBG[i].DestroyAll();
            }
        }
            
        
    }




    /// <summary>
    /// 返回向外扩张一定值的顶点数组
    /// </summary>
    /// <param name="dis">每条边平移的距离</param>
    /// <returns></returns>
    Vector3[] MarginCg(float dis)
    {
        Vector3[] newVertex = new Vector3[4];
        Vector3 xDif = new Vector3(dis, 0, 0);
        Vector3 yDif = new Vector3(0, dis, 0);
        newVertex[0] = mapVertex[0] + xDif + yDif;
        newVertex[1] = mapVertex[1] + xDif - yDif;
        newVertex[2] = mapVertex[2] - xDif - yDif;
        newVertex[3] = mapVertex[3] - xDif + yDif;

        return newVertex;
    }

    /// <summary>
    /// 调试用于反映星的标准位置
    /// </summary>
    public void GetOrigPos()
    {
        
        Vector3[] startPos = new Vector3[2 * starMap.Length];
        Vector3[] endtPos = new Vector3[2 * starMap.Length];
        for (int i = 0; i < starMap.Length; i++)
        {
            startPos[i] = new Vector3(i * unit * realStarDis,  0, 0);
            startPos[i + starMap.Length] = new Vector3(0, i * unit * realStarDis, 0);


            endtPos[i] = new Vector3(i * unit * realStarDis, starMap.Length * unit * realStarDis, 0);
            endtPos[i + starMap.Length] = new Vector3(starMap.Length * unit * realStarDis, i * unit * realStarDis, 0);
        }
        spos = startPos;
        epos = endtPos;
    }
    /// <summary>
    /// 调试用于反映星的标准位置
    /// </summary>
    public void DrawOrigPos()
    {
        for(int i = 0; i < spos.Length; i++)
        {
            Debug.DrawLine(spos[i], epos[i], Color.white);
        }
    }


    /// <summary>
    /// 获取星图的顶点数组、边界范围、回收范围、星图拼接点数组
    /// </summary>
    private void MapBoundGet()
    {
        Vector3 orgPos = this.gameObject.transform.position;//地图原点位置
        Vector3 widthOut = new Vector3(width, 0, 0);
        Vector3 heightOut = new Vector3(0, height, 0);
        float offsetRatio = 1 + connectOffset;
        offsetRatio = 1;
        mapVertex[0] = orgPos;
        mapVertex[1] = orgPos + heightOut;
        mapVertex[2] = orgPos + heightOut + widthOut;
        mapVertex[3] = orgPos + widthOut;

        xBound = new Vector2(orgPos.x, orgPos.x + width);
        yBound = new Vector2(orgPos.y, orgPos.y + height);

        xRecycle = new Vector2(-judgeFloat * width, judgeFloat * width) + xBound;
        yRecycle = new Vector2(-judgeFloat * height, judgeFloat * height) + yBound;

        if (showJudgeRect)
        {
            MyTool.DrawSides(MarginCg(-width * createFloat), Color.green);
            MyTool.DrawSides(MarginCg(-width * judgeFloat), Color.red);
            //DrawOrigPos();
        }

        connectPoint[0] = orgPos + new Vector3(-width * offsetRatio, 0, 0);       //正左
        connectPoint[1] = orgPos + new Vector3(-width * offsetRatio, height * offsetRatio, 0);  //左上
        connectPoint[2] = orgPos + new Vector3(0, height * offsetRatio, 0);       //正上
        connectPoint[3] = orgPos + new Vector3(width * offsetRatio, height * offsetRatio, 0);   //右上
        connectPoint[4] = orgPos + new Vector3(width * offsetRatio, 0, 0);        //正右
        connectPoint[5] = orgPos + new Vector3(width * offsetRatio, -height * offsetRatio, 0);  //右下
        connectPoint[6] = orgPos + new Vector3(0, height * offsetRatio, 0);       //正下
        connectPoint[7] = orgPos + new Vector3(-width * offsetRatio, -height * offsetRatio, 0); //左下

    }

    /// <summary>
    /// 将多个星图拼接到当前星图
    /// </summary>
    /// <param name="newMap">需要拼接到此图的所有星图（最多三个）</param>
    /// <param name="posNum">数组中第一个图在此图的方位表征</param>
    public void MapConnect(MapBlock[] newMap,int posNum)
    {
        
        for(int i = 0;i<newMap.Length;i++)
        {
            newMap[i].Place(connectPoint[posNum % 8]);
            //newMap[i].gameObject.SetActive(true);
            posNum++;
        }
    }
    public void Place(Vector3 position)
    {
        this.gameObject.transform.position = position;
        MapBoundGet();
        blockPos = position;
    }

    /// <summary>
    /// 通过星星在数组中的位置返回随机生成的坐标
    /// </summary>
    /// <param name="x">数组横坐标</param>
    /// <param name="y">数组纵坐标</param>
    public Vector3 GetPosFromIndex(int x,int y)
    {
        
        float xOffset = (float)MyTool.randomInt(-500,500)/1000f;
        float yOffset = (float)MyTool.randomInt1(-500,500)/1000f;
        Vector3 pos = new Vector3((x + xOffset) * unit * realStarDis, (y + yOffset) * unit * realStarDis, 0);
        if (x == 0)
        {
            pos = pos + new Vector3(0.3f * unit * realStarDis, 0, 0);
        }
        if(y == 0)
        {
            pos = pos + new Vector3(0, 0.3f * unit * realStarDis, 0);
        }
        if (x == starMap.Length - 1)
        {
            pos = pos + new Vector3( -0.3f * unit * realStarDis, 0, 0);
        }
        if(y == starMap[0].Length - 1)
        {
            pos = pos + new Vector3(0,  -0.3f * unit * realStarDis, 0);
        }


        return pos;
    }
    /// <summary>
    /// 通过数组坐标等限制信息返回随机位置
    /// </summary>
    /// <param name="x">星星在数组中的索引</param>
    /// <param name="y">星星在数组中的索引</param>
    /// <param name="realDis">任意两星的实际平均距离</param>
    /// <param name="arrXLength">数组长度</param>
    /// <param name="arrYLength">数组长度</param>
    /// <returns></returns>
    public Vector3 GetPosFromIndex(int x, int y,float realDis,int arrXLength,int arrYLength)
    {

        float xOffset = (float)MyTool.randomInt(-500, 500) / 1000f;
        float yOffset = (float)MyTool.randomInt1(-500, 500) / 1000f;
        Vector3 pos = new Vector3((x + xOffset) * realDis, (y + yOffset) * realDis, 0);
        if (x == 0)
        {
            pos = pos + new Vector3(0.3f * realDis, 0, 0);
        }
        if (y == 0)
        {
            pos = pos + new Vector3(0, 0.3f * realDis, 0);
        }
        if (x == arrXLength - 1)
        {
            pos = pos + new Vector3(-0.3f * realDis, 0, 0);
        }
        if (y == arrYLength - 1)
        {
            pos = pos + new Vector3(0, -0.3f * realDis, 0);
        }


        return pos;
    }

    /// <summary>
    /// 根据当前星图数据生成星星
    /// </summary>
    void starCreate()
    {
        diedStars = new List<Star>();
        level3_4 = new List<Star>();
        for (int i = 0; i < starMap.Length; i++)
        {

            for (int j = 0; j < starMap[i].Length; j++)
            {
                GameObject gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gb.name = "star" + i + "_" + j;
                gb.transform.SetParent(this.gameObject.transform);
                gb.transform.localPosition = GetPosFromIndex(i, j);
                Star star = gb.AddComponent<Star>();
                star.xIndex = i;
                star.yIndex = j;
                star.belongTo = this;
                Processing_1(star);
                starMap[i][j] = star;
            }
        }
        Processing_2();
        Processing_3();
    }

    /// <summary>
    /// 根据当前数据生成背景
    /// </summary>
    void BGCreate(MapBlock blockSelf)
    {
        for (int i = 0; i < blockSelf.starMapBG.Length; i++)
        {

            for (int j = 0; j < blockSelf.starMapBG[i].Length; j++)
            {
                GameObject gb;
                if (TriggerAndCacu.sigleton.bgStar)
                {
                    gb = Instantiate(TriggerAndCacu.sigleton.bgStar);
                }
                else
                {
                    gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                }
                gb.name = "starMini" + i + "_" + j;
                gb.transform.SetParent(blockSelf.gameObject.transform);
                gb.transform.localPosition = GetPosFromIndex(i, j, blockSelf.realStarDis, blockSelf.starMapBG.Length, blockSelf.starMapBG[i].Length);
                gb.transform.localScale *= MyTool.randomInt(5,20)/100f;
                if (TriggerAndCacu.sigleton.bgStarMaterial)
                {
                    Renderer rend = gb.GetComponent<Renderer>();
                    rend.material = TriggerAndCacu.sigleton.bgStarMaterial;
                }
                blockSelf.starMapBG[i][j] = gb;
            }
        }
        Processing_2(blockSelf.starMapBG,blockSelf.realStarDis);
    }

    /// <summary>
    /// 对初始生成的星图进行一级处理，根据比例确定等级更高的星
    /// </summary>
    /// <param name="star"></param>
    public void Processing_1(Star star)
    {

        Vector4 nomal = MyTool.GetRatio(countRatio);
        int random;
        float scaleRandom;

        random = MyTool.randomInt(0, 100);
        scaleRandom = (float)MyTool.randomInt1(10, 20) / 100f;
        float symbol = MyTool.randomInt(-1, 1);
        if (symbol == 0)
            symbol = 0.9f;
        if (random >= 100 * (1 - nomal.w))
        {
            //四级星，基础scale为2
            star.gameObject.transform.localScale = Vector3.one * starScale.w * (1 + scaleRandom * symbol);
            star.starLevel = 4;
            star.eatGain = 0.05f;
            level3_4.Add(star);
            star.GetRadious();
        }
        else if (random >= 100 * (1 - nomal.z - nomal.w))
        {
            //三级星，基础scale为1.4
            star.gameObject.transform.localScale = Vector3.one * starScale.z * (1 + scaleRandom * symbol);
            star.starLevel = 3;
            star.eatGain = 0.1f;
            level3_4.Add(star);
            star.GetRadious();
        }
        else if (random >= 100 * (1 - nomal.y - nomal.z - nomal.w))
        {
            //二级星，基础scale为1
            star.gameObject.transform.localScale = Vector3.one * starScale.y * (1 + scaleRandom * symbol);
            star.starLevel = 2;
            star.eatGain = 0.15f;
            star.GetRadious();
        }
        else
        {
            //一级星，基础scale为0.6
            star.gameObject.transform.localScale = Vector3.one * starScale.x * (1 + scaleRandom * symbol);
            star.starLevel = 1;
            star.eatGain = 0.2f;
            star.GetRadious();
        }
    }

    /// <summary>
    /// 常规星图的二阶处理
    /// </summary>
    public void Processing_2()
    {
        int rdNum;
        int rdNum1;
        int rdNum2;
        for (int x = 0;x< starMap.Length;x++)
        {
            for (int y = 0;y<starMap[x].Length;y++)
            {
                rdNum = MyTool.randomInt(0, 1);
                if(rdNum == 0)
                {
                    rdNum1 = MyTool.randomInt(2, 12);
                    if (rdNum1 >= 5 && x < starMap.Length - 1)
                    {
                        starMap[x][y].gameObject.transform.position += new Vector3((float)MyTool.randomInt(50, 100) / 100f * unit * realStarDis, 0, 0);
                        starMap[x][y].gameObject.transform.localScale *= (1 + MyTool.randomInt(10, 15) / 100f);
                    }
                }
                else
                {
                    rdNum2 = MyTool.randomInt(-10, 0);
                    if(rdNum2 <= -4 && y > 0)
                    {
                        starMap[x][y].gameObject.transform.position -= new Vector3(0, (float)MyTool.randomInt(50, 80) / 100f * unit * realStarDis, 0);
                        starMap[x][y].gameObject.transform.localScale *= (1 - MyTool.randomInt(10, 15) / 100f);
                    }
                }
                
            }
        }

        foreach (Star[] item in starMap)
        {
            foreach (Star star in item)
            {
                rdNum1 = MyTool.randomInt(-10, 0);
                if (rdNum1 <= -4 && star.yIndex != 0)
                {
                    star.gameObject.transform.position -= new Vector3(0, (float)MyTool.randomInt(20, 30) / 100f * unit * realStarDis, 0);
                }
                star.GetMaterial();
                //star.MergeCheck();
            }
        }

        foreach(Star s in level3_4)
        {
            if (s.gameObject.activeInHierarchy)
            {
                s.StarEatCheck();
            }
        }


    }
    /// <summary>
    /// 常规星图的三阶处理
    /// </summary>
    public void Processing_3()
    {
        int newIndex;
        int random;
        foreach(Star s in level3_4)
        {
            random = MyTool.randomInt(0, 3);
            if (random >= 2)
            {
                newIndex = MyTool.NumLoop(s.yIndex, starMap.Length - 1, (float)MyTool.randomInt(-80, -45, 45, 80) / 100f);
                s.PosCg(starMap[s.xIndex][newIndex]);
            }
        }

    }


    /// <summary>
    /// 对背景层的2阶处理
    /// </summary>
    /// <param name="objArr"></param>
    /// <param name="offset"></param>
    public void Processing_2(GameObject[][] objArr,float offset)
    {
        int rdNum;
        int rdNum1;
        int rdNum2;
        for (int x = 0; x < objArr.Length; x++)
        {
            for (int y = 0; y < objArr[x].Length; y++)
            {
                rdNum = MyTool.randomInt(0, 1);
                if (rdNum == 0)
                {
                    rdNum1 = MyTool.randomInt(2, 12);
                    if (rdNum1 >= 5 && x < objArr.Length - 1)
                    {
                        objArr[x][y].gameObject.transform.position += new Vector3((float)MyTool.randomInt(50, 100) / 100f * offset, 0, 0);
                    }
                }
                else
                {
                    rdNum2 = MyTool.randomInt(-10, 0);
                    if (rdNum2 <= -4 && y > 0)
                    {
                        objArr[x][y].gameObject.transform.position -= new Vector3(0, (float)MyTool.randomInt(50, 80) / 100f * offset, 0);
                    }
                }

            }
        }
        
    }






}
