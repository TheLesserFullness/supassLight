using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    /// <summary>
    /// �����ʵ�ʰ뾶ֵ�����ڼ����Ƿ����������������ײ
    /// </summary>
    public float radious;
    /// <summary>
    /// ����������С�ȼ�,�������ݴ�ֵ����
    /// </summary>
    public int starLevel;
    /// <summary>
    /// ���ݵ�ǰGֵ���������������ڼ��������ʱ���㣬ֱ�Ӷ�ȡʹ�ü���
    /// </summary>
    public Vector3 force;
    /// <summary>
    /// ���Ƿ���������������
    /// </summary>
    public bool inRange;
    /// <summary>
    /// �����Ƿ���ʾ����Ҫ������ͼ�Ļ�������
    /// </summary>
    public bool isAlive = true;
    /// <summary>
    /// ��ͼ�д��ǲ���ʾʱ��true��ʾ�����Ǳ��߼��������false��ʾ�Ǳ��߼�������
    /// </summary>
    public bool isReplace;
    /// <summary>
    /// ��������ͼ�е�λ�ñ�������ĳλ�ø߼����滻�ͼ���ʱ��ͨ���˱�Ƕ���ͼ���л�ԭ��
    /// </summary>
    public int xIndex, yIndex;
    /// <summary>
    /// ����������ɼ��Ÿ��ͼ�����
    /// </summary>
    public int eatCount;
    /// <summary>
    /// ��������������ʱ��������
    /// </summary>
    public float eatGain;
    /// <summary>
    /// ���������ڵ���ͼ
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
    /// �߼��ǶԵͼ��ǵ���������
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
    /// �ϲ���飬�������ǽ��м�飬��������ཻ����кϲ�
    /// </summary>
    public void MergeCheck()
    {

    }

    /// <summary>
    /// �жϺ����Ƿ����������÷�Χ�ڲ��޸�״̬
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
    /// ����������ά������XOYƽ���ڵľ���
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public float DisInXOY(Vector3 start, Vector3 end)
    {
        return (start.x - end.x) * (start.x - end.x) + (start.y - end.y) * (start.y - end.y);
    }

    /// <summary>
    /// ��ȡ�����봫���ǵľ���
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    public float DisInXOY(Star star)
    {
        return DisInXOY(this.gameObject.transform.position, star.gameObject.transform.position);
    }

    /// <summary>
    /// ������ǲ���������ֵ
    /// </summary>
    /// <returns></returns>
    public void ForceCacu()
    {
        Vector3 v3 = gameObject.transform.position - TriggerAndCacu.sigleton.mainPos;
        float dis = DisInXOY(gameObject.transform.position, TriggerAndCacu.sigleton.mainPos);

        force = radious * TriggerAndCacu.sigleton.gScale * v3 / dis;
    }

    /// <summary>
    /// �����Ƿ��Ϊfalse���������Ϸ�в���ʾ��Ҳ����������
    /// </summary>
    /// <param name="state"></param>
    public void AliveStateCg(bool state)
    {
        this.gameObject.SetActive(state);
        isAlive = state;
    }
    /// <summary>
    /// ����������
    /// </summary>
    public void Died()
    {
        //Debug.Log(this.gameObject.name + "is died�����ǵȼ�Ϊ��" + starLevel);
        belongTo.diedStars.Add(this);
        AliveStateCg(false);
    }
    /// <summary>
    /// ��������ʾ
    /// </summary>
    public void Born()
    {
        AliveStateCg(true);
    }
    /// <summary>
    /// ����֮�������
    /// </summary>
    /// <param name="star"></param>
    public void Eat(Star star)
    {
        //Debug.Log(this.gameObject.name + "�ȼ�Ϊ��" + starLevel + "----������" + star.gameObject.name + "�ȼ�Ϊ��" + star.starLevel);
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
    /// �����ʱ�����ʵ�ʰ뾶
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
    /// �����Ƿ����������������޸�
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
