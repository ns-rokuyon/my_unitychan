using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class SingletonObjectBase<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T instance;

        // http://warapuri.tumblr.com/post/28972633000/unity-50-tips
        public static T Instance {
            get {
                if ( instance == null ) {
                    instance = (T)FindObjectOfType(typeof(T));
                    if ( instance == null ) {
                        Debug.LogError("An instance of " + typeof(T) + "is needed in the scene, but there is none.");
                    }
                }
                return instance;
            }
        }

        public static T self() {
            return Instance.GetComponent<T>();
        }

        public void delay(int frame, System.Action func, FrameCountType frame_count_type = FrameCountType.Update) {
            if ( frame > 0 ) {
                Observable.TimerFrame(frame, frame_count_type)
                    .Subscribe(_ => {
                        func();
                    })
                    .AddTo(this);
            }
            else {
                func();
            }
        }
    }
}
