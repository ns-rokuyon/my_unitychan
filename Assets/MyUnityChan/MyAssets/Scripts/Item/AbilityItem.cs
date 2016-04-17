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
            AbilityDef ad = player.manager.status.getAbility(ability_id).def;
            ModalManager.Instance.show(ad.name, ad.description);
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }
    }
}
