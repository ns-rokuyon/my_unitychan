using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class LockZone : SensorZone {
        public List<Lockable> targets;

        void Awake() {
            targets = new List<Lockable>();
        }

        public virtual void Start() {
            delay(2, () => {
                var ts = this.gameObject.GetComponentsInSameArea<Lockable>();
                ts.ToList().ForEach(t => {
                    targets.Add(t as Lockable);
                });
            });

            onPlayerEnterCallback = doLock;
            onPlayerExitCallback = doUnlock;
        }

        public virtual void doLock() {
            targets.ForEach(t => t.doLock());
        }

        public virtual void doUnlock() {
            targets.ForEach(t => t.doUnlock());
        }
    }
}