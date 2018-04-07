using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyUnityChan {
    public class ControllerGuideBox : GUIObjectBase, IGUIOpenable {
        [SerializeField]
        private GameObject guide_prefab;

        [SerializeField]
        private List<GuideDef> guide_defs;

        public RectTransform rect_transform { get; protected set; }
        public List<ControllerGuide> guides { get; protected set; }

        void Awake() {
            rect_transform = GetComponent<RectTransform>();
            guides = new List<ControllerGuide>();
            DebugManager.log("GuideBox.Awake!!");
        }

        public void open() {
            var controller = GameStateManager.pm.controller as PlayerController;
            var keyconfig = controller.keyconfig;

            for ( int i = 0; i < guide_defs.Count; i++ ) {
                var guide_def = guide_defs[i];
                var obj = Instantiate(guide_prefab, rect_transform) as GameObject;
                var rt = obj.GetComponent<RectTransform>();
                var guide = obj.GetComponent<ControllerGuide>();

                float message_text_width = guide.message_text.rectTransform.rect.width;
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + message_text_width * i,
                                                  rt.anchoredPosition.y);
                guide.fixed_with_respect_to_dynamic_move = fixed_with_respect_to_dynamic_move;
                guide.set(guide_def.code, guide_def.message.get(), keyconfig);
                guides.Add(guide);
            }
        }

        public void close() {
            guides.ForEach(guide => {
                Destroy(guide.gameObject);
            });
            guides.Clear();
        }

        public void terminate() {
            close();
        }

        public bool authorized(object obj) {
            return true;
        }

        public GameObject getGameObject() {
            return gameObject;
        }

        [System.Serializable]
        public class GuideDef {
            [SerializeField]
            public Controller.InputCode code;

            [SerializeField]
            public GameText message;
        }
    }

}
