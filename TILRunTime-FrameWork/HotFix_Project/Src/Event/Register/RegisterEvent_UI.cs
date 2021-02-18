using System;
using System.Collections.Generic;

namespace HotFix_Project
{
    class RegisterEvent_UI
    {
        public static void Register(EventManager mgr)
        {
            var disp = mgr.GetDispatcher();

           // mgr.RegWrapInterface<IAchieveUI>(new IAchieveUI_Gen(disp));
        }
    }
}
