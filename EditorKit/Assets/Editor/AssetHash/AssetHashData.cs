using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

/*
 * 用于记录AssetHash
 * 并且与上次记录做对比，并返回状态
 * 
 * 使用方法
 * AssetHashData hashData = new AssetHashData("模块名称");
 * hashData.ReadFromCache();
 * var assetState = hashData.GetAssetState(assetPath);
 * 
 * if(assetState != AssetState.Normal)
 *  操作资源;
 *  
 * Assetdatabase.SaveAssets();
 * hashData.RecordAssetHash(assetPathes);//记录此时资源状态
 */
namespace AssetHash
{
    public class AssetHashData
    {
        private const string CACHE_PATH_FORMAT = "Library/{0}Cache";
        private const string CACHE_VERSION = "V1";

        //资源资源信息字典
        private Dictionary<string, AssetDescription> assetDict = new Dictionary<string, AssetDescription>();
        private string cachePath;

        private AssetHashData() { }
        public AssetHashData(string cacheName)
        {
            cachePath = string.Format(CACHE_PATH_FORMAT, cacheName);
        }

        //记录Assets Hash
        public void RecordAssetHash(List<string> pathes)
        {
            ReadFromCache();

            int totalCount = pathes.Count;
            for (int i = 0; i < totalCount; i++)
            {
                if (File.Exists(pathes[i]))
                    ImportAsset(pathes[i]);
                if (i % 2000 == 0)
                    GC.Collect();
            }
            //将信息写入缓存
            WriteToCache();
        }

        public AssetState GetAssetState(string guid)
        {
            AssetDescription ad;

            if (assetDict.TryGetValue(guid, out ad))
            {
                if (File.Exists(ad.path))
                {
                    //修改时间与记录的不同为修改过的资源
                    if (ad.assetDependencyHash != AssetDatabase.GetAssetDependencyHash(ad.path).ToString())
                    {
                        return AssetState.CHANGED;
                    }
                    else
                    {
                        //默认为普通资源
                        return AssetState.NORMAL;
                    }
                }
                //不存在为丢失
                else
                {
                    return AssetState.MISSING;
                }
            }

            //字典中没有该数据
            else
            {
                return AssetState.NODATA;
            }
        }

        //生成并加入资源信息
        private void ImportAsset(string path)
        {
            if (!path.StartsWith("Assets/"))
                return;

            //通过path获取guid进行储存
            string guid = AssetDatabase.AssetPathToGUID(path);
            //获取该资源的Hash，用于之后的修改判断
            Hash128 assetDependencyHash = AssetDatabase.GetAssetDependencyHash(path);
            //如果assetDict没包含该guid或包含了修改时间不一样则需要更新
            if (!assetDict.ContainsKey(guid) || assetDict[guid].assetDependencyHash != assetDependencyHash.ToString())
            {
                //生成asset信息
                AssetDescription ad = new AssetDescription();
                ad.name = Path.GetFileNameWithoutExtension(path);
                ad.path = path;
                ad.assetDependencyHash = assetDependencyHash.ToString();

                if (assetDict.ContainsKey(guid))
                    assetDict[guid] = ad;
                else
                    assetDict.Add(guid, ad);
            }
        }
        

        //读取缓存信息
        public bool ReadFromCache()
        {
            assetDict.Clear();
            if (!File.Exists(cachePath))
            {
                return false;
            }

            var serializedGuid = new List<string>();
            var serializedDependencyHash = new List<string>();
            //反序列化数据
            FileStream fs = File.OpenRead(cachePath);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                string cacheVersion = (string)bf.Deserialize(fs);
                if (cacheVersion != CACHE_VERSION)
                {
                    return false;
                }

                EditorUtility.DisplayCancelableProgressBar("Import Cache", "Reading Cache", 0);
                serializedGuid = (List<string>)bf.Deserialize(fs);
                serializedDependencyHash = (List<string>)bf.Deserialize(fs);
                EditorUtility.ClearProgressBar();
            }
            catch
            {
                //兼容旧版本序列化格式
                return false;
            }
            finally
            {
                fs.Close();
            }

            for (int i = 0; i < serializedGuid.Count; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(serializedGuid[i]);
                if (!string.IsNullOrEmpty(path))
                {
                    var ad = new AssetDescription();
                    ad.name = Path.GetFileNameWithoutExtension(path);
                    ad.path = path;
                    ad.assetDependencyHash = serializedDependencyHash[i];
                    assetDict.Add(serializedGuid[i], ad);
                }
            }
            return true;
        }

        //写入缓存
        private void WriteToCache()
        {
            if (File.Exists(cachePath))
                File.Delete(cachePath);

            var serializedGuid = new List<string>();
            var serializedDependencyHash = new List<string>();
            //辅助映射字典
            var guidIndex = new Dictionary<string, int>();
            //序列化
            using (FileStream fs = File.OpenWrite(cachePath))
            {
                foreach (var pair in assetDict)
                {
                    guidIndex.Add(pair.Key, guidIndex.Count);
                    serializedGuid.Add(pair.Key);
                    serializedDependencyHash.Add(pair.Value.assetDependencyHash);
                }


                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, CACHE_VERSION);
                bf.Serialize(fs, serializedGuid);
                bf.Serialize(fs, serializedDependencyHash);
            }
        }


        ////更新引用信息状态
        //public void UpdateAssetState(string guid)
        //{
        //    AssetDescription ad;
        //    if (assetDict.TryGetValue(guid, out ad) && ad.state != AssetState.NODATA)
        //    {
        //        if (File.Exists(ad.path))
        //        {
        //            //修改时间与记录的不同为修改过的资源
        //            if (ad.assetDependencyHash != AssetDatabase.GetAssetDependencyHash(ad.path).ToString())
        //            {
        //                ad.state = AssetState.CHANGED;
        //            }
        //            else
        //            {
        //                //默认为普通资源
        //                ad.state = AssetState.NORMAL;
        //            }
        //        }
        //        //不存在为丢失
        //        else
        //        {
        //            ad.state = AssetState.MISSING;
        //        }
        //    }

        //    //字典中没有该数据
        //    else if (!assetDict.TryGetValue(guid, out ad))
        //    {
        //        string path = AssetDatabase.GUIDToAssetPath(guid);
        //        ad = new AssetDescription();
        //        ad.name = Path.GetFileNameWithoutExtension(path);
        //        ad.path = path;
        //        ad.state = AssetState.NODATA;
        //        assetDict.Add(guid, ad);
        //    }
        //}

        public class AssetDescription
        {
            public string name = "";
            public string path = "";
            public string assetDependencyHash;
            public AssetState state = AssetState.NORMAL;
        }

        public enum AssetState
        {
            NORMAL,
            CHANGED,
            MISSING,
            NODATA,
        }
    }

}