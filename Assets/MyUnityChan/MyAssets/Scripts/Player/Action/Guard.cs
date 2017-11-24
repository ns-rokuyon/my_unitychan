using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class PlayerGuard : PlayerAction {
        public bool guarding { get; private set; }
        public int guard_hp { get; private set; }

        private GameObject shield_effect_obj;
        private ParticleEffect shield_effect;
        private IDisposable recoverer;
        private bool starting_recoverer;
        private Color? default_color = null;

        public bool broken {
            get {
                return guard_hp == 0;
            }
        }

        public Color? color_level {
            get {
                if ( default_color == null )
                    return Color.white;
                Color? d = default_color / (float)Const.Max.GUARD_HP;
                return Color.black + (d * guard_hp);
            }
        }

        public PlayerGuard(Character character)
            : base(character) {
            guarding = false;
            flag = null;
            guard_hp = Const.Max.GUARD_HP;

            player.time_control.PausableEveryUpdate()
                  .ThrottleFrame(10)
                  .Where(_ => shield_effect && color_level != null)
                  .Subscribe(_ => {
                      var m = shield_effect.particle_system.main;
                      m.startColor = (Color)color_level;
                  });
        }

        public override string name() {
            return "GUARD";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.GUARD;
        }

        public override bool condition() {
            return controller.keyGuard() && !player.is_rolling;
        }

        public override void perform() {
            if ( !guarding ) {
                guarding = true;
                player.getAnimator().SetTrigger("GuardStart");
                shield_effect_obj = EffectManager.createEffect(Const.ID.Effect.SHIELD_01,
                                                               player.gameObject,
                                                               0, 0.6f,
                                                               Const.INF_FRAME,
                                                               true);
                shield_effect = shield_effect_obj.GetComponent<ParticleEffect>();
                if ( shield_effect != null && default_color == null ) {
                    default_color = shield_effect.particle_system.main.startColor.color;
                }
            }
        }

        public override void off_perform() {
            if ( guarding ) {
                if ( !player.is_rolling ) {
                    player.getAnimator().CrossFade("Locomotion", 0.6f);
                }
                cancel();
            }
        }

        public void cancel() {
            if ( !guarding )
                return;

            guarding = false;
            if ( shield_effect ) {
                ObjectPoolManager.releaseGameObject(shield_effect_obj,
                                                    Const.Prefab.Effect[Const.ID.Effect.SHIELD_01]);
                shield_effect_obj = null;
                shield_effect = null;

                starting_recoverer = true;
                player.delay(60, () => {
                    starting_recoverer = false;
                    recoverer = player.time_control
                        .PausableEveryUpdate()
                        .Subscribe(_ => {
                            guard_hp += 1;
                            if ( guard_hp >= Const.Max.GUARD_HP ) {
                                guard_hp = Const.Max.GUARD_HP;
                                closeRecoverer();
                            }
                        });
                });
            }
        }

        public void damage(int dam) {
            guard_hp -= dam;
            if ( guard_hp < 0 )
                guard_hp = 0;

            if ( starting_recoverer )
                return;

            closeRecoverer();
        }

        public void closeRecoverer() {
            if ( recoverer != null ) {
                recoverer.Dispose();
                recoverer = null;
            }
        }

    }
}
