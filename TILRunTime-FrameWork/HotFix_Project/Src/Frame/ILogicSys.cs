

namespace HotFix_Project
{
    /// <summary>
    /// 定义通用的逻辑接口，统一生命期调用
    /// </summary>
    public interface ILogicSys
    {
        bool OnInit();

        void OnDestroy();

        void OnStart();

        void OnUpdate();

        void OnLateUpdate();

        void OnRoleLogout();

        void OnPause();

        void OnResume();
    }
}
