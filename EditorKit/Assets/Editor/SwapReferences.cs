using System.IO;
using UnityEditor;
using UnityEngine;

public class SwapReferences : EditorWindow
{
    private Object firstObject;
    private Object secondObject;

    [MenuItem("EditorKit/通用工具/互换引用", priority = 1)]
    static void Init()
    {
        SwapReferences window = (SwapReferences)EditorWindow.GetWindow(typeof(SwapReferences));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("选择两个资源互换它们的引用", EditorStyles.boldLabel);

        firstObject = EditorGUILayout.ObjectField("资源1", firstObject, typeof(Object), false);
        secondObject = EditorGUILayout.ObjectField("资源2", secondObject, typeof(Object), false);

        if (GUILayout.Button("互换引用"))
        {
            if (firstObject == null || secondObject == null)
                return;

            if (firstObject.GetType() != secondObject.GetType())
                return;

            if (firstObject is GameObject)
                SwapGUIDsForObjects_GameObject(firstObject, secondObject);
            else
                SwapGUIDsForObjects_Asset(firstObject, secondObject);
        }
    }

    /*
     * 只支持普通预制体不支持Model预制体
     */
    static void SwapGUIDsForObjects_GameObject(Object firstObject, Object secondObject)
    {
        string tempPath = "Assets/TempObj_SwapGuid.asset";

        string firstPath = AssetDatabase.GetAssetPath(firstObject);

        string secondPath = AssetDatabase.GetAssetPath(secondObject);

        Object tempObjA = Object.Instantiate(firstObject);
        Object tempObjB = Object.Instantiate(secondObject);

        //将B的数据->A(meta不变)
        //将A的数据->B(meta不变)
        PrefabUtility.SaveAsPrefabAsset((GameObject)tempObjB, firstPath);
        PrefabUtility.SaveAsPrefabAsset((GameObject)tempObjA, secondPath);

        DestroyImmediate(tempObjA);
        DestroyImmediate(tempObjB);

        //Move A To B
        ////Move B To A
        AssetDatabase.MoveAsset(firstPath, tempPath);
        AssetDatabase.MoveAsset(secondPath, firstPath);
        AssetDatabase.MoveAsset(tempPath, secondPath);
        //最终结果:A的数据变成了B，路径也是B，只有meta还是原来的，同理B也是。
        //以这种方式互换meta
        AssetDatabase.Refresh();

    }

    static void SwapGUIDsForObjects_Asset(Object firstObject, Object secondObject)
    {
        string tempPath = "Assets/TempObj_SwapGuid.asset";
        string firstPath = AssetDatabase.GetAssetPath(firstObject);
        string secondPath = AssetDatabase.GetAssetPath(secondObject);

        #region Swap Serialized Data
        string firstName = firstObject.name;
        string secondName = secondObject.name;
        Object tempObject = Object.Instantiate(firstObject);
        if (tempObject is Mesh tempMesh)//Mesh比较特殊，Copy之前要先Clear掉(不然,顶点位置不会更改)
            tempMesh.Clear();

        EditorUtility.CopySerialized(firstObject, tempObject);

        if (firstObject is Mesh firstMesh)
            firstMesh.Clear();

        EditorUtility.CopySerialized(secondObject, firstObject);

        if (secondObject is Mesh secondMesh)
            secondMesh.Clear();

        EditorUtility.CopySerialized(tempObject, secondObject);

        DestroyImmediate(tempObject, true);

        //恢复名称
        firstObject.name = firstName;
        secondObject.name = secondName;
        #endregion

        AssetDatabase.MoveAsset(firstPath, tempPath);
        AssetDatabase.MoveAsset(secondPath, firstPath);
        AssetDatabase.MoveAsset(tempPath, secondPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}