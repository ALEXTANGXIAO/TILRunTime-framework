
using System;
using UnityEngine;

namespace HotFix_Project
{
    class GameEvent
    {
        private static EventManager m_mgr = new EventManager();

        public static void Init()
        {
            Debug.Log("<color=#00D9FF>INIT GAMEEVENT" + "</color>");
            RegisterEvent_Logic.Register(m_mgr);
            RegisterEvent_UI.Register(m_mgr);
        }

        #region 细分的注册接口
        public static bool AddListener(int eventType, Action handler)
        {
            return m_mgr.GetDispatcher().AddListener(eventType, handler);
        }

        public static bool AddListener<T>(int eventType, Action<T> handler)
        {
            return m_mgr.GetDispatcher().AddListener(eventType, handler);
        }

        internal static void AddListener<T1, T2>(int showPaiMaiCompititionUI1, Action<object, object> showPaiMaiCompititionUI2)
        {
            throw new NotImplementedException();
        }

        internal static void AddListener(object showJiuYouPlayIntroduceUI)
        {
            throw new NotImplementedException();
        }

        public static bool AddListener<T, U>(int eventType, Action<T, U> handler)
        {
            return m_mgr.GetDispatcher().AddListener(eventType, handler);
        }

        public static bool AddListener<T, U, V>(int eventType, Action<T, U, V> handler)
        {
            return m_mgr.GetDispatcher().AddListener(eventType, handler);
        }

        public static bool AddListener<T, U, V, W>(int eventType, Action<T, U, V, W> handler)
        {
            return m_mgr.GetDispatcher().AddListener(eventType, handler);
        }

        public static void RemoveListener(int eventType, Action handler)
        {
            m_mgr.GetDispatcher().RemoveListener(eventType, handler);
        }

        public static void RemoveListener<T>(int eventType, Action<T> handler)
        {
            m_mgr.GetDispatcher().RemoveListener(eventType, handler);
        }

        public static void RemoveListener<T, U>(int eventType, Action<T, U> handler)
        {
            m_mgr.GetDispatcher().RemoveListener(eventType, handler);
        }

        public static void RemoveListener<T, U, V>(int eventType, Action<T, U, V> handler)
        {
            m_mgr.GetDispatcher().RemoveListener(eventType, handler);
        }

        public static void RemoveListener<T, U, V, W>(int eventType, Action<T, U, V, W> handler)
        {
            m_mgr.GetDispatcher().RemoveListener(eventType, handler);
        }

        public static void RemoveListener(int eventType, Delegate handler)
        {
            m_mgr.GetDispatcher().RemoveListener(eventType, handler);
        }

        #endregion

        #region 注册完整的接口实例

#if false
        public static void AddInteface<T>(T interfaceObj)
        {
            m_mgr.AddInteface(interfaceObj);
        }
#endif

        #endregion

        #region 分发消息接口

        public static T Get<T>()
        {
            return m_mgr.GetInterface<T>();
        }

        #endregion
    }
}
