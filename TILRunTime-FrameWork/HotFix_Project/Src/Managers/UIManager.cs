using System.Collections;
using UnityEngine;

namespace HotFix_Project
{
    public class UIManager
    {
        public static UIManager Instace = new UIManager();
        private Canvas canvas = null;

        public void Init()
        {
            this.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        public void ShowUI(string url)
        {
            string path = "UI/" + url + ".prefab";
            GameObject uiPrefab = ResMgr.Instance.GetAssetCache<GameObject>(path);
            GameObject uiView = GameObject.Instantiate(uiPrefab);
            uiView.transform.SetParent(this.canvas.transform, false);
            uiView.name = uiPrefab.name;
        }

        public void RemoveUI(string url)
        {

        }
    }
}