using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crypt
{
    #region 字符串加解密
    //http://www.jb51.net/article/58442.htm
    static string TextEncrypt(string content, string secretKey)
    {
        char[] data = content.ToCharArray();
        char[] key = secretKey.ToCharArray();
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= key[i % key.Length];
        }
        return new string(data);
    }
    private string TextDecrypt(char[] data, string secretKey)
    {
        char[] key = secretKey.ToCharArray();
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= key[i % key.Length];
        }
        return new string(data);
    }
    #endregion
}
