using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MyUnityChan {
    public class MovingFloor : ObjectBase {
        public List<ObjectBase> members = new List<ObjectBase>();

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

        public virtual void getOn(ObjectBase ob) {
            if ( ob.GetComponent<Player>() ) {
                ob = ob.GetComponent<Player>().manager;
            }
            ob.transform.parent = gameObject.transform;
            members.Add(ob);
        }

        public virtual void getOff(ObjectBase ob) {
            if ( ob.GetComponent<Player>() ) {
                ob = ob.GetComponent<Player>().manager;
            }
            ob.transform.parent = null;
            members.Remove(ob);
        }

        public PlayerManager getPlayerManager() {
            return members.Where(m => m.GetComponent<PlayerManager>()).FirstOrDefault() as PlayerManager;
        }

    }

    public class TriggerCollision : ObjectBase {
        public virtual void onPlayerInputUp(Player player) { }
        public virtual void onPlayerInputDown(Player player) { }
        public virtual void onPlayerEnter(Player player) { }

        public virtual bool isInputUp(Character character) {
            return !character.isFrozen() &&
                    character.isGrounded() && 
                    character.getController().keyVertical() > 0;
        }
        public virtual bool isInputDown(Character character) {
            return !character.isFrozen() &&
                    character.isGrounded() && 
                    character.getController().keyVertical() > 0;
        }
        public virtual bool isEnter(Character character) {
            return !character.isFrozen();
        }

        public virtual void onEnemyInputUp(Enemy enemy) { }
        public virtual void onEnemyInputDown(Enemy enemy) { }
        public virtual void onEnemyEnter(Enemy enemy) { }

        public virtual void OnTriggerStay(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                if ( isInputUp(player) )
                    onPlayerInputUp(player);
                else if ( isInputDown(player) )
                    onPlayerInputDown(player);
                else if ( isEnter(player) )
                    onPlayerEnter(player);
            }
            else if ( colliderInfo.gameObject.tag == "Enemy" ) {
                Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                if ( isInputUp(enemy) )
                    onEnemyInputUp(enemy);
                else if ( isInputDown(enemy) )
                    onEnemyInputDown(enemy);
                else if ( isEnter(enemy) )
                    onEnemyEnter(enemy);
            }
        }
    }

    public abstract class Warp : TriggerCollision {
        public GameObject warp_to;
        public float dst_direction;

        public abstract void warp(Player player);

    }

    public abstract class Door : ObjectBase {
        public abstract void open();
        public abstract void close();
    }
}
