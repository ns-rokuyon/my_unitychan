using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace MyUnityChan {
    public class MenuNavbarButtonTabPage : MenuNavbarButton<Const.ID.MenuTabPage> {
        private RectTransform rect_transform;
        private Button button;
        private Image image;

        public MenuTabPage tabpage { get; set; }
        public Color default_color { get; set; }
        public Color highlight_color { get; set; }

        public Const.ID.MenuTabPage tab_id {
            get { return nav; }
        }

        void Awake() {
            image = GetComponent<Image>();
            default_color = image.color;
            highlight_color = new Color32(255, 215, 150, 255);

            button = GetComponent<Button>();
            button.onClick.AddListener(focusTabByClick);

            rect_transform = GetComponent<RectTransform>();
        }
        
        void Start() {
            this.UpdateAsObservable()
                .Where(_ => MenuManager.self().isOpenedMenu )
                .Select(_ => tabpage.isFocused())
                .Subscribe(f => {
                    if ( f )
                        highlight();
                    else
                        unhighlight();
                });
        }

        public void focusTabByClick() {
            MenuManager.Instance.focus(tab_id);
        }

        public void highlight() {
            image.color = highlight_color;
            rect_transform.DOLocalMoveY(1.0f, 0.2f);
        }

        public void unhighlight() {
            image.color = default_color;
            rect_transform.DOLocalMoveY(-1.0f, 0.2f);
        }
    }
}