using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class StayAndJumpAI : AIController {
        private GameObject target;

        // Use this for initialization
        void Start() {
            target = GameStateManager.getPlayerObject();

            this.UpdateAsObservable()
                .Select(_ => Mathf.Abs(target.transform.position.x - self.transform.position.x))
                .Select(dx => dx < 4.0f)
                .Subscribe(b => inputs[(int)InputCode.JUMP] = b);
        }

    }
}