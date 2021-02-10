using System;
using System.Collections.Generic;
using UnityEngine;

class PoolManager:UnityAutoSingleton<PoolManager>
{
    public Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
    
    /// <summary>
    /// 获取GameObject
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObject(string name)
    {
        GameObject obj = null;
        if(poolDic.ContainsKey(name)&& poolDic[name].Count > 0)
        {
            obj = poolDic[name][0];
            poolDic[name].RemoveAt(0);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        }
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 存放进缓存池
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObject(string name,GameObject obj)
    {
        obj.SetActive(false);
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].Add(obj);
        }
        else
        {
            poolDic.Add(name, new List<GameObject>() { obj });
        }
    }
}
