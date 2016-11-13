using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class AIController : Controller {
        public bool isStopped = false;

        public virtual void Start() {
            // Common routines
            if ( self is ZakoAirTypeBase ) {
                // Keep inputs for flapping
                int flap_interval = (self as ZakoAirTypeBase).param.flap_interval;
                this.UpdateAsObservable().Select(x => x).Skip(flap_interval)
                    .FirstOrDefault()
                    .RepeatUntilDestroy(gameObject)
                    .Where(_ => (self as ZakoAirTypeBase).flight_level > self.transform.position.y)
                    .Subscribe(_ => {
                        inputs[(int)InputCode.JUMP] = true;
                    });
            }

            // Custom start routine
            if ( self is ICustomAIStart ) {
                (self as ICustomAIStart).customAIStart(this);
            }
        }

        // Update is called once per frame
        public virtual void Update() {
            if ( PauseManager.isPausing() ) isStopped = true;
            else isStopped = false;

            clearAllInputs();
        }

        public void inputKey(Controller.InputCode code, bool flag) {
            inputs[(int)code] = flag;
        }

        public void restart() {
            // Custom start routine
            if ( self is ICustomAIStart ) {
                (self as ICustomAIStart).customAIStart(this);
            }
        }
    }

    public interface ICustomAIStart {
        void customAIStart(AIController ai);
    }
}