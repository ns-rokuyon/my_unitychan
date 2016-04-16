using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class AbilityItem : KeyItem {
        [SerializeField]
        public Ability.Id ability_id;

        void Start() {

        }

        public override void perform(Player player) {
            player.manager.status.setAbilityStatus(ability_id, Ability.Status.ON);
        }

        public override void destroy(Player player) {
            AbilityDef ad = Ability.Defs[ability_id];
            ModalManager.Instance.show(ad.name, ad.description);
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }
    }
}
