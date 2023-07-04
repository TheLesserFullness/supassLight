using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 游戏结束页面
/// </summary>
public class DiedPanel : UIBase
{
    public DiedPanel() : base("PanelDied", UILayerType.Normal) { }

    GameObject btnAgain;
    GameObject btnExit;
    Text textEndMsg;
    string[] diedMsg = { "一叶可知秋深，尘亦可见星辰浩瀚", "一劫，亿万劫，亦一瞬", 
                            "辰有真，尘亦有真，真见真，真即真","尘埃虽渺，芸芸万千",
                        "不见尘，见流；不见辰，见转","尘生辰死，轮回不止","一朝得见，可入烟尘，可入星辰"
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
            textEndMsg.text = "寄咯！";
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

