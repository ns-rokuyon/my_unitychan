using UnityEngine;
using System.Collections;

public class Hitbox : ObjectBase {
    protected int time = 0;                         // active time
    protected TimerState end_timer = null;
    public Vector3 forward { get; set; }

    void Awake() {
        end_timer = new FrameTimerState();
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        CommonUpdate();
        UniqueUpdate(); 
    }

    protected virtual void UniqueUpdate() {
    }

    private void CommonUpdate(){
        if ( end_timer == null ) {
            end_timer.createTimer(time);
        }
        else if ( end_timer.isFinished() ){
            Destroy(this.gameObject);
            return;
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            Debug.Log("hit");
        }
    }
}
