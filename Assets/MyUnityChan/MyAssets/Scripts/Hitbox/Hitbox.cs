using UnityEngine;
using System.Collections;

public class Hitbox : ObjectBase {
    protected int time = 0;                         // active time
    protected FrameCounter end_timer = null;
    public Vector3 forward { get; set; }

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
            end_timer = FrameCounter.startFrameCounter(time);
        }
        else if ( end_timer.finished() ){
            Destroy(this.gameObject);
            return;
        }

        if ( end_timer != null ) {
            end_timer.update();
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            Debug.Log("hit");
        }
    }
}
