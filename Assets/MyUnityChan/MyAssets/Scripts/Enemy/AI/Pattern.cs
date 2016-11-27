using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public partial class AI {
        public partial class Def {
            public class Pattern {
                public static Def InputHorizontalTowardPlayer(AIModel model) {
                    return AI.Def.Name("InputHorizontalTowardPlayer")
                        .Keep(s => model.inputHorizontalTowardPlayer(s.player));
                }
            }
        }
    }
}
