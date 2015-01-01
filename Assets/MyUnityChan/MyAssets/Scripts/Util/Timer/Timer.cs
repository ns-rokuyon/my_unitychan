using UnityEngine;
using System.Collections;

public class Timer : ObjectBase {
    protected bool running = false;

    public bool isRunning(){
        return running;
    }

    public bool finished() {
        return !running;
    }

    public void destroy() {
        Destroy(this.gameObject);
    }
}
