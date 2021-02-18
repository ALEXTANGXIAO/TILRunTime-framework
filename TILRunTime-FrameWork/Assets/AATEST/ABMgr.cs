using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 知识点
/// 1.AB包相关的API
/// 2.单例模式
/// 3.委托-->Lambda表达式
/// 4.协程
/// 5.字典
/// </summary>
class ABMgr:UnitySingleton<ABMgr>
{
    private void Start()
    {
        var obj = LoadRes("ui","LoadingUI");
        Instantiate(obj);

        LoadResAsync<GameObject>("ui", "LoadingUI",(obj2) => { obj2.transform.position = -Vector3.up; });
    }

    //AB包MGR目的为了让外部更方便进行资源加载

    //主包
    private AssetBundle mainAB = null;
    //依赖包获取用的配置文件
    private AssetBundleManifest manifest = null;

    //字典，数据容器，用字典来存储加载过的AB包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// AB包存放路径
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/AssetsBundle/";
        }
    }

    private string MainABName
    {
        get
        {
#if UNITY_IOS
        return "AssetBundle"
#else
            return "AssetsBundle";
#endif
        }
    }

    public void LoadAB(string abName)
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //我们获取依赖包相关信息
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载资源来源包
        //如果没有加载过，再加载
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //同步加载 不指定类型 
    public UnityEngine.Object LoadRes(string abName,string resName)
    {
        //加载AB包
        LoadAB(abName);

        //直接实例化的接口
        //Object obj = abDic[abName].LoadAsset(resName);
        //if (obj is GameObject)
        //    return Instantiate(obj);
        //else
        //    return obj;

        //加载资源
        return abDic[abName].LoadAsset(resName);
    }

    //同步加载 根据type指定类型   //用途，Lua不支持泛型，传类型
    public UnityEngine.Object LoadRes(string abName, string resName,System.Type type)
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        return abDic[abName].LoadAsset(resName,type);
    }


    //同步加载 根据泛型指定类型   //对于C#最方便的泛型
    public T LoadRes<T>(string abName, string resName) where T:UnityEngine.Object
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        return abDic[abName].LoadAsset<T>(resName);
    }

    //异步加载
    //这里的异步加载，AB包并没有使用异步加载
    //从AB包中加载资源时 使用异步

    //异步加载 根据名字异步加载
    public void LoadResAsync(string abName,string resName,UnityAction<UnityEngine.Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync(string abName,string resName,UnityAction<UnityEngine.Object> callback)
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest abr =  abDic[abName].LoadAssetAsync(resName);

        yield return abr;

        //实例化   //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callback(Instantiate(abr.asset));
        else
            callback(abr.asset);

    }


    //异步加载 根据Type异步加载
    public void LoadResAsync(string abName, string resName,System.Type type, UnityAction<UnityEngine.Object> callback)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName,type, callback));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<UnityEngine.Object> callback)
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName,type);

        yield return abr;

        //实例化   //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callback(Instantiate(abr.asset));
        else
            callback(abr.asset);

    }

    //异步加载 根据泛型异步加载
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback = null)where T:UnityEngine.Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);

        yield return abr;

        //实例化   //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callback(Instantiate(abr.asset) as T);
        else
            callback(abr.asset as T);
    }






    //单个包卸载
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }
    //所以包卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
