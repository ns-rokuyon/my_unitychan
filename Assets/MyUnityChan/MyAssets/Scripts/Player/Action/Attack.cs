using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

namespace MyUnityChan {
    public class PlayerAttack : PlayerAction {
        public Const.ID.AttackSlotType active_attack { get; set; }

        // Slots
        public PlayerLightAttackSlot light { get; protected set; }
        public PlayerMiddleAttackSlot middle { get; protected set; }
        public PlayerHeavyAttackSlot heavy { get; protected set; }
        public PlayerUpAttackSlot up { get; protected set; }
        public PlayerDownAttackSlot down { get; protected set; }

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
            light = new PlayerLightAttackSlot(player, this);
            middle = new PlayerMiddleAttackSlot(player, this);
            heavy = new PlayerHeavyAttackSlot(player, this);
            up = new PlayerUpAttackSlot(player, this);
            down = new PlayerDownAttackSlot(player, this);
        }

        public void switchTo(PlayerAction action, Const.ID.AttackSlotType st) {
            var slot = getSlot(st);
            if ( slot != null ) return;
            slot.switchTo(action);
        }

        public void clearSlot(Const.ID.AttackSlotType st) {
            var slot = getSlot(st);
            if ( slot != null ) return;
            slot.action = null;
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
                active_attack = attack.slot;
            }
        }

        public override void off_perform() {
            active_attack = Const.ID.AttackSlotType._NO;
        }

        public override bool condition() {
            return light.condition() || middle.condition() || heavy.condition() || up.condition() || down.condition();
        }

        public void resetToDefaultAttacks() {
            light.action = null;
            middle.action = null;
            heavy.action = null;
            up.action = null;
            down.action = null;
        }

        protected PlayerAttackSlotBase getTriggeredAttack() {
            if ( up != null && up.condition() ) {
                return up;
            }
            if ( down != null && down.condition() ) {
                return down;
            }

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

        protected PlayerAttackSlotBase getAttackInTransaction() {
            if ( active_attack == Const.ID.AttackSlotType._NO )
                return null;
            var a = getActiveSlot();
            if ( a == null )
                return null;
            if ( a.action.isFreeTransaction() )
                return null;
            return a;
        }

        protected PlayerAttackSlotBase getSlot(Const.ID.AttackSlotType st) {
            switch ( st ) {
                case Const.ID.AttackSlotType.UP: return up;
                case Const.ID.AttackSlotType.DOWN: return down;
                case Const.ID.AttackSlotType.LIGHT: return light;
                case Const.ID.AttackSlotType.MIDDLE: return middle;
                case Const.ID.AttackSlotType.HEAVY: return heavy;
                default: break;
            }
            return null;
        }

        protected PlayerAttackSlotBase getActiveSlot() {
            PlayerAttackSlotBase a;
            switch ( active_attack ) {
                case Const.ID.AttackSlotType.UP:
                    a = up; break;
                case Const.ID.AttackSlotType.DOWN:
                    a = down; break;
                case Const.ID.AttackSlotType.LIGHT:
                    a = light; break;
                case Const.ID.AttackSlotType.MIDDLE:
                    a = middle; break;
                case Const.ID.AttackSlotType.HEAVY:
                    a = heavy; break;
                default:
                    a = null; break;
            }
            return a;
        }
    }

    // Attack base class
    // =============================================================
    public abstract class PlayerAttackSlotBase : PlayerAction {
        protected PlayerAction _action;     // Current enabled attack action

        public virtual Const.ID.AttackSlotType slot { get; }

        public PlayerAttack attack_manager { get; set; }
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

        public PlayerAttackSlotBase(Character character, PlayerAttack parent) : base(character) {
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


    // Directional Attack
    // =============================================================
    public abstract class PlayerDirectionalAttackSlotBase : PlayerAttackSlotBase {
        public PlayerDirectionalAttackSlotBase(Character character, PlayerAttack parent) : base(character, parent) {
        }
    }

    public class PlayerUpAttackSlot : PlayerDirectionalAttackSlotBase {
        public override Const.ID.AttackSlotType slot { get { return Const.ID.AttackSlotType.UP; } }

        public PlayerUpAttackSlot(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override bool condition() {
            return action != null &&
                controller.keyAttack() &&
                controller.keyUp() &&
                action.condition();
        }

        public override PlayerAction getDefaultAction() {
            //return null;
            return new PlayerShoryu(player);
        }

        public override string name() {
            return "UP_ATTACK";
        }
    }

    public class PlayerDownAttackSlot : PlayerDirectionalAttackSlotBase {
        public override Const.ID.AttackSlotType slot { get { return Const.ID.AttackSlotType.DOWN; } }

        public PlayerDownAttackSlot(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override bool condition() {
            return action != null &&
                controller.keyAttack() &&
                controller.keyDown() &&
                action.condition();
        }

        public override PlayerAction getDefaultAction() {
            //return null;
            return new PlayerSliding(player);
        }

        public override string name() {
            return "DOWN_ATTACK";
        }
    }

    // Attack levels
    // =============================================================
    public abstract class PlayerLevelAttackSlotBase : PlayerAttackSlotBase {
        public PlayerLevelAttackSlotBase(Character character, PlayerAttack parent) : base(character, parent) {
        }
    }

    public class PlayerLightAttackSlot : PlayerLevelAttackSlotBase {
        public override Const.ID.AttackSlotType slot { get { return Const.ID.AttackSlotType.LIGHT; } }

        public PlayerLightAttackSlot(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override string name() {
            return "LIGHT_ATTACK";
        }

        public override bool condition() {
            return controller.keyAttack() &&
                action.condition() &&
                attack_manager.active_attack == Const.ID.AttackSlotType._NO;
        }

        public override PlayerAction getDefaultAction() {
            return new PlayerPunchL(player);
        }
    }

    public class PlayerMiddleAttackSlot : PlayerLevelAttackSlotBase {
        public override Const.ID.AttackSlotType slot { get { return Const.ID.AttackSlotType.MIDDLE; } }

        public PlayerMiddleAttackSlot(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override string name() {
            return "MIDDLE_ATTACK";
        }

        public override bool condition() {
            return controller.keyAttack() &&
                action.condition() &&
                attack_manager.active_attack == Const.ID.AttackSlotType.LIGHT;
        }

        public override PlayerAction getDefaultAction() {
            return new PlayerPunchR(player);
        }
    }

    public class PlayerHeavyAttackSlot : PlayerLevelAttackSlotBase {
        public override Const.ID.AttackSlotType slot { get { return Const.ID.AttackSlotType.HEAVY; } }

        public PlayerHeavyAttackSlot(Character character, PlayerAttack parent) : base(character, parent) {
        }

        public override string name() {
            return "HEAVY_ATTACK";
        }

        public override bool condition() {
            return controller.keyAttack() &&
                action.condition() &&
                attack_manager.active_attack == Const.ID.AttackSlotType.MIDDLE;
        }

        public override PlayerAction getDefaultAction() {
            return new PlayerSpinKick(player);
        }
    }
}