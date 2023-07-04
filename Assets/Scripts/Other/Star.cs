using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    /// <summary>
    /// 球体的实际半径值，用于计算是否于其他球体产生碰撞
    /// </summary>
    public float radious;
    /// <summary>
    /// 恒星引力大小等级,引力依据此值计算
    /// </summary>
    public int starLevel;
    /// <summary>
    /// 根据当前G值产生的引力，当在计算队列中时计算，直接读取使用即可
    /// </summary>
    public Vector3 force;
    /// <summary>
    /// 星是否加入引力计算队列
    /// </summary>
    public bool inRange;
    /// <summary>
    /// 恒星是否显示，主要用于星图的回收利用
    /// </summary>
    public bool isAlive = true;
    /// <summary>
    /// 星图中此星不显示时，true表示此星是被高级星替代，false表示是被高级星吞噬
    /// </summary>
    public bool isReplace;
    /// <summary>
    /// 此星在星图中的位置表征（当某位置高级星替换低级星时，通过此标记对星图进行还原）
    /// </summary>
    public int xIndex, yIndex;
    /// <summary>
    /// 此星最多吞噬几颗更低级的星
    /// </summary>
    public int eatCount;
    /// <summary>
    /// 此星吞噬其他星时的增益率
    /// </summary>
    public float eatGain;
    /// <summary>
    /// 此星所属于的星图
    /// </summary>
    public MapBlock belongTo;


    private void Update()
    {
        if (isAlive)
        {
            inForceRange();
            if (inRange)
            {
                ForceCacu();
                TriggerAndCacu.sigleton.starInRange.Add(this);
            }
            else
            {
                TriggerAndCacu.sigleton.starInRange.Remove(this);
            }
        }
    }

    /// <summary>
    /// 高级星对低级星的主动吞噬
    /// </summary>
    public void StarEatCheck()
    {
        float[] nearest2 = { -1f, -1f };
        Vector2Int[] index = new Vector2Int[2];
        int arrXL = belongTo.starMap.Length;
        int arrYL = belongTo.starMap[0].Length;
        float dis;
        foreach (Vector2Int v in MyTool.GetAroundItem(xIndex, yIndex, arrXL, arrYL))
        {
            dis = DisInXOY(belongTo.starMap[v.x][v.y]);
            if (nearest2[0] < 0)
            {
                nearest2[0] = dis;
                nearest2[1] = dis;
            }
            else
            {
                if (dis <= nearest2[0])
                {
                    nearest2[0] = dis;
                    index[0] = v;
                }
                else if (dis < nearest2[1])
                {
                    nearest2[1] = dis;
                    index[1] = v;
                }
            }
        }
        Eat(belongTo.starMap[index[0].x][index[0].y]);
        if (belongTo.starMap[index[1].x][index[1].y].radious + radious > nearest2[1])
        {
            Eat(belongTo.starMap[index[1].x][index[1].y]);
        }
        else if (MyTool.randomInt(0, 2) == 2)
        {
            if (eatCount > 0 && starLevel > belongTo.starMap[index[1].x][index[1].y].starLevel)
            {
                Eat(belongTo.starMap[index[1].x][index[1].y]);
            }
        }

    }

    /// <summary>
    /// 合并检查，对所有星进行检查，如果两星相交则进行合并
    /// </summary>
    public void MergeCheck()
    {

    }

    /// <summary>
    /// 判断恒星是否在引力作用范围内并修改状态
    /// </summary>
    public void inForceRange()
    {
        if (DisInXOY(this.gameObject.transform.position, TriggerAndCacu.sigleton.mainPos) >= TriggerAndCacu.sigleton.forceMaxDis)
        {
            inRange = false;
        }
        else
        {
            inRange = true;
        }
    }

    /// <summary>
    /// 计算两个三维向量在XOY平面内的距离
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public float DisInXOY(Vector3 start, Vector3 end)
    {
        return (start.x - end.x) * (start.x - end.x) + (start.y - end.y) * (start.y - end.y);
    }

    /// <summary>
    /// 获取此星与传入星的距离
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public float DisInXOY(Star star)
    {
        return DisInXOY(this.gameObject.transform.position, star.gameObject.transform.position);
    }

    /// <summary>
    /// 计算此星产生的引力值
    /// </summary>
    /// <returns></returns>
    public void ForceCacu()
    {
        Vector3 v3 = gameObject.transform.position - TriggerAndCacu.sigleton.mainPos;
        float dis = DisInXOY(gameObject.transform.position, TriggerAndCacu.sigleton.mainPos);

        force = radious * TriggerAndCacu.sigleton.gScale * v3 / dis;
    }

    /// <summary>
    /// 恒星是否存活（为false则此星在游戏中不显示和也不产生力）
    /// </summary>
    /// <param name="state"></param>
    public void AliveStateCg(bool state)
    {
        this.gameObject.SetActive(state);
        isAlive = state;
    }
    /// <summary>
    /// 将此星隐藏
    /// </summary>
    public void Died()
    {
        //Debug.Log(this.gameObject.name + "is died；此星等级为：" + starLevel);
        belongTo.diedStars.Add(this);
        AliveStateCg(false);
    }
    /// <summary>
    /// 将此星显示
    /// </summary>
    public void Born()
    {
        AliveStateCg(true);
    }
    /// <summary>
    /// 星体之间的吞噬
    /// </summary>
    /// <param name="star"></param>
    public void Eat(Star star)
    {
        //Debug.Log(this.gameObject.name + "等级为：" + starLevel + "----吞噬了" + star.gameObject.name + "等级为：" + star.starLevel);
        if (star.starLevel > starLevel)
        {
            star.Eat(this);
        }
        else
        {
            this.gameObject.transform.localScale *= (1 + eatGain * star.starLevel);
            eatCount--;
            star.Died();
        }
    }

    public void GetMaterial()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();

        if (starLevel == 4)
        {
            if (MyTool.randomInt(0, 20) > 18)
            {
                rend.material = TriggerAndCacu.sigleton.materials[7];
            }
            else
            {
                rend.material = TriggerAndCacu.sigleton.materials[6];
            }
        }
        else
        {
            rend.material = TriggerAndCacu.sigleton.materials[MyTool.randomInt(0, 6)];
        }
    }

    /// <summary>
    /// 计算此时球体的实际半径
    /// </summary>
    public void GetRadious()
    {
        radious = gameObject.transform.localScale.x / 2;
    }
    public void PosCg(Star star)
    {
        Vector3 posT = star.gameObject.transform.position;
        int xT = star.xIndex;
        int yT = star.yIndex;

        belongTo.starMap[xT][yT] = this;
        belongTo.starMap[xIndex][yIndex] = star;

        star.gameObject.transform.position = this.gameObject.transform.position;
        star.xIndex = this.xIndex;
        star.yIndex = this.yIndex;

        this.gameObject.transform.position = posT;
        xIndex = xT;
        yIndex = yT;

        this.gameObject.name = "star" + xIndex + "_" + yIndex;
        star.gameObject.name = "star" + star.xIndex + "_" + star.yIndex;
    }


    /// <summary>
    /// 恒星是否加入引力计算队列修改
    /// </summary>
    /// <param name="state"></param>
    public void ForceStateChg(bool state)
    {
        inRange = state;
    }

    public bool Crashed()
    {
        if (starLevel >= 2)
        {
            float dis = DisInXOY(gameObject.transform.position, TriggerAndCacu.sigleton.mainPos);
            if ( dis < radious)
            {
                return true;
            }
        }
        return false;
    }
}
