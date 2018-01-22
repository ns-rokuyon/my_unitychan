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

        public override void Awake() {
            base.Awake();
        }

        public override void Start() {
            base.Start();

            model = self.GetComponent<AIModel>();
            if ( model ) {
                model.self = self as Enemy;
                model.controller = this;
                model.init();
                model.build();
            }
        }

        // Update is called once per frame
        public virtual void Update() {
            if ( PauseManager.isPausing() ) isStopped = true;
            else if ( TimelineManager.isPlaying ) isStopped = true;
            else isStopped = false;
        }

        public void restart() {
            if ( model ) {
                model.kill();
            }

            Start();
        }

        public AI.State getObservedState() {
            AI.State state = new AI.State();
            state.player = GameStateManager.getPlayer();
            state.model = model;
            return state;
        }
    }
}