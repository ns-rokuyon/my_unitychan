using UnityEngine;
using System.Collections;

public class Invincible : ObjectBase {
    public bool all_time = false;

    private TimerContainer timer;
    private bool is_invincible;

    // Use this for initialization
    void Start() {
        timer = null;
        is_invincible = false;
    }

    // Update is called once per frame
    void Update() {
        if ( all_time ) {
            is_invincible = true;
            return;
        }

        if ( timer != null && TimerManager.checkRunning(timer.id) ) {
            is_invincible = true;
        }
        else {
            is_invincible = false;
            timer = null;
        }
    }

    public bool now() {
        return is_invincible;
    }

    public void enable(int frame) {
        timer = new TimerContainer(TimerManager.createFrameTimer(frame));
    }

    void OnGUI() {
        GUIStyle gui_style = new GUIStyle();
        GUIStyleState gui_stylestate = new GUIStyleState();
        gui_stylestate.textColor = Color.green;
        gui_style.normal = gui_stylestate;
        if ( timer == null ) {
            GUI.Label(new Rect(10, 30, 250, 30), "TimerManager.checkRunning: timer = null", gui_style);
            GUI.Label(new Rect(10, 50, 250, 30), "TimerManager.is_invincible: " + is_invincible, gui_style);
        }
        else {
            GUI.Label(new Rect(10, 30, 250, 30), "TimerManager.checkRunning: " + TimerManager.checkRunning(timer.id), gui_style);
            GUI.Label(new Rect(10, 50, 250, 30), "TimerManager.is_invincible: " + is_invincible, gui_style);
        }
    }
}
