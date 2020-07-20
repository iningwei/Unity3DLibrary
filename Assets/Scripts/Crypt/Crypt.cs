using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
    public static void FileEncrypt(string filePath, string key)
    {
        var data_bytes = File.ReadAllBytes(filePath);
        File.WriteAllBytes(filePath, BytesEncrypt(data_bytes, key));
    }

    public static byte[] BytesEncrypt(byte[] data_bytes, string key)
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

    public static byte[] BytesDecrypt(byte[] data_bytes, string key)
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

    #region DES加解密
    /// <summary>
    /// Des加密字符串，结果以16进制表示
    /// （不会像base64编码那样产生特殊字符= + /等）
    /// </summary>
    /// <param name="content"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string DesEncryptStrWithHex(string content, string key)
    {
        if (key.Length != 8)
        {
            Debug.LogError("DesEncryptStr error des need key has 8 char");
            return "";
        }
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] byteContent = Encoding.UTF8.GetBytes(content);

        des.Key = ASCIIEncoding.ASCII.GetBytes(key);
        des.IV = ASCIIEncoding.ASCII.GetBytes(key);


        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(byteContent, 0, byteContent.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }

        return ret.ToString();
    }

    public static string DesDecryptStrFromHex(string content, string skey)
    {
        if (skey.Length != 8)
        {
            Debug.LogError("DesDecryptStr error des need key has 8 char");
            return "";
        }
        try
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] byteContent = new byte[content.Length / 2];
            for (int x = 0; x < content.Length / 2; x++)
            {
                int ret = Convert.ToInt32(content.Substring(x * 2, 2), 16);
                byteContent[x] = (byte)ret;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(),
              CryptoStreamMode.Write);
            cs.Write(byteContent, 0, byteContent.Length);
            cs.FlushFinalBlock();
            string result = Encoding.Default.GetString(ms.ToArray());
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError("DesDecryptStrFromHex error, content:" + content + ", ex:" + ex.ToString());
            return null;
        }

    }



    /// <summary>
    /// Des加密字符串，结果以base64方式表示
    /// </summary>
    /// <param name="content"></param>
    /// <param name="key">秘钥必须为8位</param>
    /// <returns>返回的串是经过base64处理的</returns>
    public static string DesEncryptStrWithBase64(string content, string key)
    {
        if (key.Length != 8)
        {
            Debug.LogError("DesEncryptStr error des need key has 8 char");
            return "";
        }
 

        byte[] bKey = Encoding.UTF8.GetBytes(key);
        byte[] bIV = Encoding.UTF8.GetBytes(key);
        byte[] bStr = Encoding.UTF8.GetBytes(content);

        DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
        desc.Padding = PaddingMode.PKCS7;//补位
        desc.Mode = CipherMode.ECB;//CipherMode.CBC

        using (MemoryStream mStream = new MemoryStream())
        {
            using (CryptoStream cStream = new CryptoStream(mStream, desc.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
            {
                cStream.Write(bStr, 0, bStr.Length);
                cStream.FlushFinalBlock();
                byte[] res = mStream.ToArray();
                return Convert.ToBase64String(res);
            }
        }
    }



    /// <summary>
    /// 对base64格式的des加密后的字符串解密
    /// </summary>
    /// <param name="content">content为base64加密后的</param>
    /// <param name="skey">秘钥必须为8位</param>
    /// <returns></returns>
    public static string DesDecryptStrFromBase64(string content, string skey)
    {
        if (skey.Length != 8)
        {
            Debug.LogError("DesDecryptStr error des need key has 8 char");
            return "";
        }

        byte[] inputByteArray = Convert.FromBase64String(content);
        DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
        desc.Padding = PaddingMode.PKCS7;
        desc.Mode = CipherMode.ECB;

        desc.Key = Encoding.UTF8.GetBytes(skey);
        desc.IV = Encoding.UTF8.GetBytes(skey);

        MemoryStream ms = new MemoryStream();

        CryptoStream cs = new CryptoStream(ms, desc.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    #endregion
}
