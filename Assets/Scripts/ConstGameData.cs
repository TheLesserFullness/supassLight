using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstGameData 
{
    /// <summary>
    /// 游戏配置数据保存的位置
    /// </summary>
    public const string savePath = "Assets/Config.asset";

    public const string blockBg = "Block_BackgroundStar";
    public const string blockMap = "Block_NormalStar";

    /// <summary>
    /// 使G系数增大的默认按键值
    /// </summary>
    public const KeyCode const_upG = KeyCode.Q;
    /// <summary>
    /// 使G系数减小的默认按键值
    /// </summary>
    public const KeyCode const_downG = KeyCode.A;
    /// <summary>
    /// 使T系数增大的默认按键值
    /// </summary>
    public const KeyCode const_upT = KeyCode.E;
    /// <summary>
    /// 使G系数减小的默认按键值
    /// </summary>
    public const KeyCode const_downT = KeyCode.D;


    public const int const_mapHeight = 100;

    public const int const_w_hRatio = 1;

    public const int unitValue = 1;
}
