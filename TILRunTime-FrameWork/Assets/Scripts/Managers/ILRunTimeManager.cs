using UnityEngine;
using System.Collections;
using System.IO;
using ILRuntime.Runtime.Enviorment;

public class ILRunTimeManager : UnitySingleton<ILRunTimeManager>
{
    private bool isGameStart = false;
    AppDomain appdomain; //ILRunTime=>解释执行虚拟机
    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;

    public override void Awake()
    {
        base.Awake();
        this.isGameStart = false;
        this.InitILRunTime();
    }

    public void InitILRunTime()
    {
        this.appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
    }

    /// <summary>
    /// 开发环境pdb调试
    /// </summary>
    /// <param name="dll"></param>
    /// <param name="pdb"></param>
    public void LoadHotFixAssembly(byte[] dll, byte[] pdb)
    {
        this.fs = new MemoryStream(dll);
        if (pdb != null)
        {
            this.p = new MemoryStream(pdb);
        }
        try
        {
            appdomain.LoadAssembly(fs, p, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("TILRunTIme加载热更DLL失败,请确保已经编译过Dll");
            return;
        }

        this.InitializeILRuntime();
    }

    /// <summary>
    /// 生成环境，不使用pdb
    /// </summary>
    /// <param name="dll"></param>
    public void ProDuceLoadHotFixAssembly(byte[] dll)
    {
        this.fs = new MemoryStream(dll);
        try
        {
            appdomain.LoadAssembly(fs, null, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        catch
        {
            Debug.LogError("TILRunTIme加载热更DLL失败,请确保已经编译过Dll");
            return;
        }

        this.InitializeILRuntime();
    }

    void InitializeILRuntime()
    {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
    }

    /// <summary>
    /// 进入热更代码/例namespace-DodGame
    /// </summary>
    public void EnterGame()
    {
        this.isGameStart = true;
#if UNITY_EDITOR
        appdomain.DebugService.StartDebugService(56000);
#endif
        appdomain.Invoke("HotFix_Project.GameMain", "Init", null, null);
    }

    public void Start()
    {
        if (isGameStart)
        {
            appdomain.Invoke("HotFix_Project.GameMain", "Start", null, null);
        }
    }

    private void Update()
    {
        if (isGameStart)
        {
            appdomain.Invoke("HotFix_Project.GameMain", "Update", null, null);
        }
    }

    private void LateUpdate()
    {
        if (isGameStart)
        {
            appdomain.Invoke("HotFix_Project.GameMain", "LateUpdate", null, null);
        }
    }

    private void FixedUpdate()
    {
        if (isGameStart)
        {
            appdomain.Invoke("HotFix_Project.GameMain", "FixedUpdate", null, null);
        }
    }

    private void OnDestroy()
    {
        if (isGameStart)
        {
            appdomain.Invoke("HotFix_Project.GameMain", "Destroy", null, null);
        }
    }
}
