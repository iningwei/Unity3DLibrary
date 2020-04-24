using UnityEngine;
using System.Collections;
using UnityEditor;

public class OpenPesistantPath : ScriptableObject
{
    [MenuItem("Custom/Open P Path")]
    static void OpenPersistantPath()
    {
        string path = Application.persistentDataPath;
        System.Diagnostics.Process.Start(path);
    }
}
