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

        public override void perform(Player player) {
            player.registerAction(action);
            remove_actions.ForEach(remove_action => player.disableAction(remove_action));
        }

        public override void destroy(Player player) {
            ModalManager.Instance.show(name.get(), description.get());
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }
    }
}
