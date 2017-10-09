using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyKinematics : EnemyActionBase {
        public float base_vx { get; set; }
        public float base_vy { get; set; }

        public float vx {
            get { return base_vx * controller.keyHorizontal(); }
        }

        public float vy {
            get { return base_vy * controller.keyVertical(); }
        }

        public EnemyKinematics(Character character, float _vx = 1.0f, float _vy = 1.0f) : base(character) {
            base_vx = _vx;
            base_vy = _vy;
        }

        public override void performFixed() {
            float x = enemy.transform.position.x + vx * enemy.time_control.deltaTime;
            float y = enemy.transform.position.y + vy * enemy.time_control.deltaTime;
            float z = enemy.transform.position.z;
            enemy.transform.position = new Vector3(x, y, z);
        }

        public override bool condition() {
            return enemy.rigid_body.isKinematic &&
                (Mathf.Abs(controller.keyHorizontal()) > 0.01f || Mathf.Abs(controller.keyVertical()) > 0.01f) &&
                !enemy.isFlinching();
        }

        public override string name() {
            return "KINEMATICS";
        }
    }
}