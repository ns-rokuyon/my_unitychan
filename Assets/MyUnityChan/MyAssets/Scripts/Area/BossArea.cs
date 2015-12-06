using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyUnityChan {
    public class BossArea : Area {
        public string boss_resource_name;
        public Vector3 boss_start_pos;
        public float player_start_x;

        private BossArea.State state;
        private GameObject boss;

        enum State {
            STANDBY,
            START,
            BATTLE,
            END,
            EMPTY
        }

        private void stateTo(BossArea.State s) {
            state = s;
        }

        void Start() {
            state = State.STANDBY;
            boss = null;
        }

        void Update() {
            switch ( state ) {
                case State.STANDBY:
                    waiting();
                    break;
                case State.START:
                    appear();
                    break;
            }
        }

        private void waiting() {
            GameObject player = Player.getPlayer();
            if ( player == null || !isIn(player.name) ) return;

            float player_x = player.transform.position.x;
            if ( player_start_x - 2.0f < player_x && player_x < player_start_x + 2.0f ) {
                stateTo(State.START);
            }
        }

        private void appear() {
            boss = PrefabInstantiater.create(Const.Prefab.Boss[boss_resource_name], Hierarchy.Layout.ENEMY);
            if ( boss ) {
                boss.transform.position = boss_start_pos;
                boss.GetComponent<Enemy>().adjustZtoBaseline();
                stateTo(State.BATTLE);
            }
        }

        #if UNITY_EDITOR
        public override void sceneGUI() {
            base.sceneGUI();

            Vector3 area_center = transform.position;
            Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;
            y_harf = (float)(bounds.size.y / 2.0);
            Vector3 boss_start_top_point = new Vector3(boss_start_pos.x, area_center.y + y_harf, baselineZ);
            Vector3 boss_start_bottom_point = new Vector3(boss_start_pos.x, area_center.y - y_harf, baselineZ);
            Vector3[] boss_start_line_points = new Vector3[] { 
                Handles.PositionHandle(boss_start_top_point, Quaternion.identity),
                Handles.PositionHandle(boss_start_bottom_point, Quaternion.identity)
            };
            Handles.color = Color.cyan;
            Handles.DrawAAPolyLine(10, boss_start_line_points);     // Draw line at boss start position

            Vector3 player_start_top_point = new Vector3(player_start_x, area_center.y + y_harf, baselineZ);
            Vector3 player_start_bottom_point = new Vector3(player_start_x, area_center.y - y_harf, baselineZ);
            Vector3[] player_start_line_points = new Vector3[] { 
                Handles.PositionHandle(player_start_top_point, Quaternion.identity),
                Handles.PositionHandle(player_start_bottom_point, Quaternion.identity)
            };
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(10, player_start_line_points);   // Draw line at player start position
        }
        #else
        public override void sceneGUI() {
        }
        #endif
    }

}
