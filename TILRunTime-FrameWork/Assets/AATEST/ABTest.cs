using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ABTest:MonoBehaviour
{
    private void Start()
    {
        //依赖包的关键知识点，利用主包获取依赖信息
        //加载主包
        AssetBundle abMain = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/");
        //加载主包的固定文件
        AssetBundleManifest abManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //从固定文件中得到依赖信息
        string[] strs = abManifest.GetAllDependencies("abpackname");
        //得到了依赖包的名字
        for(int i = 0; i < strs.Length; i++)
        {
            Debug.Log(strs[i]);
            AssetBundle a = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" +strs[i]);
        }

        // 第一步 加载AB包
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "ui");
        // 第二步 加载AB包中的资源 (同步加载)
        ab.LoadAsset<GameObject>("Loading");

        // 异步加载 -->协程   （异步加载）
        StartCoroutine(LoadABRes("ui","name"));


        //AB包不能重复加载，会出错 //👇写作所有下载的AB包
        AssetBundle.UnloadAllAssetBundles(false);   //ture会把场景已加载的的资源也卸载，false不会
    }

    IEnumerator LoadABRes(string ABName,string resName)
    {
        //第一步加载AB包
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + "ui");
        yield return abcr;
        //第二步 加载资源
        AssetBundleRequest abq = abcr.assetBundle.LoadAssetAsync<GameObject>("Loading");
        yield return abq;

    }
}
