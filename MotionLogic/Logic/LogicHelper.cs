using System;

/// <summary>
/// 逻辑辅助工具（非阻塞延时 + 边沿检测）
/// </summary>
public class LogicHelper
{
    private DateTime? _delayStart;
    private int _delayMs;
    private bool _lastSignal;

    /// <summary>
    /// 重置所有状态（延时和边沿）
    /// </summary>
    public void Reset()
    {
        _delayStart = null;
        _lastSignal = false;
    }

    /// <summary>
    /// 重置延时状态（可用于取消等待）
    /// </summary>
    public void ResetDelay()
    {
        _delayStart = null;
    }

    /// <summary>
    /// 重置边沿检测的初始状态，可指定初始值
    /// </summary>
    public void ResetEdge(bool initialValue = false)
    {
        _lastSignal = initialValue;
    }

    /// <summary>
    /// 非阻塞延时，返回 true 表示延时完成
    /// </summary>
    public bool Delay(int milliseconds)
    {
        if (!_delayStart.HasValue)
        {
            _delayStart = DateTime.Now;
            _delayMs = milliseconds;
            return false;
        }

        if ((DateTime.Now - _delayStart.Value).TotalMilliseconds >= _delayMs)
        {
            _delayStart = null;  // 完成后自动重置，下次调用将重新计时
            return true;
        }
        return false;
    }

    /// <summary>
    /// 上升沿检测（从 false → true）
    /// </summary>
    public bool RisingEdge(bool current)
    {
        bool result = current && !_lastSignal;
        _lastSignal = current;
        return result;
    }

    /// <summary>
    /// 下降沿检测（从 true → false）
    /// </summary>
    public bool FallingEdge(bool current)
    {
        bool result = !current && _lastSignal;
        _lastSignal = current;
        return result;
    }
}