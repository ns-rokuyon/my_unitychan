using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Boss : Enemy {
        [System.Serializable]
        public class Param : StructBase {
            public float walk_fx;       // Force to walk forward
            public float max_speed;
        };

        [SerializeField]
        public Boss.Param default_param;

        public Boss.Param param {
            get; protected set;
        }

        public BossArea boss_area {
            get {
                return parent_area as BossArea;
            }
        }

        protected override void start() {
            base.start();

            param = default_param;
            action_manager.registerAction(new EnemyDead(this));
            setHP(max_hp);

            var cam = GameStateManager.getPlayer().manager.camera;
            if ( cam is PlayerCameraProCamera2D ) {
                (cam as PlayerCameraProCamera2D).addCameraTarget(transform);
            }
        }
    }
}
