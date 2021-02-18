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
            //��ʼ���ȸ���Ŀ��ܣ��Զ����¼����¼�֪ͨ
            GameTime.StartFrame();
            GameEvent.Init();
            GameApp.Instance.Init();

            LoadingUI.Instance.SetActive(false);
            UIManager.Instance.Init();
            //End

            //EnterGame

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