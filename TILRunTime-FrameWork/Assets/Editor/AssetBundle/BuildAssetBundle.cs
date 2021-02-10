using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle : MonoBehaviour
{
    [MenuItem("AssetBundle/Build Windows")]
    static void Build_WindowsAB()
    {
        string dir = Application.streamingAssetsPath + "/AssetsBundle";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("AssetBundle/Build Android")]
    static void Build_AndroidAB()
    {
        string dir = Application.streamingAssetsPath + "/AssetsBundle";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("AssetBundle/Build IOS")]
    static void Build_IOSAB()
    {
        string dir = Application.streamingAssetsPath + "/AssetsBundle";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    [MenuItem("AssetBundle/Build Switch")]
    static void Build_SwitchAB()
    {
        string dir = Application.streamingAssetsPath + "/AssetsBundle";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.Switch);
    }
}
