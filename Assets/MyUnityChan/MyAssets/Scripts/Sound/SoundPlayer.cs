﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : ObjectBase {
        private AudioSource audio_source;

        void Awake() {
            audio_source = gameObject.GetComponent<AudioSource>();
        }

        public void play(string resource_path, bool playOneShot=true, int delay = 0) {
            AudioClip clip = SoundManager.Instance.getClip(resource_path);
            play(clip, playOneShot, delay);
        }

        public void play(AudioClip clip, bool playOneShot = true, int delay = 0) {
            StartCoroutine(_play(clip, playOneShot, delay));
        }

        private IEnumerator _play(AudioClip clip, bool playOneShot = true, int delay = 0) {
            while ( delay > 0 ) {
                yield return null;
                delay--;
            }
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
