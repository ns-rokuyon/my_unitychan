using UnityEngine;
using System.Collections.Generic;

public class EnemyActionManager : ActionManager {
    protected override void start() {
    }

    protected override void update() {
    }
}

public abstract class EnemyActionBase : Action {
	protected Enemy enemy;
	protected AIController controller;

	public EnemyActionBase(Character character) {
		enemy = (Enemy)character;
		controller = (AIController)enemy.getController();
	}

}
