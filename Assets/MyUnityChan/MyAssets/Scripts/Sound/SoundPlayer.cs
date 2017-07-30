using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : ObjectBase {
        private AudioSource audio_source;
        public bool locked { get; set; }

        void Awake() {
            audio_source = gameObject.GetComponent<AudioSource>();
            locked = false;
        }

        public void play(string resource_path, bool playOneShot=true, int delay = 0) {
            if ( locked ) return;
            AudioClip clip = SoundManager.Instance.getClip(resource_path);
            play(clip, playOneShot, delay);
        }

        public void play(Const.ID.SE sid, bool playOneShot=true, int delay = 0) {
            if ( locked ) return;
            if ( sid == Const.ID.SE._NO ) return;
            play(Const.Sound.SE[sid], playOneShot, delay);
        }

        public void play(AudioClip clip, bool playOneShot = true, int delay = 0) {
            if ( locked ) return;
            StartCoroutine(_play(clip, playOneShot, delay));
        }

        private IEnumerator _play(AudioClip clip, bool playOneShot = true, int delay = 0) {
            while ( delay > 0 ) {
                yield return null;
                delay--;
            }
            if ( !SettingManager.get(Settings.Flag.MUTE_SOUND) ) {
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
}
