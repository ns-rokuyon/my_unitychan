using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class AIController : Controller {
        public bool isStopped = false;

        public AIModel model { get; protected set; }
        public AI ai { get; protected set; }
        public Dictionary<int, AI> sub_ai { get; protected set; }

        public override void Start() {
            base.Start();

            model = self.GetComponent<AIModel>();
            if ( model ) {
                model.self = self as Enemy;
                model.controller = this;
                model.init();

                ai = model.define();
                ai.debug = model.debug;
                ai.build();

                sub_ai = model.defineSubAI();
                sub_ai.Values.ToList().ForEach(_ai => {
                    _ai.debug = model.debug;
                    _ai.build();
                });
            }
        }

        // Update is called once per frame
        public virtual void Update() {
            if ( PauseManager.isPausing() ) isStopped = true;
            else if ( TimelineManager.isPlaying ) isStopped = true;
            else isStopped = false;

            if ( sub_ai != null && sub_ai.Count > 0 ) {
                // When sub AIs are used, switch sub AIs
                int sub = model.current_routine;
                sub_ai.ToList().ForEach(kv => {
                    var i = kv.Key;
                    if ( i == sub )
                        kv.Value.freeze = false;
                    else
                        kv.Value.freeze = true;
                });
            }
        }

        public void restart() {
            if ( ai != null ) {
                ai.kill();
                ai = null;
            }
            if ( sub_ai != null && sub_ai.Keys.Count > 0 ) {
                sub_ai.Values.ToList().ForEach(_ai => {
                    _ai.kill();
                    _ai = null;
                });
                sub_ai = new Dictionary<int, AI>();
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