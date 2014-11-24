using UnityEngine;
using System.Collections;

public class ObjectBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public class FrameCounter {
		private bool running = false;
		private int start_frame = 0;
		private int duration = 0;

		public FrameCounter(int frame){
			duration = frame;
			start_frame = Time.frameCount;
			running = true;
		}

		public void update(){
			if (running && Time.frameCount - start_frame > duration) {
				running = false;
			}
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
}
