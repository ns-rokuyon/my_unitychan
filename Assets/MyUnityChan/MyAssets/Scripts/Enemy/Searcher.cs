using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(Enemy))]
    public class Searcher : ObjectBase {

        public SensorZone sensor;
        public bool auto_target_to_player;
        public bool appear_found_effect;
        public float found_delay_frame; // TODO
        public float lost_delay_frame;  // TODO

        public ICharacterTargetable target { get; private set; }
        public Enemy self { get; private set; }
        public bool captured { get; private set; }
        public bool lost { get { return !captured; } }
        public int finding_frame { get; private set; }  // TODO
        public int losing_frame { get; private set; }   // TODO

        void Awake() {
            self = GetComponent<Enemy>();
            if ( !self || !sensor )
                enabled = false;
        }

        void Start() {
            if ( auto_target_to_player )
                target = GameStateManager.pm;
            setTarget();
        }

        public void setTarget(ICharacterTargetable _target = null) {
            if ( _target != null )
                target = _target;

            if ( target == null ) {
                return;
            }

            Character ch = target.target_me;
            if ( ch is Player ) {
                sensor.onPlayerEnterCallback = (player, collider) => {
                    if ( appear_found_effect )
                        EffectManager.createTextEffect("!", Const.ID.Effect.FOUND_TARGET,
                                                       self.transform.position + self.worldspace_ui_position_offset,
                                                       60, true);
                };
                sensor.onPlayerStayCallback = (player, collider) => {
                    captured = true;
                };
                sensor.onPlayerExitCallback = (player, collider) => {
                    captured = false;
                };
                sensor.onEnemyEnterCallback = null;
                sensor.onEnemyStayCallback = null;
                sensor.onEnemyExitCallback = null;
            }
            else {
                sensor.onEnemyEnterCallback = (enemy, collider) => {
                    if ( appear_found_effect )
                        EffectManager.createTextEffect("!", Const.ID.Effect.FOUND_TARGET,
                                                       self.transform.position + self.worldspace_ui_position_offset,
                                                       60, true);
                };
                sensor.onEnemyStayCallback = (enemy, collider) => {
                    captured = true;
                };
                sensor.onEnemyStayCallback = (enemy, collider) => {
                    captured = false;
                };
                sensor.onPlayerEnterCallback = null;
                sensor.onPlayerStayCallback = null;
                sensor.onPlayerExitCallback = null;
            }
        }
    }
}