using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class SoundManager : SingletonObjectBase<SoundManager> {
        private Dictionary<string, AudioClip> clips;

        void Awake() {
            clips = new Dictionary<string, AudioClip>();
        }

        public AudioClip getClip(string resource_path) {
            if ( !clips.ContainsKey(resource_path) ) {
                clips[resource_path] = Resources.Load(resource_path) as AudioClip;
                if ( clips[resource_path] == null ) {
                    Debug.Log("Cannot load audio clip from " + resource_path);
                }
            }
            return clips[resource_path];
        }
    }
}
