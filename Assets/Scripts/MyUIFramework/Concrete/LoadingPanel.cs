using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 资源加载页面
/// </summary>
public class LoadingPanel : UIBase
{
    public LoadingPanel() : base("PanelLoading", UILayerType.Normal) { }


    public override void Init()
    {
        base.Init();

    }

    protected override void OnShow()
    {
        //Debug.Log("load panel open ++++++");
    }

    protected override void OnHide()
    {
        //Debug.Log("load panel hide ++++++______");
    }
}
