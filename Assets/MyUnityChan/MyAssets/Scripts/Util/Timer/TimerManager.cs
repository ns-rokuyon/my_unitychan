using UnityEngine;
using System.Collections.Generic;

public class TimerManager : SingletonObjectBase<TimerManager> {

    // check timer is running or finish
    public static bool checkFinished(int id) {
        return self().finished(id);
    }

    public static bool checkRunning(int id) {
        return !checkFinished(id);
    }
    
    public static int createFrameTimer(int frame) {
        GameObject timer_object = Instantiate(self().frame_timer_prefab) as GameObject;
        FrameTimer timer = timer_object.GetComponent<FrameTimer>();
        timer.setTimer(frame);

        int id = timer_object.GetInstanceID();
        self().add(id, timer);

        return id;
    }

    private Dictionary<int,Timer> timers;

    public GameObject frame_timer_prefab;

    // Use this for initialization
    void Start() {
        timers = new Dictionary<int,Timer>();
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
        GUIStyle gui_style = new GUIStyle();
        GUIStyleState gui_stylestate = new GUIStyleState();
        gui_stylestate.textColor = Color.green;
        gui_style.normal = gui_stylestate;
        if ( timers != null ) {
            GUI.Label(new Rect(10, 70, 250, 30), "timers.keys.count: " + timers.Keys.Count, gui_style);
        }
    }

}

public class TimerContainer {
    public int id = 0;       // id

    public TimerContainer(int _id) {
        set(_id);
    }

    public void set(int _id) {
        id = _id;
    }
}