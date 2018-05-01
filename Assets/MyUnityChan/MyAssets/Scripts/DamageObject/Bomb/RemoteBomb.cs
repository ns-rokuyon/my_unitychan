using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class RemoteBomb : Bomb, ICommunicatableBomb, IGUIWorldSpaceUILinked {
        public bool ready { get; private set; }
        public PointingLineBuildingBlock guide { get; private set; }

        void Start() {
            initialize();
        }

        public GameObject createWorldUI(Vector3 pos) {
            // Create World UI
            if ( !guide ) {
                GameObject guide_obj = PrefabInstantiater.createWorldUI(Const.Prefab.UI["WORLD_SPACE_CONTROLLER_GUIDE"], Vector3.one);
                guide = guide_obj.GetComponent<PointingLineBuildingBlock>();
            }
            guide.gameObject.transform.position = transform.position;

            // Set infomation to controller guide
            ControllerGuide cg = guide.GetComponentInChildren<ControllerGuide>();
            cg.set(Controller.InputCode.ATTACK, "Detonate");

            guide.open();

            return guide.gameObject;
        }

        public override void initialize() {
            ready = true;
        }

        public override void finalize() {
            ready = false;
            if ( guide )
                guide.terminate();
        }

        public override Vector3 getInitPosition(Transform owner) {
            return owner.position.add(owner.forward.x * 0.5f, 0.2f, 0);
        }

        public bool communicate(Bomber bomber) {
            if ( !ready )
                return false;

            explode();
            bomber.reloadFull();
            ready = false;
            if ( guide )
                guide.terminate();
            return true;
        }
    }
}