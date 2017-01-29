using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class PlayerAbility : StructBase {
        public Ability.Status status { get; set; }
        public AbilityDef def { get; private set; }
        public PlayerManager manager { get; private set; }

        public PlayerAbility(AbilityDef _def, PlayerManager _manager) {
            manager = _manager;
            def = _def;
            status = Ability.Status.NO_GET;

            var status_changed = Observable.EveryUpdate().Select(_ => status).DistinctUntilChanged();
            status_changed.Where(s => s == Ability.Status.ON).Subscribe(_ => {
                def.on(manager);
            });
            status_changed.Where(s => s == Ability.Status.OFF).Subscribe(_ => {
                def.off(manager);
            });
        }

        public void toggleStatus() {
            if ( status == Ability.Status.NO_GET ) return;

            if ( status == Ability.Status.OFF ) status = Ability.Status.ON;
            else status = Ability.Status.OFF;
        }

    }
}
