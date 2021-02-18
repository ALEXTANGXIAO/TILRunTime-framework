using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix_Project
{
    class UIJumpMgr : TSingleton<UIJumpMgr>
    {
        private delegate void ActionJump(uint[] param);

        private Dictionary<uint, ActionJump> m_dicUIJumpAction = new Dictionary<uint, ActionJump>();

        // 跳转到界面
        public void JumpUI(uint[] param)
        {

        }

        public UIJumpMgr()
        {
            //m_dicUIJumpAction.Add((uint)GAME_UI_TYPE.ShowQuickBuyUI, ShowQuickBuyUI);
        }
    }
}