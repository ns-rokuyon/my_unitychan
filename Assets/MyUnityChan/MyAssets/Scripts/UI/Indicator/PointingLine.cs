using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

namespace MyUnityChan {

    /* 
    Image and DOTween based pointing line framework

        PointingLineBuildingBlock
            + PointingLine          : Basic component to draw a line
            + PointingLineSequence  : Sequence of some building blocks
    */

    [Serializable]
    public abstract class PointingLineBuildingBlock : GUIObjectBase, IGUIOpenable {
        [Serializable]
        public class PointingIndicator : StructBase {
            [SerializeField]
            public string keyname;

            [SerializeField]
            public GameObject prefab;

            [SerializeField]
            public Vector2 offset;

            [SerializeField]
            public int delay_frame;
        }

        [SerializeField]
        protected List<PointingIndicator> start_point_indicators = new List<PointingIndicator>();

        [SerializeField]
        protected List<PointingIndicator> end_point_indicators = new List<PointingIndicator>();

        public RectTransform rect_transform { get; protected set; }
        public GameObject start_point_indicator_object { get; protected set; }
        public GameObject end_point_indicator_object { get; protected set; }

        protected Tween tween;

        public bool visible {
            get {
                return tween != null;
            }
        }

        public bool running {
            get {
                return visible && tween.IsPlaying();
            }
        }

        public Vector2 start_point {
            get {
                return rect_transform.anchoredPosition;
            }
        }

        public abstract Vector2 end_point {
            get;
        }

        public abstract Tween runForward();

        public virtual void runBackward() {
            if ( tween == null )
                return;

            tween.PlayBackwards();
            onPlayBackward();
        }

        protected void finalize() {
            tween.Goto(0.0f);
            tween.Pause();
            onFinalize();
        }

        public void initPositionToEndOf(PointingLineBuildingBlock prev_line) {
            rect_transform.anchoredPosition = prev_line.end_point;
        }

        public virtual void open() {
            runForward();
        }

        public virtual void close() {
            runBackward();
        }

        public virtual void terminate() {
            finalize();
        }

        public virtual void onPlayBackward() { }
        public virtual void onFinalize() { }
    }

    [Serializable]
    public class PointingLine : PointingLineBuildingBlock {
        [SerializeField]
        private float length;

        [SerializeField]
        private float duration;

        [SerializeField]
        private bool auto_start;

        private Vector2 start_size;
        private Vector2 start_pos;

        // End size (width, height) of rect transform
        public Vector2 end_size {
            get {
                return new Vector2(length, line_width);
            }
        }

        // Line angle (degree)
        public float deg {
            get {
                return rect_transform.eulerAngles.z;
            }
        }

        // Line angle (radian)
        public float rad {
            get {
                return deg * Mathf.Deg2Rad;
            }
        }

        // Line width (not length)
        public float line_width {
            get {
                return start_size.y;
            }
        }

        // End point position
        public override Vector2 end_point {
            get {
                return new Vector2(rect_transform.anchoredPosition.x + Mathf.Cos(rad) * length,
                                   rect_transform.anchoredPosition.y + Mathf.Sin(rad) * length);
            }
        }

        void Awake() {
            rect_transform = GetComponent<RectTransform>();
            start_pos = rect_transform.anchoredPosition;
            start_size = rect_transform.rect.size;
        }

        void Start() {
            if ( auto_start )
                runForward();
        }

        public override Tween runForward() {
            if ( tween != null ) {
                tween.Restart();
                return tween;
            }

            tween = rect_transform.DOSizeDelta(end_size, duration);

            tween = tween.OnStart(() => {
                start_point_indicators.ForEach(start_point_indicator => {
                    // Create an indicator at start point
                    string key = "gen_start_point_indicator_" + start_point_indicator.keyname;
                    delay(key, start_point_indicator.delay_frame, () => {
                        start_point_indicator_object = Instantiate(start_point_indicator.prefab, rect_transform) as GameObject;

                        var rt = start_point_indicator_object.GetComponent<RectTransform>();
                        var offset = start_point_indicator.offset;
                        rt.anchoredPosition += offset;
                    });
                });
            });

            tween = tween.OnStepComplete(() => {
                if ( tween.IsBackwards() ) {
                    // Finalize at the end of backwards
                    finalize();
                    return;
                }
                end_point_indicators.ForEach(end_point_indicator => {
                    // Create an indicator at end point
                    string key = "gen_end_point_indicator_" + end_point_indicator.keyname;
                    delay(key, end_point_indicator.delay_frame, () => {
                        end_point_indicator_object = Instantiate(end_point_indicator.prefab, rect_transform) as GameObject;

                        var rt = end_point_indicator_object.GetComponent<RectTransform>();
                        var offset = end_point_indicator.offset;
                        rt.anchoredPosition = new Vector2(length, 0.0f) + offset;
                    });
                });
            });

            tween.SetAutoKill(false);
            return tween;
        }

        public override void onPlayBackward() {
            cancelDelay("gen_start_point_indicator");
            cancelDelay("gen_end_point_indicator");
        }

        public override void onFinalize() {
            cancelDelay("gen_start_point_indicator");
            cancelDelay("gen_end_point_indicator");

            if ( start_point_indicator_object ) {
                Destroy(start_point_indicator_object);
                start_point_indicator_object = null;
            }

            if ( end_point_indicator_object ) {
                Destroy(end_point_indicator_object);
                end_point_indicator_object = null;
            }
        }
    }
}