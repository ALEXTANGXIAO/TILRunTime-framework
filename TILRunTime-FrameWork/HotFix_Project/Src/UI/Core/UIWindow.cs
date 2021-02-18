using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HotFix_Project
{
    public enum ModalType
    {
        /// <summary> 普通模态 </summary>
        NormalType,

        /// <summary> 透明模态 </summary>
        TransparentType,

        /// <summary> 普通状态且有关闭功能 </summary>
        NormalHaveClose,

        /// <summary> 透明状态且有关闭功能 </summary>
        TransparentHaveClose,

        /// <summary> 非模态 </summary>
        NoneType,
    }

    public enum UIWindowType
    {
        /// <summary> 普通窗口 </summary>
        WindowNormal,

        /// <summary> 置顶窗口 </summary>
        WindowTop,

        /// <summary> 模态窗口 </summary>
        WindowModel
    }

    public enum UIWindowOpenAudio
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 打开界面1（对战）
        /// </summary>
        OpenWindowOne,

        /// <summary>
        /// 打开界面1（通用）
        /// </summary>
        OpenWindowTwo
    }

    public enum UIWindowCloseType
    {
        /// <summary>
        /// 关闭就释放掉
        /// </summary>
        CloseDestroy = 0,

        /// <summary>
        /// 把对象Inactive,不用destory掉
        /// </summary>
        CloseInactive,
    }

    class UIBase
    {
        protected GameObject m_go;
        protected RectTransform m_transform;
        protected string m_name;

        protected bool m_destroyed = true;

        private List<WindowMsgRecord> uiRegistMsgList;
        protected List<WindowMsgRecord> m_uiRegistMsgList
        {
            get
            {
                if (uiRegistMsgList == null)
                    uiRegistMsgList = new List<WindowMsgRecord>();
                return uiRegistMsgList;
            }
        }

        public bool IsDestroyed
        {
            get { return m_destroyed; }
        }

        public bool IsCreated
        {
            get { return !IsDestroyed; }
        }

        public RectTransform transform
        {
            get { return m_transform; }
        }

        public GameObject gameObject
        {
            get { return m_go; }
        }

        public string name
        {
            get
            {
                if (string.IsNullOrEmpty(m_name))
                {
                    m_name = GetType().Name;
                }

                return m_name;
            }
        }

        protected void ClearAllRegistEvent()
        {
            var msgList = uiRegistMsgList;
            if (msgList != null)
            {
                for (int i = 0; i < msgList.Count; i++)
                {
                    var msg = msgList[i];
                    GameEvent.RemoveListener(msg.m_msgType, msg.m_handler);
                }

                msgList.Clear();
            }
        }

        #region override

        protected void AddUIEvent(int eventType, Action handler)
        {
            if (GameEvent.AddListener(eventType, handler))
            {
                m_uiRegistMsgList.Add(new WindowMsgRecord(eventType, handler));
            }
        }

        protected void AddUIEvent<T>(int eventType, Action<T> handler)
        {
            if (GameEvent.AddListener(eventType, handler))
            {
                m_uiRegistMsgList.Add(new WindowMsgRecord(eventType, handler));
            }
        }

        protected void AddUIEvent<T, U>(int eventType, Action<T, U> handler)
        {
            if (GameEvent.AddListener(eventType, handler))
            {
                m_uiRegistMsgList.Add(new WindowMsgRecord(eventType, handler));
            }
        }

        protected void AddUIEvent<T, U, V>(int eventType, Action<T, U, V> handler)
        {
            if (GameEvent.AddListener(eventType, handler))
            {
                m_uiRegistMsgList.Add(new WindowMsgRecord(eventType, handler));
            }
        }

        protected void AddUIEvent<T, U, V, W>(int eventType, Action<T, U, V, W> handler)
        {
            if (GameEvent.AddListener(eventType, handler))
            {
                m_uiRegistMsgList.Add(new WindowMsgRecord(eventType, handler));
            }
        }

        #endregion
    }

    class WindowMsgRecord
    {
        public int m_msgType;
        public Delegate m_handler;

        public WindowMsgRecord(int msgId, Delegate handler)
        {
            m_msgType = msgId;
            m_handler = handler;
        }
    }

    partial class UIWindowBase : UIBase
    {
        /// <summary>
        /// 所属的window
        /// </summary>
        protected UIWindowBase m_parent = null;
        protected Canvas m_canvas;

        private List<UIWindowBase> m_listChild = null;
        private List<UIWindowBase> m_listUpdateChild = null;
        private bool m_updateListValid = false;

        /// <summary>
        /// 当前是否显示出来了
        /// </summary>
        protected bool m_visible = false;

        /// <summary>
        /// 是否首次显示过了
        /// </summary>
        protected bool m_firstVisible = false;


        /// <summary>
        /// 关联的特效路径
        /// </summary>
        private List<string> m_listEffectPath = null;

        public UIWindowBase Parent
        {
            get { return m_parent; }
        }

        public virtual UIWindowBaseType BaseType
        {
            get { return UIWindowBaseType.None; }
        }

        public List<string> LoadedEffectPath
        {
            get { return m_listEffectPath; }
        }

        protected UIManager m_ownUIManager = null;

        protected UIManager UIMgr
        {
            get
            {
                if (m_ownUIManager == null && m_parent != null)
                {
                    return m_parent.UIMgr;
                }

                return m_ownUIManager;
            }
        }


        #region 最基础的接口

        /**
         * 创建对象
         *
         * bindGO 是否把GameObject和Window绑定在一起
         */
        protected bool CreateBase(GameObject go, bool bindGo)
        {
            ///has created
            if (!m_destroyed)
            {
                return false;
            }

            if (go == null)
            {
                return false;
            }

            m_destroyed = false;
            m_go = go;

            m_transform = go.GetComponent<RectTransform>();
            m_canvas = gameObject.GetComponent<Canvas>();

            var canvas = gameObject.GetComponentsInChildren<Canvas>(true);
            for (var i = 0; i < canvas.Length; i++)
            {
                var canva = canvas[i];
                canva.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
            }

            return true;
        }

        protected void DestroyAllChild()
        {
            //销毁子对象
            if (m_listChild != null)
            {
                for (int i = 0; i < m_listChild.Count; i++)
                {
                    var child = m_listChild[i];
                    child.Destroy();
                }

                m_listChild.Clear();
            }
        }

        public void Destroy()
        {
            if (IsDestroyed)
            {
                return;
            }

            m_destroyed = true;
            //DLogger.Debug("destroy window or widget: {0}", GetType().Name);

            ///关闭注册的消息
            ClearAllRegistEvent();
            OnDestroy();
            DestoryEffect();
            DestroyAllChild();

            if (m_parent != null)
            {
                m_parent.RmvChild(this);
                m_parent = null;
            }

            if (m_go != null)
            {
                //DodUtil.SafeDestroy(m_go);
                m_go = null;
            }

            m_transform = null;
        }

        #endregion


        protected void DestoryEffect()
        {
            if (m_listEffectPath != null)
            {
                m_listEffectPath.Clear();
            }
        }

        public void AddChild(UIWindowBase child)
        {
            if (m_listChild == null)
            {
                m_listChild = new List<UIWindowBase>();
            }

            m_listChild.Add(child);
            MarkListChanged();
        }

        public void RmvChild(UIWindowBase child)
        {
            //如果已经销毁了或者销毁过程中，那么不掉用删除
            if (m_destroyed)
            {
                return;
            }

            if (m_listChild != null)
            {
                if (m_listChild.Remove(child))
                {
                    MarkListChanged();
                }
            }
        }

        /// <summary>
        /// 重新整理update和lateupdate的调用缓存
        /// </summary>
        private void MarkListChanged()
        {
            m_updateListValid = false;
            if (m_parent != null)
            {
                m_parent.MarkListChanged();
            }
        }


        #region 子类调用的接口

        //         public GameObject CreateUIEffectGo(UIEffectWidget effectWidget, RectTransform parent, UIParticleType particleType, int sortOrder = 0,
        //             bool destroyOld = true, float scale = 1f, uint param1 = 0, uint param2 = 0, uint param3 = 0)
        //         {
        //             if (effectWidget == null)
        //             {
        //                 DLogger.EditorFatal("特效父节点为空");
        //                 return null;
        //             }
        //             GameObject go = UIEffectHelper.CreateUIParticle(particleType, effectWidget.transform, sortOrder, destroyOld, scale, param1,
        //                 param2, param3);
        // 
        //             if (go != null)
        //             {
        //                 effectWidget.AddEffect(go, parent, sortOrder);
        //                 //AddRegistResourcePath(path);
        //             }
        //             return go;
        //         }


        #endregion


        #region 通用的操作接口

        public Transform FindChild(string path)
        {
            return DUnityUtil.FindChild(transform, path);
        }

        public Transform FindChild(Transform _transform, string path)
        {
            return DUnityUtil.FindChild(_transform, path);
        }

        public T FindChildComponent<T>(string path) where T : Component
        {
            return DUnityUtil.FindChildComponent<T>(transform, path);
        }

        public T FindChildComponent<T>(Transform _transform, string path) where T : Component
        {
            return DUnityUtil.FindChildComponent<T>(_transform, path);
        }


        public void Show(bool visible)
        {
            // 加个保护
            if (m_destroyed || gameObject == null)
            {
                return;
            }
            if (m_visible != visible)
            {
                m_visible = visible;
                if (visible)
                {
                    gameObject.SetActive(true);
                    _OnVisible();
                }
                else
                {
                    _OnHidden();

                    if (gameObject == null)
                    {

                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }

                MarkListChanged();
            }
        }

        protected void _OnVisible()
        {
            if (m_listChild != null)
            {
                for (int i = 0; i < m_listChild.Count; i++)
                {
                    var child = m_listChild[i];
                    if (child.gameObject.activeInHierarchy)
                    {
                        child._OnVisible();
                    }
                }
            }
            if (!m_firstVisible)
            {
                m_firstVisible = true;
                OnFirstVisible();
            }
            OnVisible();
        }

        protected void _OnHidden()
        {
            if (m_listChild != null)
            {
                for (int i = 0; i < m_listChild.Count; i++)
                {
                    var child = m_listChild[i];
                    if (child.gameObject.activeInHierarchy)
                    {
                        child._OnHidden();
                    }
                }
            }
            OnHidden();
        }


        /// <summary>
        /// 返回是否有必要下一帧继续执行
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if (!m_visible || m_destroyed)
            {
                return false;
            }

            List<UIWindowBase> listNextUpdateChild = null;
            if (m_listChild != null && m_listChild.Count > 0)
            {
                listNextUpdateChild = m_listUpdateChild;
                var updateListValid = m_updateListValid;
                List<UIWindowBase> listChild = null;
                if (!updateListValid)
                {
                    if (listNextUpdateChild == null)
                    {
                        listNextUpdateChild = new List<UIWindowBase>();
                        m_listUpdateChild = listNextUpdateChild;
                    }
                    else
                    {
                        listNextUpdateChild.Clear();
                    }

                    listChild = m_listChild;
                }
                else
                {
                    listChild = listNextUpdateChild;
                }

                for (int i = 0; i < listChild.Count; i++)
                {
                    var window = listChild[i];

                    var needValid = window.Update();

                    if (!updateListValid && needValid)
                    {
                        listNextUpdateChild.Add(window);
                    }
                }

                if (!updateListValid)
                {
                    m_updateListValid = true;
                }
            }

            bool needUpdate = false;
            if (listNextUpdateChild == null || listNextUpdateChild.Count <= 0)
            {
                m_hasOverrideUpdate = true;
                OnUpdate();
                needUpdate = m_hasOverrideUpdate;
            }
            else
            {
                OnUpdate();
                needUpdate = true;
            }

            return needUpdate;
        }

        #endregion

        #region 子类扩展的接口

        /// <summary> 脚本生成的代码 </summary>
        protected virtual void ScriptGenerator()
        {

        }

        /// <summary>
        ///  绑定代码和prefab之间元素的关系
        /// </summary>
        protected virtual void BindMemberProperty()
        {
        }

        protected virtual void RegisterEvent()
        {
        }

        private bool m_hasOverrideUpdate = true;
        protected virtual void OnUpdate()
        {
            m_hasOverrideUpdate = false;

        }

        /// <summary>
        /// 界面创建出来的时候调用，被覆盖不可见不会重复触发
        /// </summary>
        protected virtual void OnCreate()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 创建出来首次visible
        /// 用来播放一些显示动画之类的
        /// </summary>
        protected virtual void OnFirstVisible()
        {
        }

        /// <summary>
        /// 当显示出来的时候调用
        /// 包括首次初始化后显示和上面的界面消失后重新恢复显示
        /// </summary>
        protected virtual void OnVisible()
        {
        }

        /// <summary>
        /// 界面不可见的时候调用
        /// 当被上层全屏界面覆盖后，也会触发一次隐藏
        /// </summary>
        protected virtual void OnHidden()
        {
        }

        protected void _OnSortingOrderChg()
        {
            if (m_listChild != null)
            {
                for (int i = 0; i < m_listChild.Count; i++)
                {
                    if (m_listChild[i].m_visible)
                    {
                        m_listChild[i]._OnSortingOrderChg();
                    }
                }
            }
            OnSortingOrderChg();
        }

        protected virtual void OnSortingOrderChg()
        {
        }
        #endregion


        #region 私有

        /// <summary>
        /// 为了ab版本中，UI资源被释放的问题
        /// </summary>
        /// <param name="resPath"></param>
        private void AddRegistResourcePath(string resPath)
        {
            if (m_listEffectPath == null)
            {
                m_listEffectPath = new List<string>();
            }

            if (!m_listEffectPath.Contains(resPath))
            {
                m_listEffectPath.Add(resPath);
            }
        }

        #endregion
    }

    /**
     * 用来封装各个界面里子模块用
     */
    class UIWindowWidget : UIWindowBase
    {
        public int SortingOrder
        {
            get
            {
                if (m_canvas != null)
                {
                    return m_canvas.sortingOrder;
                }

                return 0;
            }

            set
            {
                if (m_canvas != null)
                {
                    int oldOrder = m_canvas.sortingOrder;
                    if (oldOrder != value)
                    {
                        var listCanvas = gameObject.GetComponentsInChildren<Canvas>(true);
                        for (int i = 0; i < listCanvas.Length; i++)
                        {
                            var childCanvas = listCanvas[i];
                            childCanvas.sortingOrder = value + (childCanvas.sortingOrder - oldOrder);
                        }
                        m_canvas.sortingOrder = value;
                        _OnSortingOrderChg();
                    }
                }
            }
        }
        /// <summary>
        /// 所属的窗口
        /// </summary>
        public UIWindow OwnerWindow
        {
            get
            {
                var parent = m_parent;
                while (parent != null)
                {
                    if (parent.BaseType == UIWindowBaseType.Window)
                    {
                        return parent as UIWindow;
                    }

                    parent = parent.Parent;
                }

                return null;
            }
        }

        public override UIWindowBaseType BaseType
        {
            get { return UIWindowBaseType.Widget; }
        }

        /// <summary> 根据类型创建 </summary>
        public bool CreateByType<T>(UIWindowBase parent, Transform parentTrans = null) where T : UIWindowWidget
        {
            string resPath = string.Format("UI/{0}", typeof(T).Name);
            return CreateByPath(resPath, parent, parentTrans);
        }

        /// <summary> 根据资源名创建 </summary>
        public bool CreateByPath(string resPath, UIWindowBase parent, Transform parentTrans = null, bool visible = true)
        {
            GameObject goInst = ResourceManagr.Instance.GetAssetCache<GameObject>(resPath);//DResources.AllocGameObject(resPath, parentTrans);
            if (goInst == null)
            {
                return false;
            }
            if (!Create(parent, goInst, visible))
            {
                return false;
            }
            goInst.transform.localScale = Vector3.one;
            goInst.transform.localPosition = Vector3.zero;
            return true;
        }

        /**
         * 根据prefab或者模版来创建新的 widget
         */
        public bool CreateByPrefab(UIWindowBase parent, GameObject goPrefab, Transform parentTrans, bool visible = true)
        {
            if (parentTrans == null)
            {
                parentTrans = parent.transform;
            }

            var widgetRoot = GameObject.Instantiate(goPrefab, parentTrans);
            return CreateImp(parent, widgetRoot, true, visible);
        }

        /**
         * 创建窗口内嵌的界面
         */
        public bool Create(UIWindowBase parent, GameObject widgetRoot, bool visible = true)
        {
            return CreateImp(parent, widgetRoot, false, visible);
        }

        #region 私有的函数

        private bool CreateImp(UIWindowBase parent, GameObject widgetRoot, bool bindGo, bool visible = true)
        {
            if (!CreateBase(widgetRoot, bindGo))
            {
                return false;
            }
            RestChildCanvas(parent);
            m_parent = parent;
            if (m_parent != null)
            {
                m_parent.AddChild(this);
            }

            if (m_canvas != null)
            {
                m_canvas.overrideSorting = true;
            }
            ScriptGenerator();
            BindMemberProperty();
            RegisterEvent();

            OnCreate();

            if (visible)
            {
                Show(true);
            }
            else
            {
                widgetRoot.SetActive(false);
            }

            return true;
        }

        private void RestChildCanvas(UIWindowBase parent)
        {
            if (gameObject == null)
            {
                return;
            }
            if (parent == null || parent.gameObject == null)
            {
                return;
            }
            Canvas parentCanvas = parent.gameObject.GetComponentInParent<Canvas>();
            if (parentCanvas == null)
            {
                return;
            }
            var listCanvas = gameObject.GetComponentsInChildren<Canvas>(true);
            for (var index = 0; index < listCanvas.Length; index++)
            {
                var childCanvas = listCanvas[index];
                childCanvas.sortingOrder = parentCanvas.sortingOrder + childCanvas.sortingOrder % UIWindow.MaxCanvasSortingOrder;
            }
        }

        #endregion
    }

    enum UIWindowBaseType
    {
        None,
        Window,
        Widget,
    }

    /// <summary>
    /// 定义每个UI界面窗口
    /// </summary>
    class UIWindow : UIWindowBase
    {
        /// <summary>
        /// 一个界面中最大sortOrder值
        /// </summary>
        public const int MaxCanvasSortingOrder = 50;

        public float m_zRange = 0; //z轴宽度，只能应用于Normal类型的窗口，只有需要显示模型的UI才需要修改本参数

        public override UIWindowBaseType BaseType
        {
            get { return UIWindowBaseType.Window; }
        }

        /// <summary>
        /// 是否为全屏UI
        /// 显示全屏UI时候，会模糊场景相机
        /// </summary>
        public virtual bool IsFullScreen
        {
            get { return false; }
        }

        /// <summary>
        /// 是否需要相机背景模糊
        /// </summary>
        public virtual bool NeedBlurCamera
        {
            get { return false; }
        }

        public virtual bool NeedCenterUI()
        {
            return true;
        }

        /// <summary>
        /// 是否是全屏界面，而且把后面场景全部盖住
        /// </summary>
        public virtual bool IsFullScreenMaskScene
        {
            get { return false; }
        }

        private Image m_modalImage;
        private float m_modalAlpha = 0.86f;

        private static uint m_nextWindowId = 0;

        /// <summary>
        /// 窗口Id
        /// </summary>
        private uint m_windowId = 0;

        /// <summary>
        /// 是否固定SortingOrder
        /// </summary>
        public virtual bool IsFixedSortingOrder
        {
            get { return false; }
        }
        /// <summary>
        /// SortingOrder = stack.m_baseOrder + FixedAdditionalOrder
        /// </summary>
        public virtual int FixedAdditionalOrder
        {
            get { return UIWindow.MaxCanvasSortingOrder; }
        }

        public int SortingOrder
        {
            get
            {
                if (m_canvas != null)
                {
                    return m_canvas.sortingOrder;
                }

                return 0;
            }

            set
            {
                if (m_canvas != null)
                {
                    int oldOrder = m_canvas.sortingOrder;
                    if (oldOrder != value)
                    {
                        var listCanvas = gameObject.GetComponentsInChildren<Canvas>(true);
                        for (int i = 0; i < listCanvas.Length; i++)
                        {
                            var childCanvas = listCanvas[i];
                            childCanvas.sortingOrder = value + (childCanvas.sortingOrder - oldOrder);
                        }
                        m_canvas.sortingOrder = value;
                        _OnSortingOrderChg();
                    }
                }
            }
        }

        /// <summary>
        /// 窗口Id
        /// </summary>
        public uint WindowId
        {
            get { return m_windowId; }
        }

        public bool Visible
        {
            get { return m_visible; }
        }

        private bool m_isClosed = false;
        private bool m_isCreating = false;

        public Camera CanvasCamera
        {
            get
            {
                if (m_canvas != null)
                {
                    return m_canvas.worldCamera;
                }

                return null;
            }
        }

        public void AllocWindowId()
        {
            if (m_nextWindowId == 0)
            {
                m_nextWindowId++;
            }

            m_windowId = m_nextWindowId++;
        }


        #region call from UIMananger

        public bool Create(UIManager uiMgr, GameObject uiGo)
        {
            if (IsCreated)
            {
                return true;
            }

            m_isClosed = false;
            if (!CreateBase(uiGo, true))
            {
                return false;
            }

            if (m_canvas == null)
            {
                Destroy();
                return false;
            }

            m_ownUIManager = uiMgr;
            m_firstVisible = false;

            if (m_canvas != null)
            {
                m_canvas.overrideSorting = true;
            }

            ScriptGenerator();
            BindMemberProperty();
            RegisterEvent();

            m_isCreating = true;
            OnCreate();
            m_isCreating = false;

            if (m_isClosed)
            {
                Destroy();
                return false;
            }

            SetModalState(GetModalType());
            return true;
        }

        private void SetModalState(ModalType type)
        {
            var canClose = false;
            switch (type)
            {
                case ModalType.NormalType:
                    {
                        break;
                    }
                case ModalType.NormalHaveClose:
                    {
                        canClose = true;
                        break;
                    }
                case ModalType.TransparentType:
                    {
                        m_modalAlpha = 0.01f;
                        break;
                    }
                case ModalType.TransparentHaveClose:
                    {
                        m_modalAlpha = 0.01f;
                        canClose = true;
                        break;
                    }
                default:
                    {
                        m_modalAlpha = 0f;
                        break;
                    }
            }
            if (m_modalAlpha > 0)
            {
                string path = "UI/ModalSprite";
                GameObject modal = ResourceManagr.Instance.GetAssetCache<GameObject>(path);//DResources.AllocGameObject(path);
                modal.transform.SetParent(transform);
                modal.transform.SetAsFirstSibling();
                modal.transform.localScale = Vector3.one;
                modal.transform.localPosition = Vector3.zero;
                if (canClose)
                {
                    var button = DUnityUtil.AddMonoBehaviour<Button>(modal);
                    button.onClick.AddListener(Close);
                }
                m_modalImage = DUnityUtil.AddMonoBehaviour<Image>(modal);
                m_modalImage.color = new Color(0, 0, 0, m_modalAlpha);
            }
        }

        public void SetModalAlpha(float alpha)
        {
            m_modalAlpha = alpha;
            if (m_modalImage != null)
            {
                m_modalImage.color = new Color(0, 0, 0, m_modalAlpha);
            }
        }

        #endregion

        #region virtual function

        public virtual UIWindowType GetWindowType()
        {
            return UIWindowType.WindowNormal;
        }

        public virtual ModalType GetModalType()
        {
            if (IsFullScreen || GetWindowType() == UIWindowType.WindowTop)
            {
                return ModalType.TransparentType;
            }
            return ModalType.NormalType;
        }

        // 是否为特殊UI
        public virtual bool IsSpecUI()
        {
            return false;
        }

        public virtual bool IsPvpShow()
        {
            return false;
        }

        // 是否为固定UI 不可直接关闭的 比如主界面和战斗界面
        public virtual bool IsFixedUI()
        {
            return false;
        }

        #endregion


        /**
         * 关闭窗口
         */
        public virtual void Close()
        {
            if (m_isCreating)
            {
                m_isClosed = true;
                return;
            }

            var mgr = UIMgr;
            if (mgr != null)
            {
                mgr.CloseWindow(this);
            }
        }
    }
}