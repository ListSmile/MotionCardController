using System;

/// <summary>
/// 轻量级逻辑动作基类（无内置循环，由外部调度器驱动）
/// </summary>
public abstract class LogicBase
{
    /// <summary>
    /// 当前的逻辑任务名称，用于调试和日志记录
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 逻辑的帮助类，比如非阻塞延时和边沿检测等功能，派生类可使用该工具简化逻辑实现。
    /// </summary>
    public LogicHelper LGTool { get; set; } = new LogicHelper();
    /// <summary>
    /// 指示动作是否为长时间运行且不允许中途停止（例如，连续运动、长时间加工等），用于外部调度器判断是否可以中断或暂停该动作。
    /// </summary>
    public bool LongTimeNoStop { get; set; }
    /// <summary>
    /// 当前的步骤索引，由派生类的 ActualMotionLogic 方法更新，外部调度器可根据该索引判断动作的进度。
    /// </summary>
    public int CurrentStep { get; private set; }
    protected LogicBase(string name)
    {
        Name = name;
    }
    private readonly object _lock = new object();
    /// <summary>
    /// 是否正在运行
    /// </summary>
    public bool IsRunning { get; private set; }
    /// <summary>
    /// 是否已暂停（外部调度器可根据该状态决定是否继续调用 Tick 方法）
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    /// 派生类实现步骤逻辑（switch case），返回下一步索引（-1 表示结束）
    /// </summary>
    protected abstract int ActualMotionLogic(int step);
    /// <summary>
    /// 由外部调度器每帧/心跳调用，返回 false 表示动作已结束
    /// </summary>
    public bool Tick()
    {
        lock (_lock)
        {
            if (LongTimeNoStop)
            {
                IsRunning = true;
                IsPaused = false;
            }
            if (!IsRunning || IsPaused)
                return IsRunning; // 暂停时仍保持存活状态
            ///返回 -1 代表逻辑结束，外部调度器可根据返回值决定是否继续调用 Tick 方法
            ActualMotionLogic(CurrentStep);
            return true;
        }
    }

    // 控制方法（无需启动循环）
    public void Start()
    {
        lock (_lock) {GoToStep(1); IsRunning = true; IsPaused = false; }
    }
    public void Stop()
    {
        lock (_lock) 
        {
            IsRunning = false; 
        }
    }
    public void Pause()
    {
        lock (_lock) 
        { 
            IsPaused = true; 
        }
    }
    public void Resume()
    {
        lock (_lock)
        {
            IsPaused = false; 
        }
    }
    public virtual void Reset()
    {
        lock (_lock) 
        { 
            CurrentStep = 0; 
        }
    }
    public void GoToStep(int step)
    {
        lock (_lock) 
        { 
            CurrentStep = step; 
        }
    }
}