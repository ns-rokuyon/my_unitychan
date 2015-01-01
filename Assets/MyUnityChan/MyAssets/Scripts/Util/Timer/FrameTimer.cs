using UnityEngine;
using System.Collections;

public class FrameTimer : Timer {
    private int start_frame = 0;
    private int count = 0;
    private int duration = 0;

    public void setTimer(int time){
        start_frame = Time.frameCount;
        duration = time;
        running = true;
    }

    public int now() {
        return count;
    }

    // Use this for initialization
    void Start() {
        running = true;
    }

    // Update is called once per frame
    void Update() {
        if ( running ) {
            count++;
            if ( count >= duration ) {
                running = false;
            }
        }
    }
}
