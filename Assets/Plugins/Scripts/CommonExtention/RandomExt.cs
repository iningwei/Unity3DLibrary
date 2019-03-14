using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExt
{
    static int randomTimes = 0;
    static int lastValue = 0;

    static System.Random ran = new System.Random();
    public static int GenerateValue(int min, int max)
    {
        if (min > max)
        {
            int tmp = min;
            min = max;
            max = tmp;
        }
        int seed = DateTime.Now.Millisecond + randomTimes + lastValue;
        //Debug.Log("min:" + min + ",max:" + max + ",seed:" + seed);
        int ran = new System.Random(seed).Next(min, max);
        randomTimes++;
        lastValue = ran;
        return ran;
    }

}
