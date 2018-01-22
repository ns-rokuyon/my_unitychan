using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public partial class AI {
        public partial class Def {
            public class Pattern {
                public static Def InputTowardPlayer(AIModel model) {
                    return AI.Def.Name("InputTowardPlayer")
                        .Keep(s => model.inputTowardPlayer(s.player));
                }

                public static Def InputHorizontalTowardPlayer(AIModel model, float offset = 0.0f) {
                    return AI.Def.Name("InputHorizontalTowardPlayer")
                        .If(s => model.self.distanceXTo(s.player) >= offset)
                        .Then(s => model.inputHorizontalTowardPlayer(s.player));
                }

                public static Def InputVerticalTowardPlayer(AIModel model) {
                    return AI.Def.Name("InputVerticalTowardPlayer")
                        .Keep(s => model.inputVerticalTowardPlayer(s.player));
                }

                public static Def ChangeKinematicsSpeed(AIModel model, float vx, float vy) {
                    EnemyKinematics kinematics = model.self.action_manager.getAction<EnemyKinematics>("KINEMATICS");
                    return AI.Def.Name("ChangeKinematicsSpeed")
                        .Once()
                        .Do(s => {
                            kinematics.base_vx = vx;
                            kinematics.base_vy = vy;
                        });
                }

                public static Def IfLostPlayerThenSwitchTo(AIModel model, object r) {
                    return AI.Def.Name("IfLostPlayerThenSwitchTo")
                        .If(_ => model.self.searcher.lost)
                        .Then(_ => model.next_routine(r))
                        .Break();
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

        public class Routine {
            public int r { get; set; }              // Routine index
            public System.Action f { get; set; }

            public Routine(int _r, System.Action _f = null) {
                r = _r;
                f = _f;
            }

            public static Routine R(object r, System.Action f = null) {
                return new Routine((int)r, f);
            }
        }

        public class ProbRoutine : Routine {
            public float p { get; set; }            // Probability

            public ProbRoutine(int _r, float _p, System.Action _f = null) : base(_r, _f) {
                p = _p;
            }

            public static Routine R(int r, float p, System.Action f = null) {
                return new ProbRoutine(r, p, f);
            }
        }
    }
}
