﻿using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class EnemyActionManager : ActionManager {
        protected override void start() {
        }

        protected override void update() {
        }
    }

    public abstract class EnemyActionBase : Action {
        protected Enemy enemy;
        protected AIController controller;

        public override Character owner {
            get {
                return enemy;
            }
        }

        public EnemyActionBase(Character character) {
            enemy = (Enemy)character;
            controller = (AIController)enemy.getController();
            priority = 0;
            skip_lower_priority = false;
            perform_callbacks = new List<System.Action>();
        }

    }
}
