using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    public class GameMain
    {
        public static void Init()
        {
            Debug.LogError("Init �ƶ� Successfully");
            //��ʼ���ȸ���Ŀ��ܣ��Զ����¼����¼�֪ͨ
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