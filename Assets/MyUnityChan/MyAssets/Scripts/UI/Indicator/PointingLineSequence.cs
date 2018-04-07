using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

namespace MyUnityChan {
    [Serializable]
    public class PointingLineSequence : PointingLineBuildingBlock {
        [SerializeField]
        private List<PointingLineBuildingBlock> lines;

        [SerializeField]
        private bool auto_start;

        public override Vector2 end_point {
            get {
                return lines[-1].end_point;
            }
        }

        void Awake() {
            rect_transform = GetComponent<RectTransform>();
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

            Sequence seq = DOTween.Sequence();
            if ( lines == null )
                return seq;

            for ( int i = 0; i < lines.Count; i++ ) {
                var line = lines[i];
                if ( i == 0 ) {
                    seq.Append(line.runForward());
                }
                else {
                    var prev_line = lines[i - 1];
                    line.initPositionToEndOf(prev_line);
                    seq.Append(line.runForward());
                }
            }

            seq.OnStepComplete(() => {
                if ( seq.IsBackwards() ) {
                    // Finalize at the end of backwards
                    finalize();
                    return;
                }

                openLinkedOpenables();
            });

            seq.SetAutoKill(false);
            tween = seq;
            return seq;
        }

        public override void terminate() {
            lines.ForEach(line => line.terminate());
            finalize();
        }

        public void append(PointingLineBuildingBlock bb) {
            if ( lines == null )
                lines = new List<PointingLineBuildingBlock>();
            lines.Add(bb);
        }
    }
}
