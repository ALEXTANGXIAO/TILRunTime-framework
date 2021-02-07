using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class GameMain
    {
        public static void Init()
        {
            Debug.LogError("Init 云端 Successfully");
            //初始化热更项目框架：自定义事件，事件通知
            //End

            //EnterGame
            UIManager.Instace.Init();
            GameApp.Instance.Init();
        }

        public static void Start()
        {
            
        }

        public static void Update()
        {
            Debug.LogError("Update");
        }

        public static void LateUpdate()
        {

        }

        public static void FixedUpdate()
        {

        }
    }
}