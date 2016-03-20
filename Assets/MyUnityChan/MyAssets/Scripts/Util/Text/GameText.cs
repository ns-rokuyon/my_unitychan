using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class GameText : StructBase {
        public string jp;
        public string en;

        GameText(string text, bool injp=true) {
            if ( injp ) {
                jp = text;
                en = null;
            }
            else {
                jp = null;
                en = text;
            }
        }

        GameText(string jptext, string entext) {
            jp = jptext;
            en = entext;
        }

        public static GameText text(string text, bool injp = true) {
            return new GameText(text, injp);
        }

        public static GameText text(string jptext, string entext) {
            return new GameText(jptext, entext);
        }

        public string get() {
            if ( jp == null && en == null ) {
                Debug.LogWarning("GameText is not set");
                return "";
            }

            switch ( GameStateManager.self().language ) {
                case Const.Language.JP:
                    if ( jp == null ) return en;
                    return jp;
                case Const.Language.EN:
                    if ( en == null ) return jp;
                    return en;
            }

            return "";
        }
    }
}
