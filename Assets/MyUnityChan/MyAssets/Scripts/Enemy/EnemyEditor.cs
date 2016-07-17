using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MyUnityChan {
    [CustomEditor(typeof(Enemy), true)]
    public class EnemyEditor : Editor {
        private Enemy enemy;

        void OnEnable() {
            enemy = target as Enemy;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            if ( enemy is IEnemyLevelUp ) {
                (enemy as IEnemyLevelUp).levelUp();
            }
        }
    }
}
