using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MapBlock : MonoBehaviour
{
    /// <summary>
    /// ������Star֮���ƽ�����
    /// </summary>
    int starDis;
    /// <summary>
    /// ʵ�ʵĸ�����ͼ���ü������ƽ�����
    /// </summary>
    float realStarDis;
    /// <summary>
    /// ����ͼ���Ƿ��Ǳ�����
    /// </summary>
    bool isBG = false;
    /// <summary>
    /// ��ͼ����ĳ���
    /// </summary>
    int arrW, arrH;
    /// <summary>
    /// ������ΧΪ��-floatRange*starDis��floatRange*starDis��,��ʾ����floatRange����ƽ�����
    /// </summary>
    float floatRange;


    /// <summary>
    /// ����ͼ�Ƿ�������Ұ��
    /// </summary>
    public bool OutOfView;
    /// <summary>
    /// �Ƿ���Ի��մ�ͼ
    /// </summary>
    public bool allowDisroy;
    /// <summary>
    /// ��Ļ��Χ�Ƿ��Ѿ���������
    /// </summary>
    public bool viewAccrossed;
    /// <summary>
    /// ��ʼ������������
    /// </summary>
    public Star[][] starMap;
    /// <summary>
    /// �˵�ͼ���е�Star����
    /// </summary>
    public int starAmount;
    /// <summary>
    /// ��ͼ����õ�λ��
    /// </summary>
    public Vector3 blockPos;
    /// <summary>
    /// ��ͼ�ı߽�
    /// </summary>
    public Vector2 xBound, yBound;
    /// <summary>
    /// map���շ�Χ
    /// </summary>
    public Vector2 xRecycle, yRecycle;
    /// <summary>
    /// ��ͼ��ȣ���λΪ����λ���ȡ�����unity��ʵ��ֵ��Ҫ�ٳ� unit ����
    /// </summary>
    public float width;
    public float height;
    /// <summary>
    /// //ÿһ����λ����ʵ��ֵ����ͼ����˴�ֵ��Ϊ��ͼ��unity�е�ʵ�ʳ���
    /// </summary>
    public float unit;
    /// <summary>
    /// ���շ�Χ�������ͼʵ�ʷ�Χ���������ֵ��Ĭ��ȡ5%
    /// </summary>
    public float judgeFloat = 0.05f;
    /// <summary>
    /// ƴ��ƫ��,���ڱ��⽻�紦���������غ�
    /// </summary>
    public float connectOffset;
    /// <summary>
    /// �����µ���ͼ�ķ�Χ��Ĭ��Ϊ0
    /// </summary>
    public float createFloat = 0;
    /// <summary>
    /// ��ͼ�������飬��0~3������ԭ��˳ʱ���ߣ�
    /// </summary>
    public Vector3[] mapVertex;
    /// <summary>
    /// ��ͼƴ�ӵ�λ������
    /// </summary>
    public Vector3[] connectPoint;
    /// <summary>
    /// ���ֺ��ǵȼ�������������xyzw��Ӧһ�����ļ�
    /// </summary>
    public Vector4 countRatio;
    /// <summary>
    /// ÿ�ֵȼ����ǵĳ�ʼscale��xyzw��Ӧһ�����ļ�
    /// </summary>
    public Vector4 starScale;
    /// <summary>
    /// �Ƿ���ʾ�жϷ�Χ
    /// </summary>
    public bool showJudgeRect = true;
    /// <summary>
    /// Ĭ�ϲ���
    /// </summary>
    public Material bgStarMaterial;
    /// <summary>
    /// �洢�����������ļ�������ͼ�е�����
    /// </summary>
    public List<Star> level3_4;
    /// <summary>
    /// �洢��ͼ�б����ɵ�����
    /// </summary>
    public List<Star> diedStars;
    /// <summary>
    /// �����ͼ�Ǳ����飬�򴴽����Ƿ���˴�����Star�����������������
    /// </summary>
    public GameObject[][] starMapBG;


    /// <summary>
    /// �Ƿ����ɱ���������Ѿ����ɣ����ʾ�Ƿ���ʾ����
    /// </summary>
    public bool getBG = false;
    /// <summary>
    /// ��ͼ�ĵ���������Ĵ�С��������������ĳ���Ϊ��ͼ��С��1/singleBGsize
    /// </summary>
    public int singleBGsize;
    /// <summary>
    /// �������������ǵ�����
    /// </summary>
    public int bgStarCount;
    /// <summary>
    /// ��������Z���ϵ�ƫ�ƾ���
    /// </summary>
    public float zOffset;
    /// <summary>
    /// ��ǰ��ͼ�����б�����
    /// </summary>
    public MapBlock[] blockBG;
    /// <summary>
    /// ���б�������õĵ�λ
    /// </summary>
    public Vector3[] blockBGPos;
    /// <summary>
    /// ������Ĳ������ݣ������������������ޣ�
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
    /// ������ͼ���ɣ���Ĭ�ϲ���
    /// </summary>
    /// <param name="mapHeight">��ͼ�߶�</param>
    /// <param name="w_hRatio">��ͼ��߱�</param>
    /// <param name="unitValue">��ͼ��λ����</param>
    /// <param name="starAvgDis">��ͼ��ƽ���Ǽ��</param>
    /// <param name="floatRangeLimit">λ�ø�����Χ</param>
    /// <param name="material">Ĭ�ϲ���</param>
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
    /// ������ͼ���ɣ������շ�Χ����
    /// </summary>
    /// <param name="mapHeight">��ͼ�߶�</param>
    /// <param name="w_hRatio">��ͼ��߱�</param>
    /// <param name="unitValue">��ͼ��λ���ȣ�����ͼ�����˼�Ϊ��ͼ��unity��ʵ��ֵ</param>
    /// <param name="starAvgDis">ƽ���Ǽ��</param>
    /// <param name="floatRangeLimit">λ�ø�����Χ</param>
    /// <param name="judgeRate">���շ�Χ����</param>
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
    /// ��ͼ���ɣ����Ǳ�����
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
    /// ����������
    /// </summary>
    /// <param name="bgSize"></param>
    /// <param name="starAmount"></param>
    public void BGBlockCreate(int bgSize,int starAmount)
    {
        blockBG = new MapBlock[bgSize * bgSize];
        //Debug.Log(blockBG.Length + "��������=======");
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
    /// ��ͼ����
    /// </summary>
    /// <param name="mapHeight">��ͼ�߶�</param>
    /// <param name="w_hRatio">��ͼ��߱�</param>
    /// <param name="unitValue">��ͼ��λ���ȣ�����ͼ�����˼�Ϊ��ͼ��unity��ʵ��ֵ</param>
    /// <param name="starAvgDis">ƽ���Ǽ��</param>
    /// <param name="floatRangeLimit">λ�ø�����Χ</param>
    /// <param name="judgeRate">���շ�Χ����</param>
    public void Remake(int mapHeight, int w_hRatio, int unitValue, int starAvgDis, int floatRangeLimit, float judgeRate)
    {
        DestroyAll();
        //DestoryBG();
        MapBlockCrate(mapHeight, w_hRatio, unitValue, starAvgDis, floatRangeLimit, judgeRate);
    }

    /// <summary>
    /// ԭ��ˢ�³�ʼ�����λ�ã���Ҫ������ͼ�Ļ�������
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
    /// ���ٵ�ǰ��ͼ�е�������
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
    /// ������������һ��ֵ�Ķ�������
    /// </summary>
    /// <param name="dis">ÿ����ƽ�Ƶľ���</param>
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
    /// �������ڷ�ӳ�ǵı�׼λ��
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
    /// �������ڷ�ӳ�ǵı�׼λ��
    /// </summary>
    public void DrawOrigPos()
    {
        for(int i = 0; i < spos.Length; i++)
        {
            Debug.DrawLine(spos[i], epos[i], Color.white);
        }
    }


    /// <summary>
    /// ��ȡ��ͼ�Ķ������顢�߽緶Χ�����շ�Χ����ͼƴ�ӵ�����
    /// </summary>
    private void MapBoundGet()
    {
        Vector3 orgPos = this.gameObject.transform.position;//��ͼԭ��λ��
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

        connectPoint[0] = orgPos + new Vector3(-width * offsetRatio, 0, 0);       //����
        connectPoint[1] = orgPos + new Vector3(-width * offsetRatio, height * offsetRatio, 0);  //����
        connectPoint[2] = orgPos + new Vector3(0, height * offsetRatio, 0);       //����
        connectPoint[3] = orgPos + new Vector3(width * offsetRatio, height * offsetRatio, 0);   //����
        connectPoint[4] = orgPos + new Vector3(width * offsetRatio, 0, 0);        //����
        connectPoint[5] = orgPos + new Vector3(width * offsetRatio, -height * offsetRatio, 0);  //����
        connectPoint[6] = orgPos + new Vector3(0, height * offsetRatio, 0);       //����
        connectPoint[7] = orgPos + new Vector3(-width * offsetRatio, -height * offsetRatio, 0); //����

    }

    /// <summary>
    /// �������ͼƴ�ӵ���ǰ��ͼ
    /// </summary>
    /// <param name="newMap">��Ҫƴ�ӵ���ͼ��������ͼ�����������</param>
    /// <param name="posNum">�����е�һ��ͼ�ڴ�ͼ�ķ�λ����</param>
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
    /// ͨ�������������е�λ�÷���������ɵ�����
    /// </summary>
    /// <param name="x">���������</param>
    /// <param name="y">����������</param>
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
    /// ͨ�����������������Ϣ�������λ��
    /// </summary>
    /// <param name="x">�����������е�����</param>
    /// <param name="y">�����������е�����</param>
    /// <param name="realDis">�������ǵ�ʵ��ƽ������</param>
    /// <param name="arrXLength">���鳤��</param>
    /// <param name="arrYLength">���鳤��</param>
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
    /// ���ݵ�ǰ��ͼ������������
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
    /// ���ݵ�ǰ�������ɱ���
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
    /// �Գ�ʼ���ɵ���ͼ����һ���������ݱ���ȷ���ȼ����ߵ���
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
            //�ļ��ǣ�����scaleΪ2
            star.gameObject.transform.localScale = Vector3.one * starScale.w * (1 + scaleRandom * symbol);
            star.starLevel = 4;
            star.eatGain = 0.05f;
            level3_4.Add(star);
            star.GetRadious();
        }
        else if (random >= 100 * (1 - nomal.z - nomal.w))
        {
            //�����ǣ�����scaleΪ1.4
            star.gameObject.transform.localScale = Vector3.one * starScale.z * (1 + scaleRandom * symbol);
            star.starLevel = 3;
            star.eatGain = 0.1f;
            level3_4.Add(star);
            star.GetRadious();
        }
        else if (random >= 100 * (1 - nomal.y - nomal.z - nomal.w))
        {
            //�����ǣ�����scaleΪ1
            star.gameObject.transform.localScale = Vector3.one * starScale.y * (1 + scaleRandom * symbol);
            star.starLevel = 2;
            star.eatGain = 0.15f;
            star.GetRadious();
        }
        else
        {
            //һ���ǣ�����scaleΪ0.6
            star.gameObject.transform.localScale = Vector3.one * starScale.x * (1 + scaleRandom * symbol);
            star.starLevel = 1;
            star.eatGain = 0.2f;
            star.GetRadious();
        }
    }

    /// <summary>
    /// ������ͼ�Ķ��״���
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
    /// ������ͼ�����״���
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
    /// �Ա������2�״���
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
