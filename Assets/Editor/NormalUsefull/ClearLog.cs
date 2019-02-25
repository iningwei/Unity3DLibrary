using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ClearLog : MonoBehaviour
{
    /// <summary>
    /// ClearLog的具体方法可以用Reflector或者SLSpy查看Unity源码。不同版本有差异
    /// </summary>
    [MenuItem("工具/ClearLog")]
    public static void DoClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditorInternal.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
