using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始主面板
/// </summary>
public class StartPanel : UIBase
{
    //static readonly string path = "Prefabs/UI/Panel/StartPanel";

    public StartPanel() : base("PanelBegin", UILayerType.Normal) { }



    public GameObject btnStart;
    public GameObject btnSet;



    public override void Init()
    {
        base.Init();
        btnStart = uiGameObject.transform .Find("PanelBG/BtnStart").gameObject;
        btnSet = uiGameObject.transform.Find("PanelBG/BtnSet").gameObject;

        btnStart.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            BtnStartDown();
        });
        btnSet.GetComponent<Button>()?.onClick.AddListener(() =>
        {
            BtnSetDown();
        });
    }

    void BtnStartDown()
    {
        UIManager.singleton.OpenUI(UIManager.PANEL_Loading);
        GameRoot.instance.sceneSystem.SetScene(new MainScene());
    }

    void BtnSetDown()
    {
        UIManager.singleton.OpenUI(UIManager.PANEL_KeySet);
    }

    protected override void OnShow()
    {

    }

    protected override void OnHide()
    {

    }
}
