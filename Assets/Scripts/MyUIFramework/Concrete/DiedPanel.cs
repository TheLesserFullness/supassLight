using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ��Ϸ����ҳ��
/// </summary>
public class DiedPanel : UIBase
{
    public DiedPanel() : base("PanelDied", UILayerType.Normal) { }

    GameObject btnAgain;
    GameObject btnExit;
    Text textEndMsg;
    string[] diedMsg = { "һҶ��֪�������ɼ��ǳ����", "һ�٣�����٣���һ˲", 
                            "�����棬�������棬����棬�漴��","�������죬ܿܿ��ǧ",
                        "������������������������ת","�����������ֻز�ֹ","һ���ü��������̳��������ǳ�"
                        };


    public override void Init()
    {
        base.Init();

        btnAgain = uiGameObject.transform.Find("PanelBlack/BtnAgain").gameObject;
        btnExit = uiGameObject.transform.Find("PanelBlack/BtnExit").gameObject;
        textEndMsg = uiGameObject.transform.Find("PanelBlack/TextEndMsg").GetComponent<Text>();

        btnAgain.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            BtnAgainDown();
        });

        btnExit.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            BtnExitGameDown();
        });
    }

    void BtnAgainDown()
    {
        TriggerAndCacu.sigleton.ReStart();
        UIManager.singleton.CloseUI(UIManager.PANEL_Died);
    }

    void BtnExitGameDown()
    {
        GameRoot.instance.ExitGame();
    }

    protected override void OnShow()
    {
        int ran = MyTool.randomInt(0, 10);
        if(ran <= 8)
        {
            textEndMsg.text = "�Ŀ���";
        }
        else
        {
            textEndMsg.text = diedMsg[MyTool.randomInt(0, diedMsg.Length - 1)];
        }
        
        GameRoot.instance.OnPause();
    }

    protected override void OnHide()
    {
        GameRoot.instance.UnPause();
    }
}

