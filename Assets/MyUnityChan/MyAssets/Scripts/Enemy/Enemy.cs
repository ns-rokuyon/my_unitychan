using UnityEngine;
using System.Collections.Generic;

public class NPCharacter : Character {
    static protected List<GameObject> players = new List<GameObject>();

    static public void setPlayers() {
        foreach ( GameObject pl in GameObject.FindGameObjectsWithTag("Player") ) {
            players.Add(pl);
        }
    }

    static public GameObject findNearestPlayer(Vector3 pos) {
        if ( players.Count == 1 ) {
            return players[0];
        }

        GameObject nearest = null;
        float min_dist = 100000.0f;
        foreach ( GameObject player in players ) {
            if ( nearest == null ) {
                nearest = player;
                continue;
            }

            float tmp_dist = Vector3.Distance(nearest.transform.position, player.transform.position);
            if ( tmp_dist < min_dist ) {
                min_dist = tmp_dist;
                nearest = player;
            }
        }
        return nearest;
    }

    protected virtual void start() { }
    protected virtual void update() { }
}

public class Enemy : NPCharacter {
    public GameObject controller_prefab;
    protected EnemyActionManager action_manager;
    protected int stunned = 0;

    protected void loadAttachedAI() {
        GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
        controller = controller_inst.GetComponent<Controller>();

        ((AIController)controller).setSelf(this);
    }

    public void stun(int stun_power) {
        stunned = stun_power;
    }

    public bool isStunned() {
        return stunned > 0 ? true : false;
    }

    protected void updateStunned() {
        if ( stunned > 0 ) {
            stunned--;
        }
    }

    protected void damageToPlayer() {

    }

	// Use this for initialization
	void Start () {
        loadAttachedAI();
        action_manager = new EnemyActionManager();

        start();
	}
	
	// Update is called once per frame
	void Update () {
        updateStunned();	

        update();
	}
}
