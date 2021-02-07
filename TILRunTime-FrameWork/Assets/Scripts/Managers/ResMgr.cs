using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResMgr : UnitySingleton<ResMgr>
{
    public override void Awake()
    {
        base.Awake();
        //this.gameObject.AddComponent<>AssetBundleManager>;
    }

    public T GetAssetCache<T>(string name)where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        //if(AssBundleConfig.IsEditorMode)
        {
            string path = "Assets/AssetsPackage/" + name;
            Debug.Log("PATH = " + path);
            UnityEngine.Object target = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            return target as T;
        }
#endif
        //return AssetBundleManager.Instance.GetAssetCache(name) as T;
        return null;
    }
}
