using UnityEngine;
using System.Collections;

public class Character : ObjectBase {

	protected Controller controller;

	public Controller getController(){
		return controller;
	}

    public virtual bool isTouchedWall() {
        // check character is in front of wall
        CapsuleCollider capsule_collider = GetComponent<CapsuleCollider>();
        return Physics.Raycast(transform.position + new Vector3(0, capsule_collider.bounds.size.y, 0), transform.forward, 2.0f) ||
            Physics.Raycast(capsule_collider.bounds.center, transform.forward, 2.0f);
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
