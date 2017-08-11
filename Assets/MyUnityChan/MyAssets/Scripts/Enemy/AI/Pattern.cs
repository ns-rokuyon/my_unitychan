using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public partial class AI {
        public partial class Def {
            public class Pattern {
                public static Def InputTowardPlayer(AIModel model) {
                    return AI.Def.Name("InputTowardPlayer")
                        .Keep(s => model.inputTowardPlayer(s.player));
                }

                public static Def InputHorizontalTowardPlayer(AIModel model) {
                    return AI.Def.Name("InputHorizontalTowardPlayer")
                        .Keep(s => model.inputHorizontalTowardPlayer(s.player));
                }

                public static Def InputVerticalTowardPlayer(AIModel model) {
                    return AI.Def.Name("InputVerticalTowardPlayer")
                        .Keep(s => model.inputVerticalTowardPlayer(s.player));
                }

                public static Def Hover(AIModel model, float delta_ground, float eps = 0.01f) {
                    return AI.Def.Name("Hover")
                        .Required(() => model.self.rigid_body.isKinematic)
                        .Keep(s => {
                            float dist_grouund = model.self.ground_checker.getDistance();
                            if ( delta_ground + eps < dist_grouund )
                                model.controller.inputVertical(-1.0f);
                            else if ( dist_grouund < delta_ground - eps )
                                model.controller.inputVertical(1.0f);
                        });
                }
            }
        }
    }
}
