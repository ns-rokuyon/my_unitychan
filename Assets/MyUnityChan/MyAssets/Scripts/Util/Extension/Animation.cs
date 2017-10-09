using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public static class AnimationExtension {
        public static void PlayAndNext(this Animation self, string anim1, string anim2) {
            self.Play(anim1);
            Observable.Timer(System.TimeSpan.FromSeconds(self.GetClip(anim1).length))
                .Subscribe(_ => {
                    self.Play(anim2);
                });
        }
    }


    public static class AnimatorExtension {
        public static bool isAnimState(this Animator self, string anim_name) {
            // anim_name is an animator state name starts with "Base Layer." instead of clip name
            AnimatorStateInfo anim_state = self.GetCurrentAnimatorStateInfo(0);
            return anim_state.fullPathHash == Animator.StringToHash(anim_name);
        }

    }
}
