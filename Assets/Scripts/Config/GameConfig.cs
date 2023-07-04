using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig :ScriptableObject
{
    /// <summary>
    /// ��ͼ�߶�
    /// </summary>
    public int mapHeight;
    /// <summary>
    /// ��߱�
    /// </summary>
    public int w_hRatio;
    /// <summary>
    /// ��λ����
    /// </summary>
    public int unitValue;
    /// <summary>
    /// ƽ���Ǽ��
    /// </summary>
    public int starAvgDis;
    /// <summary>
    /// ��ͼ������Χ
    /// </summary>
    public float MapCreateRange;
    /// <summary>
    /// ��ͼ���շ�Χ
    /// </summary>
    public float mapRecycleRange;


    /// <summary>
    /// ��ʼGϵ��
    /// </summary>
    public int initialGScale;
    /// <summary>
    /// ��ʼTϵ��
    /// </summary>
    public int initialTimeScale;

    /// <summary>
    /// Gϵ������
    /// </summary>
    public KeyCode upG;
    /// <summary>
    /// Gϵ����С
    /// </summary>
    public KeyCode downG;
    /// <summary>
    /// Tϵ������
    /// </summary>
    public KeyCode upT;
    /// <summary>
    /// Tϵ����С
    /// </summary>
    public KeyCode downT;
    /// <summary>
    /// �����ý���
    /// </summary>
    public KeyCode setting;
}
