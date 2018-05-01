using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace MyUnityChan {
    public class TimeControllable : ObjectBase {
        protected List<System.Action<bool>> _onpaused_functions;
        protected System.IDisposable onpause_caller;
        public List<System.Action<bool>> onPausedFunctions {
            get {
                if ( _onpaused_functions == null ) {
                    _onpaused_functions = new List<System.Action<bool>>();
                    if ( onpause_caller != null ) {
                        onpause_caller.Dispose();
                        onpause_caller = null;
                    }

                    onpause_caller = this.ObserveEveryValueChanged(_ => paused)
                        .Subscribe(_paused => {
                            if ( _onpaused_functions == null )
                                return;
                            _onpaused_functions.ForEach(func => func(_paused));
                        }).
                        AddTo(this);
                }
                return _onpaused_functions;
            }
            set {
                _onpaused_functions = value;
            }
        }

        public virtual bool paused {
            get {
                return Time.timeScale == 0.0f;
            }
        }

        public virtual float deltaTime {
            get {
                return Time.deltaTime;
            }
        }

        public virtual string clockName {
            get {
                return "Time";
            }
        }

        public virtual void changeClock(string clock_name) {
        }

        // Return IntervalFrame based on TimeControllable
        // PausableIntervalFrame does not count a frame when paused == true.
        public IObservable<long> PausableIntervalFrame(int intervalFrameCount, FrameCountType frameCountType = FrameCountType.Update) {
            return PausableTimerFrame(intervalFrameCount, intervalFrameCount, frameCountType);
        }

        // Return TimerFrame based on TimeControllable.
        // PausableTimerFrame does not count a frame when paused == true.
        public IObservable<long> PausableTimerFrame(int dueTimeFrameCount, FrameCountType frameCountType = FrameCountType.Update) {
            return Observable.FromMicroCoroutine<long>((observer, cancellation) => PausableTimerFrameCore(observer, dueTimeFrameCount, cancellation), frameCountType);
        }

        public IObservable<long> PausableTimerFrame(int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType = FrameCountType.Update) {
            return Observable.FromMicroCoroutine<long>((observer, cancellation) => PausableTimerFrameCore(observer, dueTimeFrameCount, periodFrameCount, cancellation), frameCountType);
        }

        protected IEnumerator PausableTimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, CancellationToken cancel) {
            // normalize
            if ( dueTimeFrameCount <= 0 ) dueTimeFrameCount = 0;

            var currentFrame = 0;

            // initial phase
            while ( !cancel.IsCancellationRequested ) {
                if ( !paused && currentFrame++ == dueTimeFrameCount ) {
                    observer.OnNext(0);
                    observer.OnCompleted();
                    break;
                }
                yield return null;
            }
        }

        protected IEnumerator PausableTimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, int periodFrameCount, CancellationToken cancel) {
            // normalize
            if (dueTimeFrameCount <= 0) dueTimeFrameCount = 0;
            if (periodFrameCount <= 0) periodFrameCount = 1;

            var sendCount = 0L;
            var currentFrame = 0;

            // initial phase
            while ( !cancel.IsCancellationRequested ) {
                if ( !paused && currentFrame++ == dueTimeFrameCount ) {
                    observer.OnNext(sendCount++);
                    currentFrame = -1;
                    break;
                }
                yield return null;
            }

            // period phase
            while ( !cancel.IsCancellationRequested ) {
                if ( !paused && ++currentFrame == periodFrameCount ) {
                    observer.OnNext(sendCount++);
                    currentFrame = 0;
                }
                yield return null;
            }
        }

        // Return EveryUpdate based on TimeControllable.
        // PausableEveryUpdate does not count a frame when paused == true.
        public IObservable<long> PausableEveryUpdate() {
            return Observable.FromMicroCoroutine<long>((observer, cancellationToken) => PausableEveryCycleCore(observer, cancellationToken), FrameCountType.Update);
        }

        public IEnumerator PausableEveryCycleCore(IObserver<long> observer, CancellationToken cancellationToken) {
            if ( cancellationToken.IsCancellationRequested ) yield break;
            var count = 0L;
            while ( true ) {
                yield return null;
                if ( cancellationToken.IsCancellationRequested ) yield break;

                if ( !paused )
                    observer.OnNext(count++);
            }
        }
    }
}
