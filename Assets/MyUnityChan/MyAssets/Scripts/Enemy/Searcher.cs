using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(Enemy))]
    public class Searcher : ObjectBase {

        public SensorZone sensor;
        public bool auto_target_to_player;

        public ICharacterTargetable target { get; private set; }
        public Enemy self { get; private set; }
        public bool captured { get; private set; }
        public bool lost { get { return !captured; } }

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
                sensor.onPlayerStayCallback = (player, collider) => {
                    captured = true;
                };
                sensor.onPlayerExitCallback = (player, collider) => {
                    captured = false;
                };
                sensor.onEnemyStayCallback = null;
                sensor.onEnemyExitCallback = null;
            }
            else {
                sensor.onEnemyStayCallback = (enemy, collider) => {
                    captured = true;
                };
                sensor.onEnemyStayCallback = (enemy, collider) => {
                    captured = false;
                };
                sensor.onPlayerStayCallback = null;
                sensor.onPlayerExitCallback = null;
            }
        }
    }
}