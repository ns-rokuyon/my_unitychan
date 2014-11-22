using UnityEngine;
using System.Collections;

public class Zako_1 : Enemy {
    private EnemyActionManager action_manager;

	// Use this for initialization
	void Start () {
        loadAttachedAI();
        action_manager = new EnemyActionManager();
        action_manager.registerAction("WALK", new EnemyWalk(this));
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate() {
        action_manager.act("WALK");
    }
}

public class EnemyWalk : EnemyActionBase {
    private float maxspeed = 2.0f;
    private Vector3 moveF = new Vector3(100f, 0, 0);

    public EnemyWalk(Character character)
        : base(character) {
    }

    public override void perform(Character character) {
        float horizontal = controller.keyHorizontal();
        Vector3 fw = enemy.transform.forward;

        // accelerate
        if (!enemy.isTouchedWall()) {
            enemy.rigidbody.AddForce(horizontal * moveF);
        }

        float vx = enemy.rigidbody.velocity.x;
        float vy = enemy.rigidbody.velocity.y;
        if ( Mathf.Abs(vx) > maxspeed ) {
            enemy.rigidbody.velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
        }
    }
}