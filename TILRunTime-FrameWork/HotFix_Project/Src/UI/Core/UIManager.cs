using System;
using System.Collections;
using UnityEngine;

namespace HotFix_Project
{
    public class UIManager : TSingleton<UIManager>
    {
        private Canvas canvas = null;

        public void Init()
        {
            this.canvas = GameObject.Find("UICamera/Canvas").GetComponent<Canvas>();
            GameObject root = GameObject.Find("UICamera");
            UnityEngine.Object.DontDestroyOnLoad(root);
        }

        public void ShowUI(string url)
        {
            string path = "UI/" + url + ".prefab";
            GameObject uiPrefab = ResourceManagr.Instance.GetAssetCache<GameObject>(path);
            GameObject uiView = GameObject.Instantiate(uiPrefab);
            uiView.transform.SetParent(this.canvas.transform, false);
            uiView.name = uiPrefab.name;
        }

        public void RemoveUI(string url)
        {
            
        }

        public void Create()
        {

        }

        public void Update()
        {

        }

        public void LateUpdate()
        {

        }

        internal void CloseWindow(UIWindow uIWindow)
        {
            throw new NotImplementedException();
        }
    }
}