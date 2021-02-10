using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameLanch : UnitySingleton<GameLanch>
{
    private string url = "https://hotfix-1258327636.cos.ap-guangzhou.myqcloud.com/dll/";

    public bool IsEditorMode;

    public override void Awake()
    {
        base.Awake();

        //Init初始化游戏框架 ：资源框架，网络管理，日志管理....
        this.gameObject.AddComponent<ResourceManagr>();
        this.gameObject.AddComponent<ILRunTimeManager>();
        this.gameObject.AddComponent<OnlineConfig>();
        //EndInit-FrameWork

        //InitLoadingUI
        InitLoadingUI();

        this.CheckOnlineConfig();
    }

    private void CheckOnlineConfig()
    {
        OnlineConfig.Instance.GetOnlineConfig(() => { this.StartCoroutine(this.CheckHotUpdate()); });
    }

    IEnumerator CheckHotUpdate()
    {
        UnityWebRequest unityWebRequest;
        byte[] dll;
        byte[] pdb;
        
        if (!Checkdll(OnlineConfig.codeVersion))
        {
            Debug.LogError("需要更新:" + OnlineConfig.codeVersion);
            //------------------------------------------------热更新下载DLL--------------------------------------------------//
            unityWebRequest = new UnityWebRequest(url + OnlineConfig.codeVersion, UnityWebRequest.kHttpVerbGET);
            string path = (Application.streamingAssetsPath + "/Hotfix/"+ OnlineConfig.codeVersion);
            unityWebRequest.downloadHandler = new DownloadHandlerFile(path);

            yield return unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(unityWebRequest.error))
            {
                UnityEngine.Debug.LogError(unityWebRequest.error);
                Debug.Log("File successfully downloaded and saved to " + path);
                yield return null;
            }
#if UNITY_ANDROID
            unityWebRequest = new UnityWebRequest(Application.streamingAssetsPath + "/Hotfix/" + OnlineConfig.codeVersion);
#else
            unityWebRequest = new UnityWebRequest("file:///" + Application.streamingAssetsPath + "/Hotfix/" + OnlineConfig.codeVersion);
#endif
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            
            yield return unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
            {
                //LoadingUI.SetProgressBar(unityWebRequest.downloadProgress); //=》DOWNLOAD PROGRSS
                yield return null;
            }
            //LoadingUI.SetProgressBar(unityWebRequest.downloadProgress); //=》DOWNLOAD PROGRSS
            if (!string.IsNullOrEmpty(unityWebRequest.error))
                UnityEngine.Debug.LogError(unityWebRequest.error);

            dll = unityWebRequest.downloadHandler.data;
            unityWebRequest.Dispose();
            //-----------------------------------------------------------------------------------------------------------------------//
            //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
#if UNITY_ANDROID
                unityWebRequest = new UnityWebRequest(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
            //unityWebRequest = new UnityWebRequest(url + "HotFix_Project.pdb");
            unityWebRequest = new UnityWebRequest("file:///" + Application.streamingAssetsPath + "/Hotfix/HotFix_Project.pdb");
#endif
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(unityWebRequest.error))
                UnityEngine.Debug.LogError(unityWebRequest.error);
            pdb = unityWebRequest.downloadHandler.data;

            unityWebRequest.Dispose();
        }
        else
        {
            Debug.LogError("代码版本正确，不用更新:" + OnlineConfig.codeVersion);
            //----------------------------------------------------加载DLL-------------------------------------------------------------------//
#if UNITY_ANDROID
            unityWebRequest = new UnityWebRequest(Application.streamingAssetsPath + "/Hotfix/" + OnlineConfig.codeVersion);
#else
            unityWebRequest = new UnityWebRequest("file:///" + Application.streamingAssetsPath + "/Hotfix/" + OnlineConfig.codeVersion);
#endif
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
            {
                //LoadingUI.SetProgressBar(unityWebRequest.downloadProgress); //=》DOWNLOAD PROGRSS
                yield return null;
            }
            //LoadingUI.SetProgressBar(unityWebRequest.downloadProgress); //=》DOWNLOAD PROGRSS
            if (!string.IsNullOrEmpty(unityWebRequest.error))
                UnityEngine.Debug.LogError(unityWebRequest.error);
            dll = unityWebRequest.downloadHandler.data;
            unityWebRequest.Dispose();

            //----------------------------------------------------加载PDB-------------------------------------------------------------------//
            //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
#if UNITY_ANDROID
                unityWebRequest = new UnityWebRequest(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
            //unityWebRequest = new UnityWebRequest(url + "HotFix_Project.pdb");
            unityWebRequest = new UnityWebRequest("file:///" + Application.streamingAssetsPath + "/Hotfix/HotFix_Project.pdb");
#endif
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
                yield return null;
            if (!string.IsNullOrEmpty(unityWebRequest.error))
                UnityEngine.Debug.LogError(unityWebRequest.error);
            pdb = unityWebRequest.downloadHandler.data;

            unityWebRequest.Dispose();
        }
        //----------------------------------------------------加载Game------------------------------------------------------------------//
        //ILRunTimeManager.Instance.LoadHotFixAssembly(dll, null);

        //ILRunTimeManager.Instance.EnterGame();
        //----------------------------------------------------End-----------------------------------------------------------------------//

        StartCoroutine(AssetBundleManager.Instance.DownLoadMainAssetBundel(() => {this.LoadGame(dll);}));

        yield break;
    }

    private void LoadGame(byte[] dll)
    {
        //----------------------------------------------------加载Game------------------------------------------------------------------//
        ILRunTimeManager.Instance.LoadHotFixAssembly(dll, null);

        ILRunTimeManager.Instance.EnterGame();
        //----------------------------------------------------End-----------------------------------------------------------------------//
    }

    private bool Checkdll(string dll)
    {
        var path = Application.streamingAssetsPath;
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                //Debug.Log("Name : " + files[i].Name);
                //Debug.Log("FullName : " + files[i].FullName);
                //Debug.Log("DirectoryName : " + files[i].DirectoryName);
                if (dll == files[i].Name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void InitLoadingUI()
    {
        GameObject Loading = ResourceManagr.Instance.GetResourcesAsset<GameObject>("UI/LoadingUI");
        GameObject uiView = GameObject.Instantiate(Loading);
        uiView.transform.SetParent(GameObject.Find("UICamera/Canvas").GetComponent<Canvas>().transform, false);
        uiView.name = Loading.name;
    }
}
