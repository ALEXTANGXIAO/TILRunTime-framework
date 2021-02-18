
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    sealed partial class GameApp
    {
        public void Init()
        {
            m_listLogicMgr = new List<ILogicSys>();
            RegistAllSystem();
        }

        private void RegistAllSystem()
        {
            //所有全局的系统，都在这儿注册
            //系统将会按照按照注册的顺序初始化
            AddLogicSys(BehaviourSingleSystem.Instance);
        }
    }
}
