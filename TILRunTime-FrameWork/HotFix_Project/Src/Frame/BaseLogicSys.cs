

namespace HotFix_Project
{
    class BaseLogicSys<T> : ILogicSys where T : new()
    {
        private static T sInstance;

        public static bool HasInstance
        {
            get { return sInstance != null; }
        }
        public static T Instance
        {
            get
            {
                if (null == sInstance)
                {
                    sInstance = new T();
                }

                return sInstance;
            }
        }

        #region virtual fucntion

        public virtual void OnRoleLogout()
        {

        }

        public virtual bool OnInit()
        {
            return true;
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnPause()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnDrawGizmos()
        {
        }

        #endregion
    }
}
