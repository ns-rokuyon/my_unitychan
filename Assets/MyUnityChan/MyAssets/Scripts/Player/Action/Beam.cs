using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerBeam : PlayerAction {
        public AttackSpec spec = null;
        public static readonly string hitbox_resource_path = Const.Prefab.Hitbox["BEAM"];

        private string left_shoulder_path = 
            "Character1_Reference/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_LeftShoulder/";
        private Transform left_arm;
        private Transform left_fore_arm;
        private Transform left_hand;

        private int beam_interval = 20;
        private TimerState timer = null;
        
        public PlayerBeam(Character character)
            : base(character) {
            spec = new Spec();
            left_arm = player.transform.Find(left_shoulder_path + "Character1_LeftArm");
            left_fore_arm = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm");
            left_hand = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm/Character1_LeftHand");

            timer = new FrameTimerState();
        }

        public override string name() {
            return "BEAM";
        }

        public class Spec : AttackSpec {
            public Spec() {
                damage = 20;
                stun = 50;
                frame = 9999;
            }

            public override void attack(Character character, Hitbox hitbox) {
                ((Enemy)character).stun(stun);
                ((Enemy)character).damage(damage);
                EffectManager.self().createEffect(Const.Prefab.Effect["HIT_02"],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        public override bool condition() {
            return controller.keyTest();
        }

        public override void perform() {
            if ( timer != null && timer.isFinished() ) {
                Vector3 fw = player.transform.forward;
                //InvokerManager.createFrameDelayVector3Invoker(0, fw, shootProjectile);
                shootProjectile(fw);
                timer.createTimer(beam_interval);
            }
        }

        // play beam-shot motion
        public override void performLate() {
            // hand up to front
            if ( player.isLookAhead() ) {
                left_arm.rotation = Quaternion.AngleAxis(180.0f, new Vector3(0, 0, 1));
                left_fore_arm.localRotation = Quaternion.AngleAxis(-90.0f, new Vector3(1, 0, 0));
                left_hand.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
            else {
                left_arm.rotation = Quaternion.AngleAxis(0.0f, new Vector3(0, 0, 1));
                left_fore_arm.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0));
                left_hand.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
        }

        public void shootProjectile(Vector3 direction) {
            GameObject beam = ObjectPoolManager.getGameObject(Beam.resource_path);
            beam.setParent(Hierarchy.Layout.PROJECTILE);

            Beam prjc = beam.GetComponent<Beam>();
            prjc.setDir(direction);
            prjc.setStartPosition(player.transform.position);
            prjc.setPlayerInfo(player);

            // hitbox
            HitboxManager.self().create<ProjectileHitbox>(hitbox_resource_path, use_objectpool:true).ready(beam, spec);
        }

    }
}