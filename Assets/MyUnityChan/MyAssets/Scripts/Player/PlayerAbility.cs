using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class PlayerAbility : StructBase {
        public AbilityDef def { get; private set; }
        public Ability.Status status { get; set; }

        public PlayerAbility(AbilityDef _def) {
            def = _def;
            status = def.init_status;

            var status_changed = Observable.EveryUpdate().Select(_ => status).DistinctUntilChanged();
            status_changed.Where(s => s == Ability.Status.ON).Subscribe(_ => {
                def.on(GameStateManager.getPlayer().manager);
            });
            status_changed.Where(s => s == Ability.Status.OFF).Subscribe(_ => {
                def.off(GameStateManager.getPlayer().manager);
            });
        }

        public PlayerAbility(AbilityDef _def, Ability.Status _status) {
            def = _def;
            status = _status;
        }

        public void toggleStatus() {
            if ( status == Ability.Status.NO_GET ) return;

            if ( status == Ability.Status.OFF ) status = Ability.Status.ON;
            else status = Ability.Status.OFF;
        }

    }
}
