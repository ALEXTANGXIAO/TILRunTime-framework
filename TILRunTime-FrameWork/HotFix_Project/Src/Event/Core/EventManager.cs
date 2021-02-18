using System;
using System.Collections.Generic;

namespace HotFix_Project
{
    internal class DEventEntryData
    {
        public object m_interfaceWrap;
        //public object m_listIntefaceImp = null;
    };

    class EventManager
    {
        private EventDispatcher m_dispatcher = new EventDispatcher();

        /// <summary>
        /// 封装了调用的代理函数
        /// </summary>
        private Dictionary<string, DEventEntryData> m_entry = new Dictionary<string, DEventEntryData>();

        public T GetInterface<T>()
        {
            string typeName = typeof(T).FullName;
            DEventEntryData entry;
            if (m_entry.TryGetValue(typeName, out entry))
            {
                return (T)entry.m_interfaceWrap;
            }

            return default(T);
        }

#if false
        /// <summary>
        /// 注册实现了UI事件接口的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interfaceObj"></param>
        public void AddInteface<T>(T interfaceObj)
        {
            string key = typeof (T).FullName;


            DEventEntryData entry;
            if (!m_entry.TryGetValue(key, out entry))
            {
                DLogger.Assert(false);
                return;
            }

            List<T> listInter = entry.m_listIntefaceImp as List<T>;
            listInter.Add(interfaceObj);
        }

        /// <summary>
        /// 注册实现了UI事件接口的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interfaceObj"></param>
        public void RemoveInterface<T>(T interfaceObj)
        {
            string key = typeof(T).FullName;
            DEventEntryData entry;
            if (m_entry.TryGetValue(key, out entry))
            {
                List<T> listInter = entry.m_listIntefaceImp as List<T>;
                listInter.Remove(interfaceObj);
            }
        }
        
        /// <summary>
        /// 注册wrap的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callerWrap"></param>
        public void RegWrapInterface<T>(T callerWrap, object listInst)
        {
            string typeName = typeof (T).FullName;
            DLogger.Assert(!m_entry.ContainsKey(typeName));

            var entry = new DEventEntryData();
            entry.m_interfaceWrap = callerWrap;
            entry.m_listIntefaceImp = listInst;
            m_entry.Add(typeName, entry);
        }
        
#endif

        /// <summary>
        /// 注册wrap的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callerWrap"></param>
        public void RegWrapInterface<T>(T callerWrap)
        {
            string typeName = typeof(T).FullName;
            //Debug.Log(!m_entry.ContainsKey(typeName));

            var entry = new DEventEntryData();
            entry.m_interfaceWrap = callerWrap;
            m_entry.Add(typeName, entry);
        }

        public EventDispatcher GetDispatcher()
        {
            return m_dispatcher;
        }
    }
}
