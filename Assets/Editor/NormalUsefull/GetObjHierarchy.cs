using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;

//come from
//http://answers.unity3d.com/questions/266244/how-can-i-add-copypaste-clipboard-support-to-my-ga.html

public class ClipboardHelper
{
    private static PropertyInfo m_systemCopyBufferProperty = null;
    private static PropertyInfo GetSystemCopyBufferProperty()
    {
        if (m_systemCopyBufferProperty == null)
        {
            Type T = typeof(GUIUtility);
            m_systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
            if (m_systemCopyBufferProperty == null)
                throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
        }
        return m_systemCopyBufferProperty;
    }
    public static string clipBoard
    {
        get
        {
            PropertyInfo P = GetSystemCopyBufferProperty();
            return (string)P.GetValue(null, null);
        }
        set
        {
            PropertyInfo P = GetSystemCopyBufferProperty();
            P.SetValue(null, value, null);
        }
    }
}


/// <summary>
/// 获得Hierarchy面板内选中物体的层级关系。并保存到剪贴板
/// </summary>
public class GetObjHierarchy
{
    [MenuItem("My/GetObjHierarchy/追溯1个父物体")]
    static void GetObjHierarchy1()//往上追溯一个父物体
    {
        getHierarchyPath(1);
    }


    [MenuItem("My/GetObjHierarchy/追溯2个父物体")]
    static void GetObjHierarchy2()
    {
        getHierarchyPath(2);
    }


    [MenuItem("My/GetObjHierarchy/追溯3个父物体")]
    static void GetObjHierarchy3()
    {
        getHierarchyPath(3);
    }

    [MenuItem("My/GetObjHierarchy/追溯4个父物体")]
    static void GetObjHierarchy4()
    {
        getHierarchyPath(4);
    }

    [MenuItem("My/GetObjHierarchy/追溯5个父物体")]
    static void GetObjHierarchy5()
    {
        getHierarchyPath(5);
    }

    [MenuItem("My/GetObjHierarchy/追溯6个父物体")]
    static void GetObjHierarchy6()
    {
        getHierarchyPath(6);
    }
    [MenuItem("My/GetObjHierarchy/追溯7个父物体")]
    static void GetObjHierarchy7()
    {
        getHierarchyPath(7);
    }
    [MenuItem("My/GetObjHierarchy/追溯8个父物体")]
    static void GetObjHierarchy8()
    {
        getHierarchyPath(8);
    }
    [MenuItem("Custom/GetObjHierarchy/追溯9个父物体")]
    static void GetObjHierarchy9()
    {
        getHierarchyPath(9);
    }
    [MenuItem("My/GetObjHierarchy/追溯10个父物体")]
    static void GetObjHierarchy10()
    {
        getHierarchyPath(10);
    }
    [MenuItem("My/GetObjHierarchy/追溯11个父物体")]
    static void GetObjHierarchy11()
    {
        getHierarchyPath(11);
    }


    static void getHierarchyPath(int parentCount)
    {
        GameObject selectObj = Selection.activeGameObject;

        if (selectObj == null)
        {
            Debug.LogError("选择一个物体");
            return;
        }
        string path = selectObj.name;
        for (int i = 0; i < parentCount; i++)
        {
            var parent = selectObj.transform.parent;
           
            if (parent == null)
            {
                Debug.LogError("父物体数量不匹配");
                return;
            }
            else
            {
                selectObj = parent.gameObject;
                path =selectObj.name + "/" + path;
            }
        }

        Debug.Log("path is:" + path);

        ClipboardHelper.clipBoard = path;
    }


}
