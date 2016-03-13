﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyUnityChan {
    public class MissilePod : TurretBase {
        public string missile_name;
        public int missile_max;
        public GameObject indicator_ui_object;

        public int missile_num { get; protected set; }

        protected Text indicator = null;

        void Awake() {
            indicator = indicator_ui_object.GetComponent<Text>();
        }

        void Start() {
            baseStart();
            setProjectile(missile_name);
            missile_num = missile_max;
        }

        void Update() {
            baseUpdate();
            if ( indicator ) indicator.text = "" + missile_num;
        }

        public override void shoot() {
            Debug.Log(missile_num);
            if ( missile_num == 0 ) {
                return;
            }
            missile_num--;

            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[missile_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            Missile missile = obj.GetComponent<Missile>();
            missile.setDir(angle());
            missile.fire(transform.position, owner.isLookAhead() ? 1.0f : -1.0f);

            // hitbox
            ProjectileHitbox hitbox = missile.GetComponentInChildren<ProjectileHitbox>();
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