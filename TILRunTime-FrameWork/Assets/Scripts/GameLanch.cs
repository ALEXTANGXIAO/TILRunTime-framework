﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameLanch : UnitySingleton<GameLanch>
{
    public const string url = "https://hotfix-1258327636.cos.ap-guangzhou.myqcloud.com/1/";

    public string dllversion = "/1/";

    public override void Awake()
    {
        base.Awake();

        //Init初始化游戏框架 ：资源框架，网络管理，日志管理....
        this.gameObject.AddComponent<ResMgr>();
        this.gameObject.AddComponent<ILRunTimeManager>();
        //EndInit

        this.StartCoroutine(this.CheckHotUpdate());
    }

    IEnumerator CheckHotUpdate()
    {
        //1.检查版本--》

        //2.下载万场后从下载路径下下载

        //3.若下载路径没有，从streamingAssets下载

        //dll-》二进制从ab包里一起加载

        //UnityWebRequest <= WWW
        //UnityWebRequest unityWebRequest = new UnityWebRequest(url + "HotFix_Project.dll");
        UnityWebRequest unityWebRequest = new UnityWebRequest("file:///" + Application.streamingAssetsPath + "/Hotfix/HotFix_Project.dll");
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return unityWebRequest.SendWebRequest();
        while (!unityWebRequest.isDone)
            yield return null;
        if(!string.IsNullOrEmpty(unityWebRequest.error))
            UnityEngine.Debug.LogError(unityWebRequest.error);
        byte[] a = unityWebRequest.downloadHandler.data;
        unityWebRequest.Dispose();

#if UNITY_ANDROID
        WWW www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
        WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/Hotfix/HotFix_Project.dll");
        //WWW www = new WWW(url + "HotFix_Project.dll");
#endif
        while (!www.isDone)
            yield return null;
        if (!string.IsNullOrEmpty(www.error))
            UnityEngine.Debug.LogError(www.error);
        byte[] dll = www.bytes;
        www.Dispose();

        //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
#if UNITY_ANDROID
        www = new WWW(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
        www = new WWW("file:///" + Application.streamingAssetsPath + "/Hotfix/HotFix_Project.pdb");
        //www = new WWW(url + "HotFix_Project.pdb");
#endif
        while (!www.isDone)
            yield return null;
        if (!string.IsNullOrEmpty(www.error))
            UnityEngine.Debug.LogError(www.error);
        byte[] pdb = www.bytes;

        www.Dispose();

        ILRunTimeManager.Instance.LoadHotFixAssembly(dll,pdb);

        ILRunTimeManager.Instance.EnterGame();

        yield break;
    }

    private void Start()
    {
        
    }

    void Update()
    {
        
    }
}