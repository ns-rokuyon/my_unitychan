using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	protected Controller controller;

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
	
	}

	public class MoveLock : FrameCounter {
		public MoveLock(int frame) : base(frame){
		}

		public bool isLocked(){
			return base.isRunning();
		}
	};

	public abstract class DelayEvent : FrameCounter {
		protected bool done;

		public DelayEvent(int frame) : base(frame){}
		public abstract void perform();

		public bool isDone(){
			return done;
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

	public class MoveControlManager {
		private MoveLock lock_control;
		private DelayEvent delay_control;

		public MoveControlManager(){
		}

		public void register(MoveLock mlock) {
			lock_control = mlock;
		}

		public void register(DelayEvent devent) {
			delay_control = devent;
		}

		public void update(){
			if (lock_control != null && lock_control.isRunning()) {
				lock_control.update();
			}
			if (delay_control != null && !delay_control.isDone()) {
				delay_control.update();
				delay_control.perform();
			}
		}

		public bool isPlayerInputLocked(){
			if (lock_control == null) {
				return false;
			}
			return lock_control.isRunning();
		}
	}
}
