using System;
using System.Collections.Generic;

namespace HotFix_Project
{
    class DEventDelegateData
    {
        private int m_eventType = 0;

        /// <summary>
        /// 不免每次判断是否重合都要new一个，造成gc
        /// </summary>
        public List<Delegate> m_listExist = new List<Delegate>();
        //         private Delegate m_handler = null;

        private List<Delegate> m_addList = new List<Delegate>();
        private List<Delegate> m_deleteList = new List<Delegate>();
        private bool m_isExcute = false;
        private bool m_dirty = false;

        public DEventDelegateData(int evnetType)
        {
            m_eventType = evnetType;
        }

        public bool AddHandler(Delegate handler)
        {
            if (m_listExist.Contains(handler))
            {
                //DLogger.EditorFatal("repeate add handler");
                return false;
            }

            if (m_isExcute)
            {
                m_dirty = true;
                m_addList.Add(handler);
            }
            else
            {
                m_listExist.Add(handler);
            }

            return true;
        }

        public void RmvHandler(Delegate hander)
        {
            if (m_isExcute)
            {
                m_dirty = true;
                m_deleteList.Add(hander);
            }
            else
            {
                if (!m_listExist.Remove(hander))
                {
                    //DLogger.EditorFatal("delete handle failed, not exist, EvntId: {0}", StringId.HashToString(m_eventType));
                }
            }
        }

        private void CheckModify()
        {
            m_isExcute = false;
            if (m_dirty)
            {
                for (int i = 0; i < m_addList.Count; i++)
                {
                    m_listExist.Add(m_addList[i]);
                }

                m_addList.Clear();

                for (int i = 0; i < m_deleteList.Count; i++)
                {
                    m_listExist.Remove(m_deleteList[i]);
                }

                m_deleteList.Clear();
            }
        }

        public void AddHandleByInteface(object interObj)
        {
        }

        public void Callback()
        {
            m_isExcute = true;
            for (var i = 0; i < m_listExist.Count; i++)
            {
                var d = m_listExist[i];
                Action action = d as Action;
                if (action != null)
                {
                    action();
                }
            }

            CheckModify();
        }

        public void Callback<T>(T arg1)
        {
            m_isExcute = true;
            for (var i = 0; i < m_listExist.Count; i++)
            {
                var d = m_listExist[i];
                var action = d as Action<T>;
                if (action != null)
                {
                    action(arg1);
                }
            }

            CheckModify();
        }

        public void Callback<T, U>(T arg1, U arg2)
        {
            m_isExcute = true;
            for (var i = 0; i < m_listExist.Count; i++)
            {
                var d = m_listExist[i];
                var action = d as Action<T, U>;
                if (action != null)
                {
                    action(arg1, arg2);
                }
            }

            CheckModify();
        }

        public void Callback<T, U, V>(T arg1, U arg2, V arg3)
        {
            m_isExcute = true;
            for (var i = 0; i < m_listExist.Count; i++)
            {
                var d = m_listExist[i];
                var action = d as Action<T, U, V>;
                if (action != null)
                {
                    action(arg1, arg2, arg3);
                }
            }

            CheckModify();
        }

        public void Callback<T, U, V, W>(T arg1, U arg2, V arg3, W arg4)
        {
            m_isExcute = true;
            for (var i = 0; i < m_listExist.Count; i++)
            {
                var d = m_listExist[i];
                var action = d as Action<T, U, V, W>;
                if (action != null)
                {
                    action(arg1, arg2, arg3, arg4);
                }
            }

            CheckModify();
        }
    }

    /// <summary>
    /// 封装消息的底层分发和注册
    /// </summary>
    class EventDispatcher
    {
        static Dictionary<int, DEventDelegateData> m_eventTable = new Dictionary<int, DEventDelegateData>();

        #region 事件管理接口
        public bool AddListener(int eventType, Delegate handler)
        {
            DEventDelegateData data;
            if (!m_eventTable.TryGetValue(eventType, out data))
            {
                data = new DEventDelegateData(eventType);
                m_eventTable.Add(eventType, data);
            }

            return data.AddHandler(handler);
        }

        public void RemoveListener(int eventType, Delegate handler)
        {
            DEventDelegateData data;
            if (m_eventTable.TryGetValue(eventType, out data))
            {
                data.RmvHandler(handler);
            }
        }

        #endregion 

        #region 事件分发接口

        public void Send(int eventType)
        {
            DEventDelegateData d;
            if (m_eventTable.TryGetValue(eventType, out d))
            {
                d.Callback();
            }
        }

        public void Send<T>(int eventType, T arg1)
        {
            DEventDelegateData d;
            if (m_eventTable.TryGetValue(eventType, out d))
            {
                d.Callback(arg1);
            }
        }

        public void Send<T, U>(int eventType, T arg1, U arg2)
        {
            DEventDelegateData d;
            if (m_eventTable.TryGetValue(eventType, out d))
            {
                d.Callback(arg1, arg2);
            }
        }

        public void Send<T, U, V>(int eventType, T arg1, U arg2, V arg3)
        {
            DEventDelegateData d;
            if (m_eventTable.TryGetValue(eventType, out d))
            {
                d.Callback(arg1, arg2, arg3);
            }
        }

        public void Send<T, U, V, W>(int eventType, T arg1, U arg2, V arg3, W arg4)
        {
            DEventDelegateData d;
            if (m_eventTable.TryGetValue(eventType, out d))
            {
                d.Callback(arg1, arg2, arg3, arg4);
            }
        }

        #endregion

    }
}
