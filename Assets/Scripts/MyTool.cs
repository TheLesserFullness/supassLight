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
    /// 返回键盘中一个按键对应的整数值
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
    /// 在XOY平面内根据传入的力和增益系数返回加速度（简单模拟）
    /// </summary>
    /// <param name="force">当前力</param>
    /// <param name="gain">增益系数</param>
    /// <returns></returns>
    public static Vector3 GetV3SpeedInXOY(Vector3 force,float gain)
    {
        return new Vector3(force.x * gain, force.y * gain, 0);
    }

    /// <summary>
    /// 根据顶点数组依次链接绘制线条
    /// </summary>
    /// <param name="vertexes">顶点</param>
    /// <param name="color">线条颜色</param>
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
    /// 获取一个数组中最大的N个值
    /// </summary>
    /// <param name="count">获取几个数</param>
    /// <param name="arr">目标数组</param>
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
    /// 获取一个数组中指定索引区间内的最大值和最小值
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="startIndex">起始位置</param>
    /// <param name="offset">偏移量</param>
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
    /// 按从小到大的顺序返回目标数组中的最小值和最大值
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
    /// 返回目标数组中的最大值
    /// </summary>
    /// <param name="arr">目标数组</param>
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
    /// 返回两个数中较大的数
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
    /// 返回两个数中较小的数
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
    /// 返回目标数组中的最小值
    /// </summary>
    /// <param name="arr">目标数组</param>
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
    /// 将一个索引在数组内偏移一段距离
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="arrLength">环的周长，即环的最大值</param>
    /// <param name="offset">整数偏移量</param>
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
    /// 将一个索引在数组内偏移一段距离(百分比)
    /// </summary>
    /// <param name="index">当前索引</param>
    /// <param name="arrLength">数组长度</param>
    /// <param name="offset">百分比偏移量，符号既改变方向，也改变长度</param>
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
    /// 根据向量的值返回每个值所占百分比
    /// </summary>
    /// <param name="v4"></param>
    /// <returns></returns>
    public static Vector4 GetRatio(Vector4 v4)
    {
        float sum = Mathf.Abs(v4.x) + Mathf.Abs(v4.y) + Mathf.Abs(v4.z) + Mathf.Abs(v4.w);
        return new Vector4(Mathf.Abs(v4.x) / sum, Mathf.Abs(v4.y) / sum, Mathf.Abs(v4.z) / sum, Mathf.Abs(v4.w) / sum);
    }

    /// <summary>
    /// 根据数组的维数返回指定索引的相邻元素索引
    /// </summary>
    /// <param name="xIndex">目标位置横向索引</param>
    /// <param name="yIndex">目标位置纵向索引</param>
    /// <param name="arrXLength">数组横向长度</param>
    /// <param name="arrYLength">数组纵向长度</param>
    /// <returns>以二维向量形式的数组返回相邻元素</returns>
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
    /// 根据数组的维数返回指定位置处一定范围内的相邻元素索引
    /// </summary>
    /// <param name="xIndex">目标位置横向索引</param>
    /// <param name="yIndex">目标位置纵向索引</param>
    /// <param name="arrXLength">数组横向长度</param>
    /// <param name="arrYLength">数组纵向长度</param>
    /// <param name="offset">相邻范围判定</param>
    /// <returns>以二维向量形式的数组返回相邻元素</returns>
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
    /// 获取一个（-rangeLimit,rangeLimit）的随机数
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
    /// 随机数获取
    /// </summary>
    /// <param name="rangeLimit">随机数的范围</param>
    /// <param name="digtal">随机数的位数</param>
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
    /// 随机小数获取，（-分子/分母，分子/分母）之间的小数
    /// </summary>
    /// <param name="fenZi">分子</param>
    /// <param name="fenMu">分母</param>
    /// <param name="digtal">保留几位小数</param>
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
    /// 返回一个min到max之间的随机整数,-1000,到1000之间
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
    /// 返回一个在（min，max）或（min1，max1）之间的随机整数
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
