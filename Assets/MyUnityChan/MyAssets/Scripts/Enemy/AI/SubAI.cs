using UnityEngine;
using System.Collections;


namespace MyUnityChan {
    public class SubAI : AI {
        public int routine_id { get; private set; }

        private SubAI(int _routine_id, AI main_ai) : base(main_ai.self, main_ai.controller) {
            routine_id = _routine_id;
        }

        public static SubAI root(object routine_id, AI main_ai) {
            SubAI ai = new SubAI((int)routine_id, main_ai);

            // Add common patterns
            ai.def(AI.Def.Name("SubAI check (" + ai.routine_id + ")")
                   .StopIf(_ => {
                       if ( main_ai.debugger != null )
                           main_ai.debugger.pushLog("SubAI(" + ai.routine_id + "): current=" + (main_ai.controller.model.current_routine != ai.routine_id).ToString() + ", main_ai.freeze=" + main_ai.freeze);
                       return main_ai.controller.model.current_routine != ai.routine_id || main_ai.freeze;
                   }));
            return ai;
        }

        public new SubAI def(Def def) {
            patterns.Add(def);
            return this;
        }
    }
}