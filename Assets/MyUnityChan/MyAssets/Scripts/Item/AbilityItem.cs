using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class AbilityItem : KeyItem {
        [SerializeField]
        public Ability.Id ability_id;

        private static List<AbilityItem> _all = new List<AbilityItem>();

        void Start() {
            _all.Add(this);     // For debug
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

        public static void setAllAbilitiesToPlayer() {
            Player player = GameStateManager.getPlayer();
            _all.ForEach(ab => player.manager.status.setAbilityStatus(ab.ability_id, Ability.Status.ON));
        }
    }
}
