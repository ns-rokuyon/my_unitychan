using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyUnityChan {
    public class BossArea : Area {
        public SensorZone start_zone;
        public float baselineY = float.NaN;
        [SerializeField, ReadOnly] public BossArea.State state;

        public Boss boss { get; set; }

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

        public void stateTo(BossArea.State s) {
            state = s;
        }

        public void onStartBossBattle(Player player, Collider colliderInfo) {
            stateTo(State.START);

            if ( start_zone is DirectorZone )
                (start_zone as DirectorZone).play(player, colliderInfo);
        }

        public override void sceneGUI() {
            base.sceneGUI();
            if ( !isEmptyBaselineZ() && !isEmptyBaselineY() ) {
                Vector3 area_center = transform.position;
                Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;
                x_harf = (float)(bounds.size.x / 2.0);
                Vector3 left_point = new Vector3(area_center.x - x_harf, baselineY, baselineZ);
                Vector3 right_point = new Vector3(area_center.x + x_harf, baselineY, baselineZ);
                left_point = Handles.PositionHandle(left_point, Quaternion.identity);
                right_point = Handles.PositionHandle(right_point, Quaternion.identity);
                Vector3[] points = new Vector3[] { left_point, right_point };
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(10, points);
            }
        }

        public bool isEmptyBaselineY() {
            return float.IsNaN(baselineY);
        }
    }
}
