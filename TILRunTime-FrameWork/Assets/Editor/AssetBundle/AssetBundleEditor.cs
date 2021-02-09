using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditor : EditorWindow
{
    //[MenuItem("AssetBundle/Editor %#O")]
    private static void OpenAssetBundleWindow()
    {
        AssetBundleEditor ABEditor = GetWindow<AssetBundleEditor>("AssetBundles");
        ABEditor.Show();
    }

    private void OnGUI()
    {
        TitleGUI();
        AssetBundlesGUI();
        CurrentAssetBundlesGUI();
        AssetsGUI();
    }

    //标记，用于标记当前选中的AB包索引
    private int _currentAB = -1;
    //一种系统样式，使用它就可以使按钮展示为矩形黑框样式
    private GUIStyle _preButton = new GUIStyle("PreButton");
    //一种系统样式，使用它就可以使下拉框展示为矩形黑框样式
    private GUIStyle _preDropDown = new GUIStyle("PreDropDown");
    //是否隐藏无效资源
    private bool _hideInvalidAsset = false;
    //是否隐藏已绑定资源
    private bool _hideBundleAsset = false;
    //打包路径
    private string _buildPath = "";
    //打包平台
    private BuildTarget _buildTarget = BuildTarget.StandaloneWindows;

    private void TitleGUI()
    {
        if (GUI.Button(new Rect(5, 5, 60, 15), "Create", _preButton))
        {
            Debug.LogError("还没有实现哦");
        }
        //当前未选中任一AB包的话，禁用之后的所有UI控件
        GUI.enabled = _currentAB == -1 ? false : true;
        if (GUI.Button(new Rect(65, 5, 60, 15), "Rename", _preButton))
        {
        }
        if (GUI.Button(new Rect(125, 5, 60, 15), "Clear", _preButton))
        {
        }
        if (GUI.Button(new Rect(185, 5, 60, 15), "Delete", _preButton))
        {
        }
        if (GUI.Button(new Rect(250, 5, 100, 15), "Add Assets", _preButton))
        {
        }
        //取消UI控件的禁用
        GUI.enabled = true;

        _hideInvalidAsset = GUI.Toggle(new Rect(360, 5, 100, 15), _hideInvalidAsset, "Hide Invalid");
        _hideBundleAsset = GUI.Toggle(new Rect(460, 5, 100, 15), _hideBundleAsset, "Hide Bundled");

        if (GUI.Button(new Rect(250, 25, 60, 15), "Open", _preButton))
        {
        }
        if (GUI.Button(new Rect(310, 25, 60, 15), "Browse", _preButton))
        {
        }

        GUI.Label(new Rect(370, 25, 70, 15), "Build Path:");
        _buildPath = GUI.TextField(new Rect(440, 25, 300, 15), _buildPath);

        BuildTarget buildTarget = (BuildTarget)EditorGUI.EnumPopup(new Rect((int)position.width - 205, 5, 150, 15), _buildTarget, _preDropDown);

        if (GUI.Button(new Rect((int)position.width - 55, 5, 50, 15), "Build", _preButton))
        {
        }
    }

    //区域视图的范围
    private Rect _ABViewRect;
    //区域视图滚动的范围
    private Rect _ABScrollRect;
    //区域视图滚动的位置
    private Vector2 _ABScroll;
    //区域高度标记，这里不用管它，是后续用来控制视图滚动量的
    private int _ABViewHeight = 0;
    //一种系统样式，使用他可以使控件周围表现为一个BOX的模样
    private GUIStyle _box = new GUIStyle("Box");

    private void AssetBundlesGUI()
    {
        //区域的视图范围：左上角位置固定，宽度固定（240），高度为窗口高度的一半再减去标题栏高度（20），标题栏高度为什么是20？看一下标题栏的控件高度就行了呗，多余的是空隙之类的
        _ABViewRect = new Rect(5, 25, 240, (int)position.height / 2 - 20);
        //滚动的区域是根据当前显示的控件数量来确定的，如果显示的控件（AB包）太少，则滚动区域小于视图范围，则不生效，_ABViewHeight会根据AB包数量累加
        _ABScrollRect = new Rect(5, 25, 240, _ABViewHeight);


        _ABScroll = GUI.BeginScrollView(_ABViewRect, _ABScroll, _ABScrollRect);
        GUI.BeginGroup(_ABScrollRect, _box);

        //Begin和End中间就是我们要显示的控件列表，当然，如果AB包数量太少，我们的滚动区域还是不能小于视图区域
        if (_ABViewHeight < _ABViewRect.height)
        {
            _ABViewHeight = (int)_ABViewRect.height;
        }

        GUI.EndGroup();
        GUI.EndScrollView();
    }

    //区域视图的范围
    private Rect _currentABViewRect;
    //区域视图滚动的范围
    private Rect _currentABScrollRect;
    //区域视图滚动的位置
    private Vector2 _currentABScroll;
    //区域高度标记，这里不用管它，是后续用来控制视图滚动量的
    private int _currentABViewHeight = 0;

    private void CurrentAssetBundlesGUI()
    {
        //区域的视图范围：左上角位置固定在上一个区域的底部，宽度固定（240），高度为窗口高度的一半再减去空隙（15），上下都有空隙
        _currentABViewRect = new Rect(5, (int)position.height / 2 + 10, 240, (int)position.height / 2 - 15);
        _currentABScrollRect = new Rect(5, (int)position.height / 2 + 10, 240, _currentABViewHeight);


        _currentABScroll = GUI.BeginScrollView(_currentABViewRect, _currentABScroll, _currentABScrollRect);
        GUI.BeginGroup(_currentABScrollRect, _box);

        if (_currentABViewHeight < _currentABViewRect.height)
        {
            _currentABViewHeight = (int)_currentABViewRect.height;
        }

        GUI.EndGroup();
        GUI.EndScrollView();
    }

    //区域视图的范围
    private Rect _assetViewRect;
    //区域视图滚动的范围
    private Rect _assetScrollRect;
    //区域视图滚动的位置
    private Vector2 _assetScroll;
    //区域高度标记，这里不用管它，是后续用来控制视图滚动量的
    private int _assetViewHeight = 0;

    private void AssetsGUI()
    {
        //区域的视图范围：左上角位置固定，宽度为窗口宽度减去左边的区域宽度以及一些空隙（255），高度为窗口高度减去上方两层标题栏以及一些空隙（50）
        _assetViewRect = new Rect(250, 45, (int)position.width - 255, (int)position.height - 50);
        _assetScrollRect = new Rect(250, 45, (int)position.width - 255, _assetViewHeight);


        _assetScroll = GUI.BeginScrollView(_assetViewRect, _assetScroll, _assetScrollRect);
        GUI.BeginGroup(_assetScrollRect, _box);

        if (_assetViewHeight < _assetViewRect.height)
        {
            _assetViewHeight = (int)_assetViewRect.height;
        }

        GUI.EndGroup();
        GUI.EndScrollView();
    }
}

//【AssetInfo.cs】
public class AssetInfo
{
    /// <summary>
    /// 资源全路径
    /// </summary>
    public string AssetFullPath
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源路径
    /// </summary>
    public string AssetPath
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string AssetName
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源的GUID（文件夹无效）
    /// </summary>
    public string GUID
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源类型（文件夹无效）
    /// </summary>
    public Type AssetType
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源文件类型
    /// </summary>
    public FileType AssetFileType
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 有效的文件资源
        /// </summary>
        ValidFile,
        /// <summary>
        /// 文件夹
        /// </summary>
        Folder,
        /// <summary>
        /// 无效的文件资源
        /// </summary>
        InValidFile
    }

    /// <summary>
    /// 资源是否勾选
    /// </summary>
    public bool IsChecked
    {
        get;
        set;
    }

    /// <summary>
    /// 文件夹是否展开（资源无效）
    /// </summary>
    public bool IsExpanding
    {
        get;
        set;
    }

    /// <summary>
    /// 所属AB包（文件夹无效）
    /// </summary>
    public string Bundled
    {
        get;
        set;
    }

    /// <summary>
    /// 文件夹的子资源（资源无效）
    /// </summary>
    public List<AssetInfo> ChildAssetInfo
    {
        get;
        set;
    }
}

