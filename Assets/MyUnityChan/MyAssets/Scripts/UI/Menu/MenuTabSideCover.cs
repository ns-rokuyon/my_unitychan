using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace MyUnityChan {
    public class MenuTabSideCover : GUIObjectBase {
        public Text text { get; set; }
        public RectTransform rect_transform { get; set; }
        public float anchor_x { get; set; }

        void Awake() {
            text = GetComponentInChildren<Text>();
            rect_transform = GetComponent<RectTransform>();
            anchor_x = rect_transform.anchoredPosition.x;
        }

        public void reset(float offset, float deg) {
            Sequence seq = DOTween.Sequence();

            // Back to default position
            seq.Append(rect_transform.DOAnchorPosX(anchor_x, 0.1f));
            seq.Join(rect_transform.DORotateQuaternion(Quaternion.Euler(0.0f, 0.0f, 0.0f), 0.1f));

            // Slide
            seq.Append(rect_transform.DOAnchorPosX(anchor_x + offset, 0.2f));
            // Rotate
            seq.Join(rect_transform.DORotateQuaternion(Quaternion.Euler(0.0f, 0.0f, deg), 0.2f));

            seq.Play();
        }
    }
}