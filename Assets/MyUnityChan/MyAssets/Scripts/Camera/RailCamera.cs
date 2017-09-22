using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class RailCamera : ObjectBase {
        public CameraPathAnimator animator;     // CameraPath
        public bool focus_player;
        public bool auto_discard;
        public int discard_interval_frame;

        public Player player {
            get {
                return GameStateManager.getPlayer();
            }
        }

        public override void OnEnable() {
            base.OnEnable();

            if ( animator ) {
                if ( focus_player )
                    animator.orientationTarget = player.bone_manager.bone(Const.ID.UnityChanBone.HEAD).transform;
                if ( auto_discard )
                    animator.AnimationFinishedEvent += Animator_AnimationFinishedEvent;
                animator.Play();
            }
        }

        private void Animator_AnimationFinishedEvent() {
            delay(discard_interval_frame, () => {
                gameObject.SetActive(false);
            });
        }
    }
}