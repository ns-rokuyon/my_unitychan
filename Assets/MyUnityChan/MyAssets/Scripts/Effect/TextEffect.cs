using UnityEngine;
using System.Collections;
using TMPro;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

namespace MyUnityChan {
    [RequireComponent(typeof(TextMeshPro))]
    public class TextEffect : Effect {
        public bool overtime_transparent = true;   // Gradually make it transparent
        public int transparent_subtraction_diff = 4;
        public Vector3 random_rotation_min;
        public Vector3 random_rotation_max;

        public TextMeshPro textmesh { get; protected set; }
        public IDisposable transparenter { get; protected set; }
        public Tweener tweener { get; protected set; }

        public string text {
            set {
                textmesh.text = value;
            }
        }

        public override void initialize() {
            base.initialize();

            textmesh = GetComponent<TextMeshPro>();
            textmesh.rectTransform.localRotation = Quaternion.identity;

            if ( random_rotation_max != Vector3.zero || random_rotation_min != Vector3.zero ) {
                float x = UnityEngine.Random.Range(random_rotation_min.x, random_rotation_max.x);
                float y = UnityEngine.Random.Range(random_rotation_min.y, random_rotation_max.y);
                float z = UnityEngine.Random.Range(random_rotation_min.z, random_rotation_max.z);
                textmesh.rectTransform.localRotation = Quaternion.Euler(x, y, z);
            }

            if ( overtime_transparent ) {
                if ( transparenter != null )
                    resetTransparent();

                transparenter = this.UpdateAsObservable()
                    .Where(_ => textmesh.faceColor.a > (byte)0)
                    .Subscribe(_ => {
                        if ( textmesh.faceColor.a - (byte)transparent_subtraction_diff <= 0 )
                            textmesh.faceColor = textmesh.faceColor.changeA(0);
                        else
                            textmesh.faceColor = textmesh.faceColor.sub(0, 0, 0, (byte)transparent_subtraction_diff);
                    }).AddTo(this);
            }

            if ( tweener != null )
                resetTween();
            tweener = textmesh.rectTransform.DOScale(2.0f, 1.0f);
        }

        protected override void onReady() {
            base.onReady();
            // TODO
        }

        public override void finalize() {
            base.finalize();

            if ( transparenter != null ) {
                resetTransparent();
            }
            if ( tweener != null ) {
                resetTween();
            }
        }

        public void resetTransparent() {
            textmesh.faceColor = textmesh.faceColor.changeA(255);
            transparenter.Dispose();
            transparenter = null;
        }

        public void resetTween() {
            tweener.Kill();
            tweener = null;
            transform.localScale = Vector3.one;
        }
    }
}