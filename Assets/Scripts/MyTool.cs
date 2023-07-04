using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public static class MyTool 
{
    public static int[] seedArr;
    public static int[] indexPool;
    public static int seedIndex = 0;

    public static float LimitValue(float val, float min,float max)
    {
        if(val < min)
        {
            return min;
        }
        if (val > max)
        {
            return max;
        }
        return val;
    }

    /// <summary>
    /// ���ؼ�����һ��������Ӧ������ֵ
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int GetKeyIntValue(KeyCode key)
    {
        return (int)Enum.Parse(typeof(KeyCode), key.ToString());
    }

    public static float GetLengthInXOY(Vector3 v3)
    {
        Vector2 v2 = new Vector2(v3.x,v3.y);
        return v2.magnitude;
    }

    /// <summary>
    /// ��XOYƽ���ڸ��ݴ������������ϵ�����ؼ��ٶȣ���ģ�⣩
    /// </summary>
    /// <param name="force">��ǰ��</param>
    /// <param name="gain">����ϵ��</param>
    /// <returns></returns>
    public static Vector3 GetV3SpeedInXOY(Vector3 force,float gain)
    {
        return new Vector3(force.x * gain, force.y * gain, 0);
    }

    /// <summary>
    /// ���ݶ��������������ӻ�������
    /// </summary>
    /// <param name="vertexes">����</param>
    /// <param name="color">������ɫ</param>
    public static void DrawSides(Vector3[] vertexes, Color color)
    {
        int vCount = vertexes.Length;
        Vector3 start;
        Vector3 end;
        if (vCount > 2)
        {
            for (int i = 0; i < vCount; i++)
            {
                start = vertexes[i];
                end = vertexes[i + 1 >= vCount ? 0 : i + 1];
                Debug.DrawLine(start, end, color);
            }
        }
        else
        {
            if (vCount > 1)
            {
                Debug.DrawLine(vertexes[0], vertexes[1], color);
            }
        }

    }

    /// <summary>
    /// ��ȡһ������������N��ֵ
    /// </summary>
    /// <param name="count">��ȡ������</param>
    /// <param name="arr">Ŀ������</param>
    /// <returns></returns>
    public static float[] GetNMaxOfArr(int count,float[] arr)
    {
        if (count >= arr.Length)
        {
            return arr;
        }
        float[] maxNumArr = new float[count];
        for(int i = 0; i < count; i++)
        {

        }
        return maxNumArr;
    }
    /// <summary>
    /// ��ȡһ��������ָ�����������ڵ����ֵ����Сֵ
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="startIndex">��ʼλ��</param>
    /// <param name="offset">ƫ����</param>
    /// <returns></returns>
    public static float[] GetMinAndMax(float[] arr,int startIndex,int offset)
    {
        float max = arr[startIndex];
        float min = arr[startIndex];
        for (int i = startIndex + 1; i < Min(arr.Length,offset + startIndex); i++)
        {
            if (arr[i] >= max)
            {
                max = arr[i];
            }
            else if (arr[i] < min)
            {
                min = arr[i];
            }
        }
        float[] result = { min, max };
        return result;
    }
    /// <summary>
    /// ����С�����˳�򷵻�Ŀ�������е���Сֵ�����ֵ
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static float[] GetMinAndMax(float[] arr)
    {

        float max = arr[0];
        float min = arr[0];
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] >= max)
            {
                max = arr[i];
            }
            else if (arr[i] < min)
            {
                min = arr[i];
            }
        }
        float[] result = { min, max };
        return result;
    }
    /// <summary>
    /// ����Ŀ�������е����ֵ
    /// </summary>
    /// <param name="arr">Ŀ������</param>
    /// <returns></returns>
    public static float Max(float[] arr)
    {
        float max = arr[0];
        for(int i = 1; i < arr.Length; i++)
        {
            if (arr[i] > max)
            {
                max = arr[i];
            }
        }

        return max;
    }
    /// <summary>
    /// �����������нϴ����
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static float Max(float num1,float num2)
    {
        if (num1 > num2)
        {
            return num1;
        }
        else
        {
            return num2;
        }
    }
    /// <summary>
    /// �����������н�С����
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static float Min(float num1, float num2)
    {
        if (num1 > num2)
        {
            return num2;
        }
        else
        {
            return num1;
        }
    }
    /// <summary>
    /// ����Ŀ�������е���Сֵ
    /// </summary>
    /// <param name="arr">Ŀ������</param>
    /// <returns></returns>
    public static float Min(float[] arr)
    {
        float min = arr[0];
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] < min)
            {
                min = arr[i];
            }
        }
        return min;
    }

    /// <summary>
    /// ��һ��������������ƫ��һ�ξ���
    /// </summary>
    /// <param name="index">����</param>
    /// <param name="arrLength">�����ܳ������������ֵ</param>
    /// <param name="offset">����ƫ����</param>
    /// <returns></returns>
    public static int NumLoop(int index, int arrLength, int offset)
    {
        int res = index + offset;
        if (res > arrLength)
        {
            res = (res % arrLength) - arrLength;
        }
        else if (res < 0)
        {
            res = arrLength + (res % arrLength);
        }
        return res;
    }
    /// <summary>
    /// ��һ��������������ƫ��һ�ξ���(�ٷֱ�)
    /// </summary>
    /// <param name="index">��ǰ����</param>
    /// <param name="arrLength">���鳤��</param>
    /// <param name="offset">�ٷֱ�ƫ���������żȸı䷽��Ҳ�ı䳤��</param>
    /// <returns></returns>
    public static int NumLoop(int index, int arrLength, float offset)
    {
        int _offset;
        if (offset > 0)
        {
            _offset =(int)((arrLength - index) * offset);
        }
        else
        {
            _offset = (int)(index * offset);
        }
        return NumLoop(index, arrLength, _offset);
    }

    /// <summary>
    /// ����������ֵ����ÿ��ֵ��ռ�ٷֱ�
    /// </summary>
    /// <param name="v4"></param>
    /// <returns></returns>
    public static Vector4 GetRatio(Vector4 v4)
    {
        float sum = Mathf.Abs(v4.x) + Mathf.Abs(v4.y) + Mathf.Abs(v4.z) + Mathf.Abs(v4.w);
        return new Vector4(Mathf.Abs(v4.x) / sum, Mathf.Abs(v4.y) / sum, Mathf.Abs(v4.z) / sum, Mathf.Abs(v4.w) / sum);
    }

    /// <summary>
    /// ���������ά������ָ������������Ԫ������
    /// </summary>
    /// <param name="xIndex">Ŀ��λ�ú�������</param>
    /// <param name="yIndex">Ŀ��λ����������</param>
    /// <param name="arrXLength">������򳤶�</param>
    /// <param name="arrYLength">�������򳤶�</param>
    /// <returns>�Զ�ά������ʽ�����鷵������Ԫ��</returns>
    public static List<Vector2Int> GetAroundItem(int xIndex,int yIndex,int arrXLength,int arrYLength)
    {
        List<Vector2Int> aroundItem = new List<Vector2Int>();
        int itemx, itemy;
        for(int x = 0; x < 3; x++)
        {
            itemx = xIndex + x - 1;
            if(itemx >= 0 && itemx < arrXLength)
            {
                for (int y = 0; y < 3; y++)
                {
                    itemy = yIndex + y - 1;
                    if ((itemy >= 0 && itemy < arrYLength) && !(itemx == xIndex && itemy == yIndex))
                    {
                        aroundItem.Add(new Vector2Int(itemx, itemy));
                    }
                }
            }
            
        }
        return aroundItem;
    }

    /// <summary>
    /// ���������ά������ָ��λ�ô�һ����Χ�ڵ�����Ԫ������
    /// </summary>
    /// <param name="xIndex">Ŀ��λ�ú�������</param>
    /// <param name="yIndex">Ŀ��λ����������</param>
    /// <param name="arrXLength">������򳤶�</param>
    /// <param name="arrYLength">�������򳤶�</param>
    /// <param name="offset">���ڷ�Χ�ж�</param>
    /// <returns>�Զ�ά������ʽ�����鷵������Ԫ��</returns>
    public static List<Vector2Int> GetAroundItem(int xIndex, int yIndex, int arrXLength, int arrYLength,int offset)
    {
        List<Vector2Int> aroundItem = new List<Vector2Int>();
        int itemx, itemy;
        for (int x = 0; x < 3 + offset; x++)
        {
            itemx = xIndex + x - offset;
            if (itemx >= 0 && itemx < arrXLength)
            {
                for (int y = 0; y < 3 + offset; y++)
                {
                    itemy = yIndex + y - offset;
                    if ((itemy >= 0 && itemy < arrYLength) && !(itemx == xIndex && itemy == yIndex))
                    {
                        aroundItem.Add(new Vector2Int(itemx, itemy));
                    }
                }
            }

        }
        return aroundItem;
    }


    /// <summary>
    /// ��ȡһ����-rangeLimit,rangeLimit���������
    /// </summary>
    /// <param name="rangeLimit"></param>
    /// <returns></returns>
    public static float randomNum(int rangeLimit)
    {
        System.Random ra = new System.Random();
        int n = ra.Next(-rangeLimit, rangeLimit);
        return n;
    }

    /// <summary>
    /// �������ȡ
    /// </summary>
    /// <param name="rangeLimit">������ķ�Χ</param>
    /// <param name="digtal">�������λ��</param>
    /// <returns></returns>
    public static float randomNum(int rangeLimit, int digtal)
    {
        int num = 1;
        if (digtal != 0)
        {
            for (int i = 0; i < Mathf.Abs(digtal); i++)
            {
                num = num * 10;
            }
        }
        System.Random ra = new System.Random();
        int n = ra.Next(-rangeLimit * num, rangeLimit * num);
        return n / num;
    }

    /// <summary>
    /// ���С����ȡ����-����/��ĸ������/��ĸ��֮���С��
    /// </summary>
    /// <param name="fenZi">����</param>
    /// <param name="fenMu">��ĸ</param>
    /// <param name="digtal">������λС��</param>
    /// <returns></returns>
    public static float randomNum(int fenZi, int fenMu, int digtal)
    {
        int num = 1;
        if (digtal != 0)
        {
            for (int i = 0; i < Mathf.Abs(digtal) - 1; i++)
            {
                num = num * 10;
            }
        }
        System.Random ra = new System.Random(GetMemory(10));
        int n = ra.Next(-fenZi * num, fenZi * num);
        float value = (float)n / (float)(num * fenMu);
        return value;
    }

    /// <summary>
    /// ����һ��min��max֮����������,-1000,��1000֮��
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static int randomInt(int min,int max)
    {
        System.Random ra = new System.Random(GetMemory(GetSeed()));
        int value = ra.Next(min, max + 1);
        return value;
    }
    /// <summary>
    /// ����һ���ڣ�min��max����min1��max1��֮����������
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="min1"></param>
    /// <param name="max1"></param>
    /// <returns></returns>
    public static int randomInt(int min, int max,int min1,int max1)
    {
        if (randomInt(0, 5) >= 2)
        {
            return randomInt(min, max);
        }
        else
        {
            return randomInt(min1, max1);
        }
    }

    public static int randomInt1(int min, int max)
    {
        //SeedArrCheck();

        System.Random ra = new System.Random(GetMemory(GetSeed() * 2));
        int value = ra.Next(min, max + 1);
        return value;
    }

    public static int GetMemory(object o)
    {
        GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);
        return int.Parse(GCHandle.ToIntPtr(h).ToString());
    }


    public static void SeedArrCheck()
    {
        if (seedArr == null)
        {
            seedArr = new int[2000];
            seedArr[0] = -1000;
            for (int i = 1; i < seedArr.Length; i++)
            {
                seedArr[i] = seedArr[i - 1] + 1;
            }
        }
    }
    public static int GetSeed()
    {
        SeedArrCheck();
        if (seedIndex > seedArr.Length - 1)
        {
            seedIndex = 0;
        }
        int seed = seedArr[seedIndex];
        seedIndex++;
        return seed;
    }
}
