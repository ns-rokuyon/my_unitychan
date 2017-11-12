using UnityEngine;
using System.Collections;
using System.Linq;

namespace MyUnityChan {
    public abstract class DamageObjectBase : PoolObjectBase {

        public bool has_hitbox_in_children;

        public virtual void Awake() {
            commonSetting();
        }

        public virtual void setStartPosition(Vector3 pos) {
            transform.position = pos;
        }

        public void commonSetting() {
            int ch_layer = LayerMask.NameToLayer("Character");
            int do_layer = LayerMask.NameToLayer("DamageObject");

            Physics.IgnoreLayerCollision(ch_layer, do_layer);

            // Pausing handler
            var pss = GetComponentsInChildren<ParticleSystem>().Concat(GetComponents<ParticleSystem>()).ToList();
            if ( pss.Count() > 0 ) {
                time_control.onPausedFunctions.Add((paused) => {
                    if ( paused )
                        pss.ForEach(ps => ps.Pause());
                    else
                        pss.ForEach(ps => {
                            if ( ps.isPaused ) ps.Play();
                        });
                });
            }
        }
    }
}
