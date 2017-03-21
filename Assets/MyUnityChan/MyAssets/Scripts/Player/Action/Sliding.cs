﻿using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public class PlayerSliding : PlayerAction {

        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                stun = 60;
                frame = 5;
                hitstop = 20;
                knockback = 20;
                launch_fy = 5.0f;
                hit_se = Const.ID.SE.HIT_1;
                effect_name = Const.ID.Effect.HIT_01;
            }
        }

        private AttackSpec spec = null;
        private string hitbox_resource_path;

        public PlayerSliding(Character character)
            : base(character) {
            priority = 5;
            skip_lower_priority = true;
            hitbox_resource_path = Const.Prefab.Hitbox["KICK"];
            spec = new Spec();
        }

        public override string name() {
            return "SLIDING";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.SLIDING;
        }

        public override void performFixed() {
            player.GetComponent<Rigidbody>().AddForce(player.transform.forward * 10.0f, ForceMode.VelocityChange);
        }

        public override void perform() {
            player.voice(Const.ID.PlayerVoice.ATTACK3);
            player.getAnimator().CrossFade("Sliding", 0.001f);
            player.lockInput(40);
            Observable.TimerFrame(6)
                .Subscribe(_ => createHitbox());
        }

        public void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.7f * fw.x, 0.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }

        public override bool condition() {
            return controller.keyDown() && controller.keyAttack() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
        }

    }
}