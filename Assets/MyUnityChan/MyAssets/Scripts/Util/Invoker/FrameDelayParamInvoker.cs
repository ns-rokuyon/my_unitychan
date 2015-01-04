using UnityEngine;
using System.Collections;

public class FrameDelayParamInvoker<T> : Invoker where T : struct {
    protected FrameTimerState timer;

    public delegate void DelayParamDelegate(T param);
    private DelayParamDelegate delay_func;
    private T param;

    void Awake() {
        timer = new FrameTimerState();
    }

    public void set(int frame, T p, DelayParamDelegate func) {
        delay_func = func;
        param = p;
        timer.createTimer(frame);
    }

    protected override void invoke() {
        delay_func(param);
    }

    protected override bool schedule() {
        return timer.isFinished();
    }
}