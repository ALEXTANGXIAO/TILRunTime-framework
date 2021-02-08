﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle : MonoBehaviour
{
    [MenuItem("AssetBundle/Build Windows")]
    static void Build_WindowsAB()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("AssetBundle/Build Android")]
    static void Build_AndroidAB()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("AssetBundle/Build IOS")]
    static void Build_IOSAB()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    [MenuItem("AssetBundle/Build Switch")]
    static void Build_SwitchAB()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetsBundle", BuildAssetBundleOptions.None, BuildTarget.Switch);
    }
}
