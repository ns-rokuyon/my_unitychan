using UnityEngine;
using System.Collections;

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

        protected override void start() {
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
    }
}