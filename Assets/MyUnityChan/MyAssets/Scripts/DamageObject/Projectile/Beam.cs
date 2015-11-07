using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Beam : Projectile  {
        public static readonly string resource_path = Const.Prefab.Projectile["BEAM"];

        public class Spec : AttackSpec {
            public Spec() {
                damage = 20;
                stun = 50;
                frame = 9999;
            }

            public override void attack(Character character, Hitbox hitbox) {
                character.stun(stun);
                character.damage(damage);
                EffectManager.self().createEffect(Const.Prefab.Effect["HIT_02"],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            projectileCommonUpdate(resource_path);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos + target_dir * 0.4f + Vector3.up * 1.2f;
            start_position = transform.position;

            projectileCommonSetStartPosition();
        }

        public override void initialize() {
            penetration = false;
            distance_moved = 0.0f;
            max_range = 40.0f;
            speed = 0.2f;
            hit_num = 0;
        }

        public override void finalize() {
        }
    }

}