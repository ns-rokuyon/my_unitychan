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
        public PlayableDirector director { get; protected set; }

        public enum State {
            STANDBY,
            START,
            BATTLE,
            END,
            EMPTY
        }

        public override void Awake() {
            base.Awake();

            director = GetComponent<PlayableDirector>();

            if ( start_zone ) {
                start_zone.onPlayerEnterCallback = onStartBossBattle;
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

            director.Play();
        }
    }
}
