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
        public Dictionary<Ability.Id, Ability.Status> abilities { get; set; }

        protected override void start() {
            abilities = new Dictionary<Ability.Id, Ability.Status>();
            foreach ( var def in Ability.Defs ) {
                abilities.Add(def.Key, Ability.Status.NO_GET);
            }
        }

        protected override void update() {
            foreach ( var ab in abilities ) Debug.Log(ab);
        }

        public void addEnergyTank() {
            energy_tanks += 1;

            // Heal fully 
            reserved_hp = getReservedHpLimit();
        }

        public int getReservedHpLimit() {
            return energy_tanks * Const.Unit.RESERVED_HP;
        }

        public void setAbilityStatus(Ability.Id id, Ability.Status st) {
            abilities[id] = st;
        }

        public void toggleAbilityStatus(Ability.Id id) {
            Ability.Status now = abilities[id];
            if ( now == Ability.Status.NO_GET ) return;

            if ( now == Ability.Status.OFF ) abilities[id] = Ability.Status.ON;
            else abilities[id] = Ability.Status.OFF;
        }

    }
}