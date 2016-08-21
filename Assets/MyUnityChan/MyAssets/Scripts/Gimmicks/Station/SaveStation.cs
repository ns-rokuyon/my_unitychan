using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SaveStation : StationBase {
        public override void perform(Player player) {
            SaveManager.save();
        }
    }
}