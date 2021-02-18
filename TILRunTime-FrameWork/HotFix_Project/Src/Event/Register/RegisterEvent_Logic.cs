using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix_Project
{
    class RegisterEvent_Logic
    {
        public static void Register(EventManager mgr)
        {
            var disp = mgr.GetDispatcher();
            //mgr.RegWrapInterface<IActorLogic>(new IActorLogic_Gen(disp));
        }
    }
}