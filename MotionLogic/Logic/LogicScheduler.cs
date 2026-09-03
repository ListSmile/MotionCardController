using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class LogicScheduler
{
    /*
    计划（伪代码，逐步说明）：
    1. 将 `LogicScheduler` 变成单例：
       - 私有构造函数
       - 使用 `Lazy<LogicScheduler>` 提供延迟且线程安全的单例实例
       - 提供 `public static LogicScheduler Instance` 访问点
    2. 提升并发安全性：
       - 增加私有锁对象 `_lock`
       - `Add` / `Remove` 在锁内修改 `_actions`
       - 调度循环读取时先在锁内创建 `_actions` 的快照（数组），然后在锁外遍历并调用 `Tick`
         以避免在调用 `Tick` 时持有锁导致阻塞或死锁，同时防止集合在枚举时被修改
    3. 保留现有的 `Start` / `Stop` 行为，确保多次调用安全
    4. 保持可配置的 `TickIntervalMs` 和原有的行为语义
    */

    private static readonly Lazy<LogicScheduler> _instance = new(() => new LogicScheduler());
    public static LogicScheduler Instance => _instance.Value;

    private readonly List<LogicBase> _actions = new List<LogicBase>();
    private readonly object _lock = new object();
    private CancellationTokenSource _cts;
    private Task _loopTask;

    // 可配置心跳间隔（建议 50ms ~ 100ms，避免过度轮询）
    public int TickIntervalMs { get; set; } = 50;

    // 私有构造，防止外部实例化
    private LogicScheduler() { }

    public void Add(LogicBase action)
    {
        if (action == null) return;
        lock (_lock)
        {
            _actions.Add(action);
        }
    }

    public void Remove(LogicBase action)
    {
        if (action == null) return;
        lock (_lock)
        {
            _actions.Remove(action);
        }
    }

    public void Start()
    {
        if (_loopTask != null) return;
        _cts = new CancellationTokenSource();
        _loopTask = Task.Run(() => SchedulerLoop(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
        _loopTask?.Wait(TimeSpan.FromSeconds(2));
        _cts?.Dispose();
        _cts = null;
        _loopTask = null;
    }

    private async Task SchedulerLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            LogicBase[] snapshot;
            lock (_lock)
            {
                snapshot = _actions.ToArray();
            }

            for (int i = snapshot.Length - 1; i >= 0; i--)
            {
                try
                {
                    snapshot[i].Tick();
                }
                catch
                {
                    // 保持简洁：忽略单个逻辑的异常，避免中断调度循环
                }
            }

            try
            {
                await Task.Delay(TickIntervalMs, token).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // 取消后退出循环
                break;
            }
        }
    }
}