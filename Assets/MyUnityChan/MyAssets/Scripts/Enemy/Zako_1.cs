using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Zako_1 : Enemy {

        protected override void start() {
            GameObject action_manager_inst = Instantiate(enemy_action_manager_prefab) as GameObject;
            action_manager = action_manager_inst.GetComponent<EnemyActionManager>();

            action_manager.registerAction(new EnemyWalk(this));
        }

        protected override void update() {
        }

        void FixedUpdate() {
        }
    }

    public class EnemyWalk : EnemyActionBase {
        private float maxspeed = 1.0f;
        private Vector3 moveF = new Vector3(20f, 0, 0);

        public override string name() {
            return "WALK";
        }

        public EnemyWalk(Character character)
            : base(character) {
        }

        public override void performFixed() {
            float horizontal = controller.keyHorizontal();
            Vector3 fw = enemy.transform.forward;

            // accelerate
            if ( !enemy.isTouchedWall() ) {
                enemy.GetComponent<Rigidbody>().AddForce(horizontal * moveF);
            }

            float vx = enemy.GetComponent<Rigidbody>().velocity.x;
            float vy = enemy.GetComponent<Rigidbody>().velocity.y;
            if ( Mathf.Abs(vx) > maxspeed ) {
                enemy.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
            }
        }

        public override bool condition() {
            return !enemy.isStunned();
        }
    }
}