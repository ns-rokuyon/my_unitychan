using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerStatus : CharacterStatus {
        public override int hp {
            get {
                return base.hp;
            }
            set {
                base.hp = value;
                if ( base.hp <= 0 ) {
                    base.hp = 0;

                    if ( reserved_hp > 0 ) {
                        int resv = reserved_hp < Const.Unit.RESERVED_HP ? reserved_hp : Const.Unit.RESERVED_HP;
                        base.hp = resv;
                        reserved_hp -= resv;
                    }
                }
                else if ( base.hp > Const.Max.PLAYER_HP ) {
                    int over = base.hp - Const.Max.PLAYER_HP;
                    base.hp = Const.Max.PLAYER_HP;
                    reserved_hp += over;
                    if ( reserved_hp > getReservedHpLimit() ) reserved_hp = getReservedHpLimit();
                }
            }
        }
        public int reserved_hp { get; private set; }
        public int energy_tanks { get; private set; }
        public int missile_tanks { get; set; }
        public Dictionary<Ability.Id, PlayerAbility> abilities { get; set; }
        public PlayerSetting setting { get; private set; }

        protected override void awake() {
            base.awake();
            abilities = new Dictionary<Ability.Id, PlayerAbility>();
            foreach ( var def in Ability.Defs ) {
                abilities.Add(def.Key, new PlayerAbility(def.Value));
            }

            setting = new PlayerSetting();
        }

        protected override void update() {
        }

        public void addEnergyTank() {
            energy_tanks += 1;

            // Heal fully 
            reserved_hp = getReservedHpLimit();
        }

        public int getReservedHpLimit() {
            return energy_tanks * Const.Unit.RESERVED_HP;
        }

        public PlayerAbility getAbility(Ability.Id id) {
            return abilities[id];
        }

        public void setAbilityStatus(Ability.Id id, Ability.Status st) {
            if ( abilities[id].status == Ability.Status.NO_GET ) {
                abilities[id].def.on(GameStateManager.getPlayer().manager, true);
            }
            abilities[id].status = st;
        }

        public void toggleAbilityStatus(Ability.Id id) {
            abilities[id].toggleStatus();
        }

    }
}