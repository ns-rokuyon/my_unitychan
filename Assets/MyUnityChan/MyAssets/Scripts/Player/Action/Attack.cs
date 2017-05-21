using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

namespace MyUnityChan {
    public class PlayerAttack : PlayerAction {
        public Const.ID.AttackLevel active_attack { get; set; }

        public PlayerLightAttack light { get; protected set; }
        public PlayerMiddleAttack middle { get; protected set; }
        public PlayerHeavyAttack heavy { get; protected set; }

        public override IDisposable transaction {
            get {
                var attack = getAttackInTransaction();
                if ( attack == null )
                    return null;
                return attack.action.transaction;
            }
            set {
                base.transaction = null;
            }
        }

        public override string name() {
            return "ATTACK";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.ATTACK;
        }

        public PlayerAttack(Character character)
            : base(character) {
        }

        public override void init() {
            light = new PlayerLightAttack(player, this);
            middle = new PlayerMiddleAttack(player, this);
            heavy = new PlayerHeavyAttack(player, this);
        }

        public override void performFixed() {
            var attack = getAttackInTransaction();
            if ( attack != null )
                attack.performFixed();
        }

        public override void perform() {
            var attack = getTriggeredAttack();
            if ( attack != null ) {
                attack.perform();
                active_attack = attack.level;
            }
        }

        public override void off_perform() {
            active_attack = Const.ID.AttackLevel._NO;
        }

        public override bool condition() {
            return light.condition() || middle.condition() || heavy.condition();
        }

        public void resetToDefaultAttacks() {
            light.action = null;
            middle.action = null;
            heavy.action = null;
        }

        protected PlayerLevelAttackBase getTriggeredAttack() {
            if ( light != null && light.condition() ) {
                return light;
            }
            if ( middle != null && middle.condition() ) {
                return middle;
            }
            if ( heavy != null && heavy.condition() ) {
                return heavy;
            }
            return null;
        }

        protected PlayerLevelAttackBase getAttackInTransaction() {
            if ( active_attack == Const.ID.AttackLevel._NO )
                return null;
            var a = getLevelAttack(active_attack);
            if ( a == null )
                return null;
            if ( a.action.isFreeTransaction() )
                return null;
            return a;
        }

        protected PlayerLevelAttackBase getLevelAttack(Const.ID.AttackLevel level) {
            PlayerLevelAttackBase a;
            switch ( active_attack ) {
                case Const.ID.AttackLevel.LIGHT:
                    a = light; break;
                case Const.ID.AttackLevel.MIDDLE:
                    a = middle; break;
                case Const.ID.AttackLevel.HEAVY:
                    a = heavy; break;
                default:
                    a = null; break;
            }
            return a;
        }
    }

    // Attack levels
    // =============================================================
    public abstract class PlayerLevelAttackBase : PlayerAction {
        protected PlayerAction _action;     // Current enabled attack action

        public PlayerAttack attack_manager { get; set; }
        public virtual Const.ID.AttackLevel level { get; }
        public PlayerAction action {
            get {
                return _action ?? (_action = getDefaultAction());
            }
            set {
                _action = value;
            }
        }
        public override IDisposable transaction {
            get {
                return action.transaction;
            }
        }
        public override bool use_transaction {
            get {
                return action.use_transaction;
            }
        }

        public PlayerLevelAttackBase(Character character, PlayerAttack parent) : base(character) {
            attack_manager = parent;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override void perform() {
            action.perform();
        }

        public override void performFixed() {
            action.performFixed();
        }

        public void switchTo(PlayerAction action) {
            _action = action;
        }

        public abstract PlayerAction getDefaultAction();
    }

    public class PlayerLightAttack : PlayerLevelAttackBase {
        public override Const.ID.AttackLevel level { get { return Const.ID.AttackLevel.LIGHT; } }

        public PlayerLightAttack(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override string name() {
            return "LIGHT_ATTACK";
        }

        public override bool condition() {
            return controller.keyAttack() &&
                action.condition() &&
                attack_manager.active_attack == Const.ID.AttackLevel._NO;
        }

        public override PlayerAction getDefaultAction() {
            return new PlayerPunchL(player);
        }
    }

    public class PlayerMiddleAttack : PlayerLevelAttackBase {
        public override Const.ID.AttackLevel level { get { return Const.ID.AttackLevel.MIDDLE; } }

        public PlayerMiddleAttack(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override string name() {
            return "MIDDLE_ATTACK";
        }

        public override bool condition() {
            return controller.keyAttack() &&
                action.condition() &&
                attack_manager.active_attack == Const.ID.AttackLevel.LIGHT;
        }

        public override PlayerAction getDefaultAction() {
            return new PlayerPunchR(player);
        }
    }

    public class PlayerHeavyAttack : PlayerLevelAttackBase {
        public override Const.ID.AttackLevel level { get { return Const.ID.AttackLevel.HEAVY; } }

        public PlayerHeavyAttack(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override string name() {
            return "HEAVY_ATTACK";
        }

        public override bool condition() {
            return controller.keyAttack() &&
                action.condition() &&
                attack_manager.active_attack == Const.ID.AttackLevel.MIDDLE;
        }

        public override PlayerAction getDefaultAction() {
            return new PlayerSpinKick(player);
        }
    }
}