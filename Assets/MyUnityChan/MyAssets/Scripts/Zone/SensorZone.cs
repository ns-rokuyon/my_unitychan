using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class SensorZone : Zone {
        public bool only_firsttime;

        public int enter_delay_frame = 0;
        public int exit_delay_frame = 0;

        public System.Action<Player, Collider> onPlayerEnterCallback { get; set; }
        public System.Action<Player, Collider> onPlayerStayCallback { get; set; }
        public System.Action<Player, Collider> onPlayerExitCallback { get; set; }
        public System.Action<Enemy, Collider> onEnemyEnterCallback { get; set; }
        public System.Action<Enemy, Collider> onEnemyStayCallback { get; set; }
        public System.Action<Enemy, Collider> onEnemyExitCallback { get; set; }

        private Dictionary<ObjectBase, IDisposable> enter_delayer = new Dictionary<ObjectBase, IDisposable>();
        private Dictionary<ObjectBase, IDisposable> exit_delayer = new Dictionary<ObjectBase, IDisposable>();

        protected override void onPlayerEnter(Player player, Collider colliderInfo) {
            if ( enter_delay_frame > 0 ) {
                enter_delayer[player] = player.createTimer(enter_delay_frame)
                                              .Subscribe(_ => { }, _ => {
                                                  if ( onPlayerEnterCallback != null )
                                                      onPlayerEnterCallback(player, colliderInfo);

                                                  if ( only_firsttime ) {
                                                      gameObject.SetActive(false);
                                                      return;
                                                  }

                                                  player.staying_zones.Add(this);
                                                  enter_delayer[player] = null;
                                              });
            }
            else {
                // Immediately enter
                if ( onPlayerEnterCallback != null)
                    onPlayerEnterCallback(player, colliderInfo);

                if ( only_firsttime ) {
                    gameObject.SetActive(false);
                    return;
                }

                player.staying_zones.Add(this);
            }
        }

        protected override void onPlayerStay(Player player, Collider colliderInfo) {
            if ( !player.isInZone(this) ) {
                // If enter_delay_frame > 0,
                // the onPlayerStay() will not call the onPlayerStayCallback() until after enter_delay_frame 
                return;
            }

            if ( onPlayerStayCallback != null ) {
                onPlayerStayCallback(player, colliderInfo);
            }
        }

        protected override void onPlayerExit(Player player, Collider colliderInfo) {
            if ( exit_delay_frame > 0 ) {
                if ( enter_delayer[player] != null ) {
                    // Cancel to change enter to exit
                    enter_delayer[player].Dispose();
                    enter_delayer[player] = null;

                    return;
                }

                exit_delayer[player] = player.createTimer(exit_delay_frame)
                                             .Subscribe(_ => { }, _ => {
                                                 if ( onPlayerExitCallback != null )
                                                     onPlayerExitCallback(player, colliderInfo);

                                                 player.staying_zones.Remove(this);
                                                 exit_delayer[player] = null;
                                             });
            }
            else {
                // Immediately exit
                if ( onPlayerExitCallback != null )
                    onPlayerExitCallback(player, colliderInfo);

                player.staying_zones.Remove(this);
            }
        }

        protected override void onEnemyEnter(Enemy enemy, Collider colliderInfo) {
            if ( enter_delay_frame > 0 ) {
                enter_delayer[enemy] = enemy.createTimer(enter_delay_frame)
                                            .Subscribe(_ => { }, _ => {
                                                if ( onEnemyEnterCallback != null )
                                                    onEnemyEnterCallback(enemy, colliderInfo);

                                                if ( only_firsttime ) {
                                                    gameObject.SetActive(false);
                                                    return;
                                                }

                                                enemy.staying_zones.Add(this);
                                                enter_delayer[enemy] = null;
                                            });
            }
            else {
                // Immediately enter
                if ( onEnemyEnterCallback != null)
                    onEnemyEnterCallback(enemy, colliderInfo);

                if ( only_firsttime ) {
                    gameObject.SetActive(false);
                    return;
                }

                enemy.staying_zones.Add(this);
            }
        }

        protected override void onEnemyStay(Enemy enemy, Collider colliderInfo) {
            if ( !enemy.isInZone(this) ) {
                // If enter_delay_frame > 0,
                // the onPlayerStay() will not call the onPlayerStayCallback() until after enter_delay_frame 
                return;
            }

            if ( onEnemyStayCallback != null ) {
                onEnemyStayCallback(enemy, colliderInfo);
            }
        }

        protected override void onEnemyExit(Enemy enemy, Collider colliderInfo) {
            if ( exit_delay_frame > 0 ) {
                if ( enter_delayer[enemy] != null ) {
                    // Cancel to change enter to exit
                    enter_delayer[enemy].Dispose();
                    enter_delayer[enemy] = null;

                    return;
                }

                exit_delayer[enemy] = enemy.createTimer(exit_delay_frame)
                                           .Subscribe(_ => { }, _ => {
                                               if ( onEnemyExitCallback != null )
                                                   onEnemyExitCallback(enemy, colliderInfo);

                                               enemy.staying_zones.Remove(this);
                                               exit_delayer[enemy] = null;
                                           });
            }
            else {
                // Immediately exit
                if ( onEnemyExitCallback != null )
                    onEnemyExitCallback(enemy, colliderInfo);

                enemy.staying_zones.Remove(this);
            }
        }
    }
}