using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class InvokerManager : SingletonObjectBase<InvokerManager> {
        public GameObject frame_delay_invoker_prefab;
        public GameObject frame_delay_vector3_invoker_prefab;

        void Start() {
        }

        public static void createFrameDelayInvoker(int frame, FrameDelayInvoker.DelayDelegate func) {
            FrameDelayInvoker invoker =
                (Instantiate(Instance.frame_delay_invoker_prefab) as GameObject).setParent(Hierarchy.Layout.INVOKER).GetComponent<FrameDelayInvoker>();
            invoker.set(frame, func);
        }

        public static void createFrameDelayVector3Invoker(int frame, Vector3 param, FrameDelayVector3Invoker.DelayParamDelegate func) {
            FrameDelayVector3Invoker invoker =
                (Instantiate(Instance.frame_delay_vector3_invoker_prefab) as GameObject).setParent(Hierarchy.Layout.INVOKER).GetComponent<FrameDelayVector3Invoker>();
            invoker.set(frame, param, func);
        }

        public static void createCoroutine(IEnumerator f) {
            Instance.startCoroutine(f);
        }

        public void startCoroutine(IEnumerator f) {
            StartCoroutine(f);
        }
    }
}
