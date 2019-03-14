using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExt
{

    /// <summary>
    /// 判断字符串是否包含汉字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool ContainChinese(this string text)
    {
        bool res = false;
        foreach (char t in text)
        {
            if ((int)t > 127)
                res = true;
        }
        return res;
    }


    //反斜杠\ backslash
    //斜杠/  slash
    /// <summary>
    /// 归一化路径中的斜杠或反斜杠为斜杠
    /// </summary>
    /// <param name="inputStr"></param>
    /// <returns></returns>
    public static string UniformSlash(this string inputStr)
    {
        return inputStr.Replace('\\', '/');
    }
}
