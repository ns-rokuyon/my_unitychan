using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class AbilityItem : KeyItem {
        [SerializeField] public Const.PlayerAction action;
        public List<string> remove_actions;

        [SerializeField]
        public GameText name;

        [SerializeField]
        public GameText description;

        [SerializeField]
        public Const.CharacterName target;

        public override void perform(Player player) {
            Player target_player = player.manager.getPlayer(target);
            target_player.registerAction(action);
            remove_actions.ForEach(remove_action => target_player.disableAction(remove_action));
        }

        public override void destroy(Player player) {
            ModalManager.Instance.show(name.get(), description.get());
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }
    }
}
