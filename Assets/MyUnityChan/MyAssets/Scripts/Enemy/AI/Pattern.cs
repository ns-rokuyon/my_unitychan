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

                // Switching random routines
                public static Def RandomRoutines(AIModel model, int interval, params ProbRoutine[] routines) {
                    int nRoutines = routines.Length;
                    List<float> rs = new List<float>();
                    routines.ToList().ForEach(routine => {
                        // Set boundary
                        if ( rs.Count == 0 ) {
                            rs.Add(routine.p);
                        } else {
                            rs.Add(rs.Last() + routine.p);
                        }

                        // Fix edge
                        if ( rs.Count == nRoutines ) {
                            rs[rs.Count - 1] = 1.0f;
                        }
                    });
                    return AI.Def.Name("RandomRoutines").Keep(s => {
                        float p = UnityEngine.Random.Range(0.0f, 1.0f);

                        // Switch routine
                        model.current_routine = routines[rs.FindIndex(bp => p < bp)].r;

                        // Reset states
                        model.controller.ai.reset();
                        model.controller.sub_ai.Values.ToList().ForEach(ai => ai.reset());
                    })
                    .Interval(interval);
                }
                
                // Switching routines sequentially
                public static Def RotateRoutines(AIModel model, int interval, params Routine[] routines) {
                    int nRoutines = routines.Length;
                    int i = nRoutines - 1;
                    return AI.Def.Name("RotateRoutines").Do(s => {
                        i++;
                        if ( i >= nRoutines )
                            i = 0;

                        // Switch routine
                        model.current_routine = routines[i].r;

                        // Reset states
                        model.controller.ai.reset();
                        model.controller.sub_ai.Values.ToList().ForEach(ai => ai.reset());
                    })
                    .Interval(interval);
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

            public static Routine R(int r, System.Action f = null) {
                return new Routine(r, f);
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
