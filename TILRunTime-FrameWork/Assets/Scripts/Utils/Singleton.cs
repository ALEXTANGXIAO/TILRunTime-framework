using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
class Singleton<T> where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new T();
                //Debug.Log(_instance != null);
            }
            return _instance;
        }
    }
}

/// <summary>
/// Unity单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : MonoBehaviour
where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance = null)
                {
                    GameObject obj = new GameObject();
                    _instance = (T)obj.AddComponent(typeof(T));
                    obj.hideFlags = HideFlags.DontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}

/// <summary>
/// Unity自动单例模式,自动实例化自己
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnityAutoSingleton<T> : MonoBehaviour
where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                _instance = obj.AddComponent<T>();
                //obj.hideFlags = HideFlags.DontSave;
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
