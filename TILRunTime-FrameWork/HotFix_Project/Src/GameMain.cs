using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class GameMain
    {
        public static void Init()
        {
            Debug.LogError("Init bversion Successfully");
            //初始化热更项目框架：自定义事件，事件通知
            GameTime.StartFrame();
            GameEvent.Init();
            GameApp.Instance.Init();
            UIManager.Instance.Init();
            //End

            //EnterGame
            LoadingUI.Instance.SetActive(false);

            GameObject mapPrefab = ResourceManagr.Instance.GetAssetCache<GameObject>("Cube.prefab");
            GameObject map = GameObject.Instantiate(mapPrefab);
        }

        public static void Start()
        {
            GameTime.StartFrame();
            GameApp.Instance.Start();
        }

        public static void Update()
        {
            GameTime.StartFrame();
            GameApp.Instance.Update();
        }

        public static void LateUpdate()
        {
            GameTime.StartFrame();
            GameApp.Instance.LateUpdate();
        }

        public static void FixedUpdate()
        {

        }

        public static void Destroy()
        {
            GameTime.StartFrame();
            GameApp.Instance.Destroy();
        }

        public static void OnApplicationPause(bool isPause)
        {
            GameTime.StartFrame();
            if (isPause)
            {
                GameApp.Instance.OnPause();
            }
            else
            {
                GameApp.Instance.OnResume();
            }
        }
    }
}