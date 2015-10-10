using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SoundPlayer : ObjectBase {
        private AudioSource audio_source;

        void Awake() {
            audio_source = gameObject.GetComponent<AudioSource>();
        }

        public void play(string resource_path, bool playOneShot=false) {
            if ( audio_source == null ) {
                Debug.LogError("AudioSource is not attached");
                return;
            }
            AudioClip clip = SoundManager.Instance.getClip(resource_path);
            if ( playOneShot ) {
                audio_source.PlayOneShot(clip);
            }
            else {
                audio_source.clip = clip;
                audio_source.Play();
            }
        }
    }
}
