using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;

namespace MyUnityChan {
    [RequireComponent(typeof(SoundPlayer))]
    [RequireComponent(typeof(ChronosTimeControllable))]
    public abstract class EffectBase : PoolObjectBase {
        protected string resource_path = null;

        public override void initialize() {
            // Pausing handler
            var pss = GetComponentsInChildren<ParticleSystem>().Concat(GetComponents<ParticleSystem>()).ToList();
            if ( pss.Count() > 0 ) {
                time_control.onPausedFunctions.Add((paused) => {
                    if ( paused ) {
                        pss.ForEach(ps => ps.Pause());
                    }
                    else
                        pss.ForEach(ps => {
                            if ( ps.isPaused ) ps.Play();
                        });
                });
            }
        }

        public override void finalize() {
            time_control.onPausedFunctions.Clear();
            time_control.onPausedFunctions = null;
        }

        public virtual void enablePool(string _resource_path) {
            resource_path = _resource_path;
        }
    }
}