using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class AIController : Controller {
        public bool isStopped = false;

        public AI ai { get; set; }

        public virtual void Start() {
            AIModel model = self.GetComponent<AIModel>();
            if ( model ) {
                model.self = self as Enemy;
                model.controller = this;
                model.init();

                ai = model.define();
                if ( model.debug )
                    ai.debug = true;
                ai.build();
            }
        }

        // Update is called once per frame
        public virtual void Update() {
            if ( PauseManager.isPausing() ) isStopped = true;
            else isStopped = false;

            //clearAllInputs();
        }

        public void restart() {
            if ( ai != null ) {
                ai.kill();
                ai = null;
            }
            Start();
        }

        public AI.State getObservedState() {
            AI.State state = new AI.State();
            state.player = GameStateManager.getPlayer();
            return state;
        }
    }
}