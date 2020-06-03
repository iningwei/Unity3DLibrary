using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Crypt
{
    #region 字符串加解密
    //http://www.jb51.net/article/58442.htm
    public static string TextEncrypt(string content, string secretKey)
    {
        char[] data = content.ToCharArray();
        char[] key = secretKey.ToCharArray();
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= key[i % key.Length];
        }
        return new string(data);
    }
    public static string TextDecrypt(char[] data, string secretKey)
    {
        char[] key = secretKey.ToCharArray();
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= key[i % key.Length];
        }
        return new string(data);
    }
    #endregion

    #region 二进制加解密
    public void FileEncrypt(string filePath, string key)
    {
        var data_bytes = File.ReadAllBytes(filePath);
        File.WriteAllBytes(filePath, BytesEncrypt(data_bytes, key));
    }

    public byte[] BytesEncrypt(byte[] data_bytes, string key)
    {
        if (key.Trim() == "")
        {
            Debug.LogError("error,key can not be empty!");
        }
        var key_bytes = System.Text.Encoding.UTF8.GetBytes(key);
        for (int i = 0; i < data_bytes.Length; i++)
        {
            data_bytes[i] = (byte)(data_bytes[i] ^ key_bytes[i % key_bytes.Length]);
        }
        return data_bytes;
    }

    public byte[] BytesDecrypt(byte[] data_bytes, string key)
    {
        if (key.Trim() == "")
        {
            Debug.LogError("error,key can not be empty!");

        }
        var key_bytes = System.Text.Encoding.UTF8.GetBytes(key);
        for (int i = 0; i < data_bytes.Length; i++)
        {
            data_bytes[i] = (byte)(data_bytes[i] ^ key_bytes[i % key_bytes.Length]);
        }

        return data_bytes;
    }
    #endregion
}
