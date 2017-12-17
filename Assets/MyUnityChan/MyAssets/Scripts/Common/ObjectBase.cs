using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace MyUnityChan {
    public class ObjectBase : MonoBehaviour {

        public SoundPlayer sound {
            get; protected set;
        }

        private TimeControllable _tc;
        public TimeControllable time_control {
            get { return _tc ?? (_tc = GetComponent<TimeControllable>()); }
        }

        private RigidbodyWrapper _rb;
        public RigidbodyWrapper rigid_body {
            get {
                if ( !_rb ) {
                    if ( time_control && time_control is ChronosTimeControllable )
                        _rb = new ChronosRigidbodyWrapper(gameObject);
                    else
                        _rb = new RigidbodyWrapper(gameObject);
                }
                return _rb;
            }
        }

        private Dictionary<string, int> _frame_recorder = null;
        public Dictionary<string, int> frame_recorder {
            get { return _frame_recorder ?? (_frame_recorder = new Dictionary<string, int>()); }
        }

        private Dictionary<string, IDisposable> _disposables;
        public Dictionary<string, IDisposable> disposables {
            get { return _disposables ?? (_disposables = new Dictionary<string, IDisposable>()); }
        }

        public Area parent_area { get; set; }
        public bool managed_by_objectpool { get; set; }

        public virtual void OnEnable() {
            if ( sound )
                sound.locked = false;
        }

        public virtual void OnDisable() {
            //Debug.LogWarning(name + " in " + (parent_area ? parent_area.name : "somewhere") + " has just been disabled");
        }

        public void adjustZtoBaseline() {
            Area area = AreaManager.Instance.getAreaFromMemberObject(this.gameObject);
            if ( !area.isEmptyBaselineZ() ) {
                float z = area.getBaselineZ();
                this.gameObject.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y,
                    z
                    );
            }
        }

        public void delay(int frame, System.Action func, FrameCountType frame_count_type = FrameCountType.Update) {
            if ( frame <= 0 ) {
                func();
                return;
            }

            System.Func<int, FrameCountType, IObservable<long>> timer;
            if ( time_control )
                timer = time_control.PausableTimerFrame;
            else {
                timer = Observable.TimerFrame;
                DebugManager.warn(name + " has no TimeControllable component, so that delay() uses Observable.TimerFrame (fallback)");
            }

            timer(frame, frame_count_type).Subscribe(_ => func()).AddTo(this);
        }

        public void delay(string key, int frame, System.Action func, FrameCountType frame_count_type = FrameCountType.Update) {
            if ( frame <= 0 ) {
                func();
                return;
            }
            
            System.Func<int, FrameCountType, IObservable<long>> timer;
            if ( time_control )
                timer = time_control.PausableTimerFrame;
            else {
                timer = Observable.TimerFrame;
                DebugManager.warn(name + " has no TimeControllable component, so that delay() uses Observable.TimerFrame (fallback)");
            }

            IDisposable d = timer(frame, frame_count_type).Subscribe(_ => func()).AddTo(this);
            if ( disposables.ContainsKey(key) )
                disposables[key].Dispose();
            disposables[key] = d;
        }

        public void doPrevInterval(string key, int frame, System.Action func) {
            if ( !frame_recorder.ContainsKey(key) ) {
                frame_recorder.Add(key, Time.frameCount);
                func();
                return;
            }

            if ( Time.frameCount - frame_recorder[key] >= frame ) {
                frame_recorder[key] = Time.frameCount;
                func();
                return;
            }
        }

        public virtual bool assert() {
            return true;
        }

        protected void setupSoundPlayer() {
            sound = gameObject.GetComponent<SoundPlayer>();
        }

        public SoundPlayer getSoundPlayer() {
            return sound;
        }

        public void se(Const.ID.SE sid, bool playOneShot=true, int delay = 0) {
            if ( sound == null ) {
                DebugManager.log(name + " doesn't have SoundPlayer component", Const.Loglevel.WARN);
                return;
            }
            if ( parent_area && parent_area != AreaManager.self().getNowArea() )
                return;
            sound.play(sid, playOneShot, delay);
        }

        public string prefabPath(Const.ID.Controller name) {
            return Const.Prefab.Controller[name];
        }

        public string prefabPath(Const.ID.Effect name) {
            return Const.Prefab.Effect[name];
        }

        public string prefabPath(Const.ID.Item name) {
            return Const.Prefab.Item[name];
        }

        public IEnumerator waitFrame(int frame) {
            while ( frame > 0 ) {
                yield return null;
                frame--;
            }
        }

        public virtual void OnTriggerEnter(Collider other) {
        }

        public virtual void OnTriggerExit(Collider other) {
        }
    }
}
