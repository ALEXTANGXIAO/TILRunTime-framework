using System.Collections;
using UnityEngine;

namespace HotFix_Project
{
    public class GameApp 
    {
        public static GameApp Instance = new GameApp();

        public void Init()
        {
            this.EnterGameScene();
        }

        private void EnterGameScene()
        {
            //释放游戏地图
            //GameObject mapPrefab = ResMgr.Instance.GetAssetCache<GameObject>("Camera");
            //GameObject map = GameObject.Instantiate(mapPrefab);

            //释放UI
            UIManager.Instace.ShowUI("Image");
        }
    }
}