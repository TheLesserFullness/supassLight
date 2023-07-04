using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 按键设置界面
/// </summary>
public class KeySetPanel : UIBase
{
    public KeySetPanel() : base("PanelKeySet", UILayerType.Top) { }


    struct dataItem
    {
        public string textName;
        public string textValue;

        public dataItem(string name, string value)
        {
            textName = name;
            textValue = value;
        }
    };

    public int nowIndex = -1;

    dataItem[] btnData = { new dataItem("G系数增益","Q"),new dataItem("G系数减益","A"), new dataItem("T系数增益", "E"), new dataItem("T系数减益", "D") };
    GameObject btnItem;
    GameObject btnExit;
    GameObject[] btnObj;
    Text[] textName;
    Text[] textValue;
    public override void Init()
    {
        base.Init();
        Transform transBtnParent = uiGameObject.transform.Find("PanelBlack/Scroll View/Viewport/Content"); 
        btnItem = transBtnParent.Find("BtnItem").gameObject;
        btnExit = uiGameObject.transform.Find("PanelBlack/Scroll View/Viewport/BtnExit").gameObject;

        //btnData = { new dataItem("G系数增益", "Q"),new dataItem("G系数减益", "A"), new dataItem("T系数增益", "E"), new dataItem("T系数减益", "D") };

        btnObj = new GameObject[btnData.Length];
        textName = new Text[btnData.Length];
        textValue = new Text[btnData.Length];
        for (int i= 0; i < btnData.Length; i++)
        {
            btnObj[i] = GameObject.Instantiate(btnItem, transBtnParent);
            btnObj[i].SetActive(true);
            textName[i] = btnObj[i].transform.Find("TextName").gameObject.GetComponent<Text>();
            textValue[i] = btnObj[i].transform.Find("TextKey").gameObject.GetComponent<Text>();
            int temp = i;
            textName[i].text = btnData[i].textName;
            textValue[i].text = btnData[i].textValue;
            btnObj[i].GetComponent<Button>()?.onClick.AddListener(() =>
            {
                KeyChangeDown(temp);
            });
        }


        btnExit.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            BtnExitDown();
        });


    }

    /// <summary>
    /// 根据按键索引修改数据
    /// </summary>
    /// <param name="index"></param>
    void KeyChangeDown(int index)
    {
        if (nowIndex >= 0)
        {
            textName[nowIndex].color = Color.black;
            textValue[nowIndex].color = Color.black;
        }
        textName[index].color = Color.red;
        textValue[index].color = Color.red;
        nowIndex = index;
    }

    void BtnExitDown()
    {
        UIManager.singleton.CloseUI(UIManager.PANEL_KeySet);
    }

    public override void Update()
    {
        textValue[0].text = GameRoot.instance.upG.ToString();
        textValue[1].text = GameRoot.instance.downG.ToString();
        textValue[2].text = GameRoot.instance.upT.ToString();
        textValue[3].text = GameRoot.instance.downT.ToString();
    }



    protected override void OnShow()
    {
        Update();
    }

    protected override void OnHide()
    {
        if (nowIndex >= 0)
        {
            textName[nowIndex].color = Color.black;
            textValue[nowIndex].color = Color.black;
        }
        nowIndex = -1;
    }

}
