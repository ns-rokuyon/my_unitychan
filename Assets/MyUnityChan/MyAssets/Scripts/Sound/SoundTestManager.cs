using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    [RequireComponent(typeof(SoundPlayer))]
    public class SoundTestManager : SingletonObjectBase<SoundTestManager> {
        public GameObject category_dropdown_object;
        public GameObject title_text_object;
        
        public SoundPlayer sound_player { get; protected set; }
        public Dropdown category_dropdown { get; protected set; }
        public Text title_text { get; protected set; }
        public Const.ID.SoundTestCategory category { get; protected set; }
        public int cursor;

        void Start() {
            cursor = 0;
            category_dropdown = category_dropdown_object.GetComponent<Dropdown>();
            title_text = title_text_object.GetComponent<Text>();
            sound_player = GetComponent<SoundPlayer>();
            this.ObserveEveryValueChanged(_ => category_dropdown.value)
                .Subscribe(v => {
                    category = (Const.ID.SoundTestCategory)v;
                    cursor = 0;
                    play();
                });
        }

        public void prev() {
            cursor--;
            if ( cursor < 0 )
                cursor = nClips() == 0 ? 0 : nClips() - 1;
            play();
        }

        public void next() {
            cursor++;
            if ( cursor >= nClips() )
                cursor = 0;
            play();
        }

        public int nClips() {
            switch ( category ) {
                case Const.ID.SoundTestCategory.SE:
                    return System.Enum.GetValues(typeof(Const.ID.SE)).Length;
                case Const.ID.SoundTestCategory.UNITYCHAN_VOICE:
                    return System.Enum.GetValues(typeof(Const.ID.PlayerVoice)).Length;
                default:
                    break;
            }
            return 0;
        }

        public void play() {
            if ( nClips() < 1 ) {
                title_text.text = "";
                return;
            }
            string title = "" + (cursor + 1) + " / " + nClips() + "\n"; 
            switch ( category ) {
                case Const.ID.SoundTestCategory.SE:
                    sound_player.play((Const.ID.SE)cursor);
                    title_text.text = title + ((Const.ID.SE)cursor).ToString();
                    break;
                case Const.ID.SoundTestCategory.UNITYCHAN_VOICE:
                    AudioClip clip = AssetBundleManager.get(Const.ID.AssetBundle.UNITYCHAN_VOICE,
                        Const.Sound.Voice.UnityChan[(Const.ID.PlayerVoice)cursor]) as AudioClip;
                    sound_player.play(clip);
                    title_text.text = title + ((Const.ID.PlayerVoice)cursor).ToString();
                    break;
                default:
                    break;
            }
        }

    }
}