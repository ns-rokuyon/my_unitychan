using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class TimerManager : PrefabManagerBase<TimerManager> {
        private Dictionary<int, Timer> timers;
        
        public static TimerManager self() {
            return Instance.GetComponent<TimerManager>();
        }

        // check timer is running or finish
        public static bool checkFinished(int id) {
            return (self() as TimerManager).finished(id);
        }

        public static bool checkRunning(int id) {
            return !checkFinished(id);
        }

        public static Timer get(int id) {
            return (self() as TimerManager).timers[id];
        }

        public static void destroy(int id) {
            Timer timer = (self() as TimerManager).timers[id];
            if ( timer ) {
                timer.destroy();
                self().timers[id] = null;
            }
        }

        public static int getPooledObjectIndex(GameObject go, string resource_path) {
            return ObjectPoolManager.getObjectIndex(go, resource_path);
        }

        public override T create<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T timer = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.TIMER).GetComponent<T>();
                (timer as Timer).enablePool(resource_path);
                return timer;
            }
            return instantiatePrefab(resource_path, Hierarchy.Layout.TIMER).GetComponent<T>();
        }

        void Awake() {
            prefabs = new Dictionary<string, GameObject>();
            timers = new Dictionary<int, Timer>();
        }

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            List<int> keys = new List<int>(timers.Keys);

            foreach ( int id in keys ) {
                if ( timers[id] != null && timers[id].finished() ) {
                    timers[id].destroy();
                    timers[id] = null;
                }
            }
        }

        public void add(int id, Timer timer_component) {
            if ( timers.ContainsKey(id) ) {
                timers[id] = timer_component;
            }
            else {
                timers.Add(id, timer_component);
            }
        }

        public int add(Timer timer_component) {
            int id = timers.Count;
            timers.Add(id, timer_component);
            return id;
        }


        public bool finished(int id) {
            if ( !timers.ContainsKey(id) ) {
                throw new System.InvalidOperationException("not exists: " + id);
            }

            if ( timers[id] == null || !timers[id].gameObject.activeSelf || timers[id].finished() ) {
                return true;
            }
            return false;
        }

        void OnGUI() {
        }

    }

    public abstract class TimerState {
        public int timer_id = 0;

        public TimerState() {
            createTimer(1);
        }

        public int id() {
            return timer_id;
        }

        public bool isFinished() {
            return TimerManager.checkFinished(timer_id);
        }

        public bool isRunning() {
            return TimerManager.checkRunning(timer_id);
        }

        public Timer getTimer() {
            return TimerManager.get(timer_id);
        }

        public void destroy() {
            if ( isRunning() ) {
                TimerManager.destroy(timer_id);
            }
        }

        public abstract void createTimer(int time);
        public abstract void createTimer(float time);
    }
}