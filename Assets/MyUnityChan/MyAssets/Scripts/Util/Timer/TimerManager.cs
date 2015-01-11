﻿using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class TimerManager : SingletonObjectBase<TimerManager> {

        // check timer is running or finish
        public static bool checkFinished(int id) {
            return self().finished(id);
        }

        public static bool checkRunning(int id) {
            return !checkFinished(id);
        }

        private Dictionary<int, Timer> timers;

        public GameObject frame_timer_prefab;

        // Use this for initialization
        void Start() {
            timers = new Dictionary<int, Timer>();
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
            timers.Add(id, timer_component);
        }

        public bool finished(int id) {
            if ( !timers.ContainsKey(id) ) {
                throw new System.InvalidOperationException("not exists: " + id);
            }

            if ( timers[id] == null || timers[id].finished() ) {
                return true;
            }
            return false;
        }

        void OnGUI() {
        }

    }

    public abstract class TimerState {
        protected int timer_id = 0;

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

        public abstract void createTimer(int time);
        public abstract void createTimer(float time);
    }
}