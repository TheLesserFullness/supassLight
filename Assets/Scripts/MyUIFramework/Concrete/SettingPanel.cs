using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase
{   
    /// <summary>
    /// 当前UI的路径
    /// </summary>
    //static readonly string path = "Prefabs/UI/Panel/PanelSetting";

    public SettingPanel() : base("PanelSetting", UILayerType.Upper) { }

    GameObject btnExit;
    GameObject bntChangeKey;
    GameObject btnGoOn;
    GameObject btnExitGame;

    public override void Init()
    {
        base.Init();
        btnExit = uiGameObject.transform.Find("PanelBlack/PanelButtons/BtnExit").gameObject;
        bntChangeKey = uiGameObject.transform.Find("PanelBlack/PanelButtons/BtnChangeBtn").gameObject;
        btnGoOn = uiGameObject.transform.Find("PanelBlack/PanelButtons/BtnGoOn").gameObject;
        btnExitGame = uiGameObject.transform.Find("PanelBlack/PanelButtons/BtnExitGame").gameObject;

        btnExit.GetComponent<Button>().onClick.AddListener(() =>
        {
            BtnExitDown();
        });
        bntChangeKey.GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.singleton.OpenUI(UIManager.PANEL_KeySet);
        });
        btnGoOn.GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.singleton.CloseUI(UIManager.PANEL_Setting);
        });
        btnExitGame.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameRoot.instance.ExitGame();
        });
    }

    void BtnExitDown()
    {
        UIManager.singleton.CloseUI(UIManager.PANEL_Setting);
        
    }

    protected override void OnShow()
    {
        GameRoot.instance.OnPause();
    }

    protected override void OnHide()
    {
        GameRoot.instance.UnPause();
    }

}
