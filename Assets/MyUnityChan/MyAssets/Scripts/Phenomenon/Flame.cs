using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Flame : PhenomenonBase {
        private TimerState lifetime = null;
        private DamageObjectHitbox hitbox = null;

        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                frame = 9999;
            }

            public override void attack(Character character, Hitbox _hitbox) {
                GameObject owner = _hitbox.getOwner();
                if ( owner == null || owner == character.gameObject ) {
                    return;
                }
                character.damage(damage);
            }
        }


        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            if ( lifetime != null && lifetime.isFinished() ) {
                ObjectPoolManager.releaseGameObject(this.gameObject, Const.Prefab.Phenomenon["FLAME"]);
            }
        }

        public override void initialize() {
            createHitbox();
            lifetime = new FrameTimerState();
            lifetime.createTimer(180);
        }

        public override void finalize() {
        }

        private void createHitbox() {
            hitbox = HitboxManager.self().create<DamageObjectHitbox>(Const.Prefab.Hitbox["FLAME"], true);
            hitbox.ready(this.gameObject, new Spec());
        }

        public DamageObjectHitbox getHitbox() {
            return hitbox;
        }

    }
}
