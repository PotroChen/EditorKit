using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;

public static class AssetDatabaseExtensions
{
    /// <summary>
    /// Copy 序列化信息到 原本的资源内，不然引用会有问题(Unity引擎的问题)，
    /// 然后使用返回的新引用
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Object CreateOrReplaceAsset(Object asset, string path)
    {
        string assetPath = AssetDatabase.GetAssetPath(asset);
        //说明Asset是asset不是一个instance,如果asset存到新路径会报错，创建一个instance
        if (!string.IsNullOrEmpty(assetPath))
        {
            asset = UnityEngine.Object.Instantiate(asset);
            asset.name = asset.name.Replace("(Clone)","");
        }

        var serializedAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        if (serializedAsset != null)
        {
            if (asset.GetType() != serializedAsset.GetType())
                throw new System.Exception("can not replace between two different types");

            if (serializedAsset is GameObject go)
            {
                var prefabAssetType = PrefabUtility.GetPrefabAssetType(serializedAsset);
                if (prefabAssetType != PrefabAssetType.Regular)
                {
                    throw new System.Exception("不支持ModelPrefab(变体预制体还没测过)");
                }
                PrefabUtility.SaveAsPrefabAsset((GameObject)asset, path);
                return asset;
            }
            else
            {
                if (serializedAsset is Mesh m)
                    m.Clear(false);

                EditorUtility.CopySerialized(asset, serializedAsset);//Copy 序列化信息到 原本的资源内，不然引用会有问题(Unity引擎的问题)

                return serializedAsset;
            }
        }
        else
        {
            if (asset is GameObject go)
                PrefabUtility.SaveAsPrefabAsset(go, path);
            else
                AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }

    /*
     * Unity 2019.4.40f1:
     * 用该函数更改prefab后,调用Assetdatabase.Refresh时的UnityEditor.PrefabUtility.prefabInstanceUpdated中,不要加
     * 载对应的prefab，不然会使prefab文件损坏
     * 
     * 一般情况使用不到这个函数，如果想通过这种方式更改unity默认的引用的话，就此放弃吧。
     * Unity的引用是同时记录了fileId(prefab内部的localID)和guid,guid改了但是fileID找不到，引用还是会missing的
     */
    public static void SetAssetGUID(string path, string guid)
    {
        //fileFormatVersion: 2 是可以的
        int lastIndexOfAssets = Application.dataPath.LastIndexOf("Assets");
        string metaFilePath = Application.dataPath.Remove(lastIndexOfAssets) + path + ".meta";

        var lines = File.ReadAllLines(metaFilePath);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.StartsWith("guid: "))
            {
                lines[i] = string.Format("guid: {0}", guid);
                break;
            }
        }
        File.WriteAllLines(metaFilePath, lines);
    }


}
