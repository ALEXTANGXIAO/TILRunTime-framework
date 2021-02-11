using System.Collections;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using System;

public class OnlineConfig :UnitySingleton<OnlineConfig>
{
    public static string onlineParamUrl = "https://hotfix-1258327636.cos.ap-guangzhou.myqcloud.com/OnlineParam.json";

    public static string OnlineParamString;

    public static string codeVersion;

    public static string assetBundleVersion;

    void Start()
    {
        //StartCoroutine(_GetOnlineConfig());
    }

    public void GetOnlineConfig(Action callback)
    {
        StartCoroutine(_GetOnlineConfig(callback));
    }

    IEnumerator _GetOnlineConfig(Action callback)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(onlineParamUrl);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
            OnlineParamString = unityWebRequest.downloadHandler.text;
            JsonData jsonData = JsonMapper.ToObject(OnlineParamString);
            if (jsonData != null)
            {
                JsonData CodeVersion = jsonData["CodeVersion"];
                JsonData AssetBundleVersion = jsonData["AssetBundleVersion"];

                codeVersion = CodeVersion.ToString();
                assetBundleVersion = AssetBundleVersion.ToString();
            }
        }

        if (callback != null)
        {
            callback();
        }
    }
}
