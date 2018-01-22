using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public partial class AI : StructBase {
        public Character self { get; set; }
        public AIController controller { get; set; }
        public List<Def> patterns { get; set; }
        public bool freeze { get; set; }
        public bool debug { get; set; }     // No used
        public AIDebugger debugger { get; set; }

        protected AI(Character ch, AIController cont) {
            self = ch;
            controller = cont;
            patterns = new List<Def>();
        }

        public static AI root(Character _self, AIController _controller) {
            AI ai = new AI(_self, _controller);
            // Add common patterns
            ai.def(AI.Def.Name("Area checker")
                   .StopIf(s => s.model.ai.self.getAreaName() != s.player.getAreaName()))
              .def(AI.Def.Name("Locked input")
                   .StopIf(s => s.model.ai.self.isInputLocked()));
            if ( _self is ZakoAirTypeBase ) {
                ai.def(AI.Def.Name("Flap")
                       .Interval((_self as ZakoAirTypeBase).param.flap_interval)
                       .If(s => s.model.ai.self.getHP() > 0)
                       .If(_ => !_self.isHitRoof() && (_self as ZakoAirTypeBase).flight_level > _self.transform.position.y)
                       .Then(_ => _controller.inputKey(Controller.InputCode.JUMP)));
            }
            return ai;
        }

        public AI def(Def def) {
            patterns.Add(def);
            return this;
        }


        public void reset() {
            patterns.ForEach(def => def.reset());
        }

        public void think(State state) {
            state.flagstack.Clear();

            if ( debugger ) {
                debugger.pushLog("---");
            }
            foreach ( var def in patterns ) {
                if ( debugger ) {
                    debugger.pushLog(this.GetHashCode() + ": " + def.name);
                }

                bool flag = false;
                var checks = def.conditions.Select(cond => cond.check(state));
                if ( !checks.Contains(false) )
                    flag = true;

                state.flagstack.Add(flag);

                if ( flag ) {
                    if ( def.true_behavior != null )
                        def.true_behavior.act(state);
                }
                else {
                    if ( def.false_behavior != null )
                        def.false_behavior.act(state);
                }

                if ( def.break_condition != null && def.break_condition.check(state) ) {
                    state.message.stop = true;
                }

                if ( state.message.stop ) {
                    // If message.stop in state was changed to true,
                    // stop thinking loop
                    break;
                }
            }

            if ( debug ) printFlagStack(state);
        }

        public void printFlagStack(State state) {
            DebugManager.log("printFlagStack");
            for ( int i = 0; i < state.flagstack.Count; i++ ) {
                bool f = state.flagstack[i];
                DebugManager.log("[AI<" + self.name + ">] name: '" + patterns[i].name + "', judge=" + f);
            }
        }

        // Observable state structure for AI
        public class State {
            public Player player { get; set; }
            public Message message { get; set; }
            public AIModel model { get; set; }
            public List<bool> flagstack { get; set; }

            public State() {
                message = new Message();
                flagstack = new List<bool>();
            }
        }

        public class Message {
            public bool stop { get; set; }
        }
    }
}