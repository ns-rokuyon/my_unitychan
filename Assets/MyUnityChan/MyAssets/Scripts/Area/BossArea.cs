using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyUnityChan {
    public class BossArea : Area {
        public SensorZone start_zone;

        [SerializeField, ReadOnly] public BossArea.State state;

        public GameObject boss { get; protected set; }

        public enum State {
            STANDBY,
            START,
            BATTLE,
            END,
            EMPTY
        }

        public override void Awake() {
            base.Awake();

            if ( !start_zone )
                DebugManager.warn("" + name + " has no zone references to start boss battle");

            if ( start_zone ) {
                start_zone.onPlayerEnterCallback = onStartBossBattle;
                if ( !(start_zone as DirectorZone).skip_setup ) {
                    DebugManager.warn("DirectorZone.skip_setup should be set true");
                }
            }
        }

        public override void Start() {
            base.Start();

            state = State.STANDBY;
            boss = null;
        }

        private void stateTo(BossArea.State s) {
            state = s;
        }

        public void onStartBossBattle(Player player, Collider colliderInfo) {
            stateTo(State.START);

            if ( start_zone is DirectorZone )
                (start_zone as DirectorZone).play(player, colliderInfo);
        }
    }
}
