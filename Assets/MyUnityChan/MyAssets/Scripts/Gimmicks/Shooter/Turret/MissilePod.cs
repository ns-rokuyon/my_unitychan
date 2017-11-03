﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class MissilePod : TurretBase {
        public string missile_name;
        public int missile_max;
        public GameObject indicator_ui_object;

        public int missile_num { get; protected set; }

        protected Text indicator = null;
        private Player player = null;

        void Awake() {
            indicator = indicator_ui_object.GetComponent<Text>();
        }

        public override void Start() {
            base.Start();

            setProjectile(missile_name);
            missile_num = missile_max;

            player = GetComponent<Player>();
            if ( player ) {
                // Auto sync (this.missile_max <-> player.status.missile_tanks * 5)
                this.ObserveEveryValueChanged(_ => player.manager.status.missile_tanks)
                    .Where(tanks => tanks > 0)
                    .Subscribe(_ => {
                        missile_max += Const.Unit.ADDITIONAL_MISSILES;
                        addMissile(Const.Unit.ADDITIONAL_MISSILES);
                    });
            }
        }

        void Update() {
            if ( indicator ) indicator.text = "" + missile_num;
        }

        public override void shoot() {
            if ( missile_num == 0 ) {
                return;
            }
            missile_num--;

            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[missile_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            Missile missile = obj.GetComponent<Missile>();
            missile.setDir(direction);
            missile.linkShooter(this);
            missile.fire(transform.position + muzzle_offset, owner.isLookAhead() ? 1.0f : -1.0f);

            // hitbox
            ProjectileHitbox hitbox = missile.GetComponentInChildren<ProjectileHitbox>();
            hitbox.depend_on_parent_object = true;
            hitbox.setOwner(gameObject);
            hitbox.setEnabledCollider(true);
            hitbox.ready(obj, missile.spec);

            // sound
            sound();
        }

        public void addMissile(int n) {
            missile_num += n;
            if ( missile_num > missile_max ) {
                missile_num = missile_max;
            }
        }

    }
}
