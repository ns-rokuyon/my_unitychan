using UnityEngine;
using System.Collections.Generic;
using UniRx;
using System.Linq;

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
        public bool gameover { get; set; }
        public Dictionary<Ability.Id, PlayerAbility> abilities { get; set; }
        public PlayerSetting setting { get; private set; }
        public PlayerManager manager { get; set; }

        private ReactiveProperty<int> missile_tank_num = new ReactiveProperty<int>(0);
        private ReactiveDictionary<Const.ID.Projectile.Beam, bool> beam_slots = new ReactiveDictionary<Const.ID.Projectile.Beam, bool>();

        [SerializeField, ReadOnly]
        private List<Ability.Id> abilitiy_ids = new List<Ability.Id>();

        // Current number of missile tank
        public int MissileTankNum {
            get { return missile_tank_num.Value; }
        }

        public List<Const.ID.Projectile.Beam> AvailableBeams {
            get {
                AvailableBeamStream.ToList().ForEach(b => DebugManager.log(b));
                return AvailableBeamStream.ToList();
            }
        }

        // Reactive collection of available beam ids
        public ReactiveCollection<Const.ID.Projectile.Beam> AvailableBeamStream {
            get { return beam_slots.Select(kv => kv.Key).OrderBy(id => id).ToReactiveCollection(); }
        }

        // Reactive prop for missile tank num
        public ReadOnlyReactiveProperty<int> MissileTankNumStream {
            get { return missile_tank_num.ToReadOnlyReactiveProperty(); }
        }

        protected override void awake() {
            base.awake();
            setting = new PlayerSetting();
        }

        protected override void start() {
            base.start();

            // Init ability
            foreach ( var ability in abilities ) {
                if ( ability.Value.def.init_status == Ability.Status.ON ) {
                    setAbilityStatus(ability.Value.def.id, Ability.Status.ON);
                }
            }
        }

        protected override void update() {
        }

        public void setupAbilities() {
            if ( !manager )
                DebugManager.error("manager is null");
            abilities = new Dictionary<Ability.Id, PlayerAbility>();
            foreach ( var def in Ability.Defs ) {
                abilities.Add(def.Key, new PlayerAbility(def.Value, manager));
                abilitiy_ids.Add(def.Key);
            }
        }

        public void addEnergyTank() {
            energy_tanks += 1;

            // Heal fully 
            reserved_hp = getReservedHpLimit();
        }

        public void addMissileTank() {
            missile_tank_num.Value += 1;
        }

        public void enableBeam(Const.ID.Projectile.Beam beam_id) {
            beam_slots[beam_id] = true;
        }

        public void disableBeam(Const.ID.Projectile.Beam beam_id) {
            if ( !beam_slots.ContainsKey(beam_id) )
                return;
            beam_slots[beam_id] = false;
        }

        public int getReservedHpLimit() {
            return energy_tanks * Const.Unit.RESERVED_HP;
        }

        public PlayerAbility getAbility(Ability.Id id) {
            return abilities[id];
        }

        public void setAbilityStatus(Ability.Id id, Ability.Status st) {
            if ( abilities[id].status == Ability.Status.NO_GET ) {
                abilities[id].def.on(manager, true);
            }
            abilities[id].status = st;
        }

        public void toggleAbilityStatus(Ability.Id id) {
            abilities[id].toggleStatus();
        }

    }
}