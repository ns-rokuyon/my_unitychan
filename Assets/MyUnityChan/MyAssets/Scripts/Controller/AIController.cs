using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class AIController : Controller {
        public bool isStopped = false;

        // Test
        public bool __new_ai_controller_test = false;
        public AI ai { get; set; }

        public virtual void Start() {
            AIModel model = self.GetComponent<AIModel>();
            if ( model ) {
                model.self = self as Enemy;
                model.controller = this;
                ai = model.build();
            }

            // Common routines
            if ( self is ZakoAirTypeBase ) {
                // Keep inputs for flapping
                int flap_interval = (self as ZakoAirTypeBase).param.flap_interval;
                this.UpdateAsObservable().Select(x => x).Skip(flap_interval)
                    .FirstOrDefault()
                    .RepeatUntilDestroy(gameObject)
                    .Where(_ => (self as ZakoAirTypeBase).flight_level > self.transform.position.y)
                    .Subscribe(_ => {
                        inputKey(InputCode.JUMP);
                    });
            }
        }

        // Update is called once per frame
        public virtual void Update() {
            if ( PauseManager.isPausing() ) isStopped = true;
            else isStopped = false;

            if ( __new_ai_controller_test ) {
                return;
            }
            clearAllInputs();
        }

        public void restart() {
        }

        public AI.State getObservedState() {
            AI.State state = new AI.State();
            state.player = GameStateManager.getPlayer();
            return state;
        }
    }

    public interface ICustomAIStart {
        void customAIStart(AIController ai);
    }
}