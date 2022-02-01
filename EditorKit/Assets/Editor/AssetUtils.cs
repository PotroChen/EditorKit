using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public static class AssetUtils
{
    //[MenuItem("Assets/EditorKit", priority = 0)]
    [MenuItem("Assets/EditorKit/CopyPath")]
    static void CopyAssetPath()
    {
        string path = "";
        if (Selection.assetGUIDs != null && Selection.assetGUIDs.Length == 1)
        {
            path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        }
        GUIUtility.systemCopyBuffer = path;
    }

    [MenuItem("Assets/EditorKit/CopyGUID")]
    static void CopyAssetGUID()
    {
        string guid = "";
        if (Selection.assetGUIDs != null && Selection.assetGUIDs.Length == 1)
        {
            guid = Selection.assetGUIDs[0];
        }
        GUIUtility.systemCopyBuffer = guid;
    }
}
