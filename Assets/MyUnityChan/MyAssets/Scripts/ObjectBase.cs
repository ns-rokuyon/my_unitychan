using UnityEngine;
using System.Collections;

public class ObjectBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "MovingFloor" && transform.parent == null) {
            transform.parent = other.gameObject.transform;
        }
    }

    public void OnTriggerExit(Collider other) {
        if ( other.tag == "MovingFloor" && transform.parent != null ) {
            transform.parent = null;
        }
    }

    // manual frame counter
    // call update() per frame manually
	public class FrameCounter {
		private bool running = false;
		private int start_frame = 0;
        private int count = 0;
		private int duration = 0;

		public FrameCounter(int frame){
			duration = frame;
			start_frame = Time.frameCount;
			running = true;
            count = 0;
		}

		public void update(){
            if ( running ) {
                count++;
                if ( count >= duration ) {
                    running = false;
                }
            }
		}

        public int now() {
            return count;
        }

		public bool isRunning(){
			return running;
		}

		public bool finished(){
			return !running;
		}

        public static FrameCounter startFrameCounter(int frame) {
            return new FrameCounter(frame);
        }
	
	}

	public abstract class DelayEvent : FrameCounter {
		protected bool done;

		public DelayEvent(int frame) : base(frame){}
		public abstract void perform();

		public bool isDone(){
			return done;
		}
	}

    // manual invoker
	public class DelayNormalEvent : DelayEvent {
		public delegate void DelayDelegate();

		private DelayDelegate delay_func;

		public DelayNormalEvent(int frame, DelayDelegate func) : base(frame){
			delay_func = func;
		}

		public override void perform(){
			if (!done && finished()) {
				delay_func();
				done = true;
			}
		}
	}

	public class DelayDirectionEvent : DelayEvent {
		public delegate void DelayDelegate(Vector3 dir);

		private DelayDelegate delay_func;
		private Vector3 direction;

		public DelayDirectionEvent(int frame, Vector3 dir, DelayDelegate func) : base(frame){
			delay_func = func;
			direction = dir;
		}

		public override void perform(){
			if (!done && finished()) {
				delay_func(direction);
				done = true;
			}
		}
	}
}
