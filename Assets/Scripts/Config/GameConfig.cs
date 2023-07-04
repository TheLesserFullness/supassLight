using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig :ScriptableObject
{
    /// <summary>
    /// 星图高度
    /// </summary>
    public int mapHeight;
    /// <summary>
    /// 宽高比
    /// </summary>
    public int w_hRatio;
    /// <summary>
    /// 单位长度
    /// </summary>
    public int unitValue;
    /// <summary>
    /// 平均星间距
    /// </summary>
    public int starAvgDis;
    /// <summary>
    /// 星图创建范围
    /// </summary>
    public float MapCreateRange;
    /// <summary>
    /// 星图回收范围
    /// </summary>
    public float mapRecycleRange;


    /// <summary>
    /// 初始G系数
    /// </summary>
    public int initialGScale;
    /// <summary>
    /// 初始T系数
    /// </summary>
    public int initialTimeScale;

    /// <summary>
    /// G系数增大
    /// </summary>
    public KeyCode upG;
    /// <summary>
    /// G系数减小
    /// </summary>
    public KeyCode downG;
    /// <summary>
    /// T系数增大
    /// </summary>
    public KeyCode upT;
    /// <summary>
    /// T系数减小
    /// </summary>
    public KeyCode downT;
    /// <summary>
    /// 打开设置界面
    /// </summary>
    public KeyCode setting;
}
