using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : UnitySingleton<AssetBundleManager>
{
    private string url = "https://hotfix-1258327636.cos.ap-guangzhou.myqcloud.com/ab/";

    public override void Awake()
    {
        base.Awake();
//#if UNITY_EDITOR
//        IsEditorMode = true;
//#else
//        IsEditorMode = false;
//#endif
    }

    public T GetAssetCache<T>(string name) where T : UnityEngine.Object
    {
        //Debug.Log("LOAD BY ASSETSBUNDLE MANAGER");

#if UNITY_ANDROID
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle((Application.streamingAssetsPath + "/AssetsBundle/" + OnlineConfig.codeVersion);
#else
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle("file:///" + Application.streamingAssetsPath + "/AssetsBundle/" + OnlineConfig.assetBundleVersion);
#endif
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SendWebRequest();

        while (!request.isDone)
        {
            
        }

        byte[] bytes = request.downloadHandler.data;

        //AssetBundle ab = AssetBundle.LoadFromMemory(bytes);   //官方不建议这个方法，性能比较差

        FileStream stream = new FileStream(Application.streamingAssetsPath + "/AssetsBundle/ui", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        stream.Write(bytes, 0, bytes.Length);
        AssetBundle ab = AssetBundle.LoadFromStream(stream);

        GameObject obj = ab.LoadAsset<GameObject>(name);

        UnityEngine.Object target = ab.LoadAsset<T>(name);

        return target as T;
    }

    public IEnumerator DownLoadMainAssetBundel(Action callback = null)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url + OnlineConfig.assetBundleVersion + "/AssetsBundle");
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

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url + OnlineConfig.assetBundleVersion + "/");
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
