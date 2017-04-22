using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class EnemyBattleLockZone : LockZone {
        public int enemy_count { get; protected set; }

        public override void Start() {
            base.Start();
            enemy_count = 0;

            onPlayerEnterCallback = doLock;
            onPlayerExitCallback = null;
        }

        public override void doLock() {
            base.doLock();
            enemy_count = parent_area.getActiveEnemyCount();
            this.ObserveEveryValueChanged(_ => parent_area.getActiveEnemyCount())
                .Do(c => enemy_count = c)
                .Subscribe(c => {
                    if ( c == 0 )
                        doUnlock();
                })
                .AddTo(this);
        }
    }
}
