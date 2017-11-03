using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class ShootableObject : DamageObjectBase {
        public ShooterBase shooter { get; protected set; }
        public float waiting_time_for_destroying = 0.0f;

        public bool waiting_for_destroying { get; protected set; }
        protected delegate void SetEnabledToComponent(bool f);
        protected List<SetEnabledToComponent> component_to_disable_in_waiting 
            = new List<SetEnabledToComponent>();

        public void linkShooter(ShooterBase sh) {
            shooter = sh;
        }

        protected IEnumerator destroy(string resource_path) {
            if ( waiting_time_for_destroying > 0.0f ) {
                if ( rigid_body ) rigid_body.velocity = Vector3.zero;
                foreach ( var component_enabler in component_to_disable_in_waiting ) {
                    component_enabler(false);
                }
                waiting_for_destroying = true;
                yield return new WaitForSeconds(waiting_time_for_destroying);
            }
            ObjectPoolManager.releaseGameObject(gameObject, resource_path);
        }

    }
}