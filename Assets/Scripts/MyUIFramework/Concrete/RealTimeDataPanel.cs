using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ��Ϸ�����е�ҳ��
/// </summary>
public class RealTimeDataPanel : UIBase
{
    public RealTimeDataPanel() : base("PanelRealTimeData", UILayerType.Normal) { }


    struct dataStruct
    {
        public string textName;
        public string textValue;

        public dataStruct(string name, string value)
        {
            textName = name;
            textValue = value;
        }
    };

    public GameObject dataItem;
    public GameObject btnSet;
    public Text[] textName;
    public Text[] textValue;
    dataStruct[] itemData = { new dataStruct("Gϵ��", "1"), new dataStruct("Tϵ��", "1"),  };


    public override void Init()
    {
        base.Init();
        textName = new Text[itemData.Length];
        textValue = new Text[itemData.Length];
        btnSet = uiGameObject.transform.Find("Top/BtnSet").gameObject;
        Transform transPafent = uiGameObject.transform.Find("Top/PanelData");
        dataItem = transPafent.Find("DataItem").gameObject;

        GameObject item;
        for (int i = 0; i < itemData.Length; i++)
        {
            item = GameObject.Instantiate(dataItem,transPafent);
            item.gameObject.SetActive(true);
            textName[i] = item.transform.Find("TextName").GetComponent<Text>();
            textName[i].text = itemData[i].textName;
            textValue[i] = item.transform.Find("TextValue").GetComponent<Text>();
            textValue[i].text = itemData[i].textValue;
        }

        btnSet.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            BtnSetDown();
        });
    }


    void BtnSetDown()
    {
        UIManager.singleton.OpenUI(UIManager.PANEL_Setting);
    }

    public override void Update()
    {
        textValue[0].text = TriggerAndCacu.sigleton.gScale.ToString();
        textValue[1].text = TriggerAndCacu.sigleton.timeScale.ToString();
    }


    protected override void OnShow()
    {
        //Debug.Log($"ҳ�棺{uiName}��ʾ");
    }

    protected override void OnHide()
    {
        //Debug.Log($"ҳ�棺{uiName}����");
    }
}
