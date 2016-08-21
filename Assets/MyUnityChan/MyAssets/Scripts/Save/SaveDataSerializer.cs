using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public interface IDataSerializer {
        void serialize();
        void deserialize();
    }

    public class SaveDataSerializer : StructBase, IDataSerializer {
        public virtual void deserialize() {
            throw new NotImplementedException();
        }

        public virtual void serialize() {
            throw new NotImplementedException();
        }

        protected PlayerStatus getPlayerStatus() {
            return GameStateManager.getPlayer().manager.status;
        }
    }

    public class SaveDataConsoleTextSerializer : SaveDataSerializer {
        // test
        public override void serialize() {
            PlayerStatus status = getPlayerStatus();
            string text = "hp=" + status.hp + ", energy_tanks=" + status.energy_tanks + ", missile_tanks=" + status.missile_tanks;
            DebugManager.log(text);
        }

        public override void deserialize() {
            // TODO
        }
    }
}