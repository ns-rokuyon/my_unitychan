using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class StayAndJumpAI : AIController {
        private GameObject target;

        public int jump_interval { get; set; }
        public int wait_sec { get; set; }

        // Use this for initialization
        public override void Start() {
            base.Start();
            wait_sec = 0;

            target = GameStateManager.getPlayerObject();

            // Jump
            this.UpdateAsObservable()
                .Where(_ => wait_sec == 0)
                .Select(_ => Mathf.Abs(target.transform.position.x - self.transform.position.x))
                .Select(dx => dx < 4.0f)
                .Subscribe(b => {
                    inputs[(int)InputCode.JUMP] = b;
                    if ( b ) wait_sec = jump_interval;
                });

            // Count
            this.UpdateAsObservable()
                .Where(_ => this.wait_sec > 0)
                .Subscribe(_ => wait_sec--);

            // Flip
            this.UpdateAsObservable()
                .Subscribe(_ => (self as NPCharacter).faceToPlayer());
        }

    }
}