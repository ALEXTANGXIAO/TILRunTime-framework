using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetBundleManager : UnitySingleton<AssetBundleManager>
{
    /// <summary>
    /// AB主包
    /// </summary>
    private AssetBundle mainAB = null;

    /// <summary>
    /// 依赖包获取用的配置文件
    /// </summary>
    private AssetBundleManifest manifest = null;

    /// <summary>
    /// 数据容器，用字典来存储AB包
    /// </summary>
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// AB包存放路径
    /// </summary>
    private string PathUrl
    {
        get
        {
#if UNITY_ANDROID
            return "file:///" + Application.streamingAssetsPath + "/AssetsBundle/");
#else
            return Application.streamingAssetsPath + "/AssetsBundle/";
#endif
        }
    }

    /// <summary>
    /// AB主包Name
    /// </summary>
    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "AssetsBundle"
#else
            return "AssetsBundle";
#endif
        }
    }

    public void LoadAB(string abName)
    {
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        
        AssetBundle ab;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //--------------------------------------调用接口------------------------------------------//
    /// <summary>
    /// 同步加载 不指定类型 
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object GetAssetCache(string abName, string resName)
    {
        LoadAB(abName);
        return abDic[abName].LoadAsset(resName);
    }

    /// <summary>
    /// 同步加载 根据type指定类型   BECAUSE Lua不支持泛型，传类型
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public UnityEngine.Object GetAssetCache(string abName, string resName, System.Type type)
    {
        LoadAB(abName);
        return abDic[abName].LoadAsset(resName, type);
    }

    /// <summary>
    /// 同步加载 根据泛型指定类型   //对于C#最方便的泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public T GetAssetCache<T>(string abName, string resName) where T : UnityEngine.Object
    {
        LoadAB(abName);
        return abDic[abName].LoadAsset<T>(resName);
    }

    /// <summary>
    /// 异步加载 根据名字异步加载
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    public void GetAssetCacheAsync(string abName, string resName, UnityAction<UnityEngine.Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<UnityEngine.Object> callback)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        //if (abr.asset is GameObject)
        //    callback(Instantiate(abr.asset));
        //else
            callback(abr.asset);
    }

    /// <summary>
    /// 异步加载 根据Type异步加载
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public void GetAssetCacheAsync(string abName, string resName, System.Type type, UnityAction<UnityEngine.Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callback));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<UnityEngine.Object> callback)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        //if (abr.asset is GameObject)
        //    callback(Instantiate(abr.asset));
        //else
            callback(abr.asset);

    }

    /// <summary>
    /// 异步加载 根据泛型异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    public void GetAssetCacheAsync<T>(string abName, string resName, UnityAction<T> callback = null) where T : UnityEngine.Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        //if (abr.asset is GameObject)
        //    callback(Instantiate(abr.asset) as T);
        //else
            callback(abr.asset as T);
    }

    /// <summary>
    /// 单个AB卸载
    /// </summary>
    /// <param name="abName"></param>
    public void UnLoadAB(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }
    
    /// <summary>
    /// 所有AB卸载
    /// </summary>
    public void UnLoadALLAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
    //---------------------------------------------------------------------------------------//

    /// <summary>
    /// AB在线地址
    /// </summary>
    private string OnlinePathUrl
    {
        get
        {
            return "https://hotfix-1258327636.cos.ap-guangzhou.myqcloud.com/ab/";
        }
    } 

    public override void Awake()
    {
        base.Awake();
//#if UNITY_EDITOR
//        IsEditorMode = true;
//#else
//        IsEditorMode = false;
//#endif
    }

    public T GetAssetCache<T>(string resName) where T : UnityEngine.Object
    {
        Debug.Log("LOAD BY ASSETSBUNDLE MANAGER" + resName);
        var abName = "ui";
        
        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
        
        UnityEngine.Object target = ab.LoadAsset<T>(resName);

        return target as T;
    }

    public IEnumerator DownLoadMainAssetBundel(Action callback = null)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(OnlinePathUrl + OnlineConfig.assetBundleVersion + "/AssetsBundle");
        string path = (Application.streamingAssetsPath + "/AssetsBundle/" + "AssetsBundle");
        request.downloadHandler = new DownloadHandlerFile(path);

        yield return request.SendWebRequest();

        while (request.isHttpError)
        {
            Debug.LogError("ERROR:" + request.error);
            yield return null;
        }
        while (!request.isDone)
        {
            LoadingUI.SetProgressBar(request.downloadProgress); //=》DOWNLOAD PROGRSS
            yield return null;
        }
        LoadingUI.SetProgressBar(request.downloadProgress);     //=》DOWNLOAD PROGRSS

        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);

        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] names = manifest.GetAllAssetBundles();
        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log(Application.streamingAssetsPath + "/AssetsBundle/" + names[i]);
            StartCoroutine(DownLoadAssetBundle(names[i], callback));
        }
    }

    public IEnumerator DownLoadAssetBundle(string bundleName,Action callback = null)
    {

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(OnlinePathUrl + OnlineConfig.assetBundleVersion + "/");
        string path = (Application.streamingAssetsPath + "/AssetsBundle/" + bundleName);
        request.downloadHandler = new DownloadHandlerFile(path);

        yield return request.SendWebRequest();

        while (request.isHttpError)
        {
            Debug.LogError("ERROR:" + request.error);
            yield return null;
        }
        while (!request.isDone)
        {
            LoadingUI.SetProgressBar(request.downloadProgress); //=》DOWNLOAD PROGRSS
            yield return null;
        }
        LoadingUI.SetProgressBar(request.downloadProgress);     //=》DOWNLOAD PROGRSS
        byte[] bytes = request.downloadHandler.data;

        if (callback != null)
        {
            callback();
        }
    }
}
