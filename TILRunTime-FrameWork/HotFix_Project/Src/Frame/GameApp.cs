
using System.Collections.Generic;

namespace HotFix_Project
{
    sealed partial class GameApp : TSingleton<GameApp>
    {
        private List<ILogicSys> m_listLogicMgr;

        public bool AddLogicSys(ILogicSys logicSys)
        {
            //判断是否存在

            if (m_listLogicMgr.Contains(logicSys))
            {
                return false;
            }

            if (!logicSys.OnInit())
            {
                return false;
            }

            m_listLogicMgr.Add(logicSys);

            return true;
        }

        public void Start()
        {
            var listLogic = m_listLogicMgr;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnStart();
            }
        }

        public void Update()
        {
            var listLogic = m_listLogicMgr;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnUpdate();
            }
        }

        public void LateUpdate()
        {
            var listLogic = m_listLogicMgr;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnLateUpdate();
            }
        }


        public void RoleLogout()
        {
            var listLogic = m_listLogicMgr;
            var logicCnt = listLogic.Count;
            for (int i = 0; i < logicCnt; i++)
            {
                var logic = listLogic[i];
                logic.OnRoleLogout();
            }
        }

        public void Destroy()
        {
        }

        public void OnPause()
        {
            for (int i = 0; i < m_listLogicMgr.Count; i++)
            {
                var logicSys = m_listLogicMgr[i];
                logicSys.OnPause();
            }
        }

        public void OnResume()
        {
            for (int i = 0; i < m_listLogicMgr.Count; i++)
            {
                var logicSys = m_listLogicMgr[i];
                logicSys.OnResume();
            }
        }
    }
}

