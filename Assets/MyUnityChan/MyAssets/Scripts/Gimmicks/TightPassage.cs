using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace MyUnityChan {
    public class TightPassage : ObjectBase, IPassable {
        public bool passing { get; private set; }

        private List<MeshRenderer> renderers;
        private List<Collider> colliders;
        private List<Material> frontside_mats;

        void Awake() {
            renderers = new List<MeshRenderer>();
            colliders = new List<Collider>();
            frontside_mats = new List<Material>();

            MeshRenderer[] _renderers = GetComponentsInChildren<MeshRenderer>();
            if ( _renderers.Length == 0 ) {
                return;
            }

            renderers = _renderers.ToList();

            renderers.ForEach(r => {
                colliders.Add(r.GetComponent<MeshCollider>());

                if ( r.sharedMaterials.Length == 2 ) {
                    frontside_mats.Add(r.sharedMaterials.Last());
                }
            });
        }

        void Start() {
            this.UpdateAsObservable()
                .Where(_ => GameStateManager.Instance.player_manager.now == Const.CharacterName.MINI_UNITYCHAN)
                .Subscribe(_ => {
                    bool is_inside = checkPlayerIsInside();

                    if ( !passing && is_inside ) {
                        // Enter
                        frontside_mats.ForEach(mat => {
                            DOTween.ToAlpha(() => mat.color,
                                            (c) => mat.color = c,
                                            0.2f, 0.4f);
                        });
                        passing = true;
                        GameStateManager.Instance.player_manager.current_passing = this;
                    }
                    else if ( passing && !is_inside ) {
                        // Quit
                        frontside_mats.ForEach(mat => {
                            DOTween.ToAlpha(() => mat.color,
                                            (c) => mat.color = c,
                                            1.0f, 0.4f);
                        });
                        passing = false;
                    }
                })
                .AddTo(this);
        }

        private bool checkPlayerIsInside() {
            var pm = GameStateManager.Instance.player_manager;
            if ( pm.now != Const.CharacterName.MINI_UNITYCHAN )
                return false;

            Player player = pm.getNowPlayerComponent();
            Ray ray = new Ray(pm.camera.transform.position,
                              (player.transform.position.add(0, 0.5f, 0) - pm.camera.transform.position).normalized);
            RaycastHit hit;
            if ( Physics.Raycast(ray, out hit, 20, LayerMask.GetMask("Ground")) ) {
                int i = colliders.IndexOf(hit.collider);
                if ( i < 0 )
                    return false;
                return true;
            }

            return false;
        }
    }
}