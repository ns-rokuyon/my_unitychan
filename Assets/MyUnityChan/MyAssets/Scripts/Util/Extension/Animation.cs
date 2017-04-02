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
}
