using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GameText : StructBase {
        public string jp { get; private set; }
        public string en { get; private set; }

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
            if ( jp == null ) {
                return en;
            }
            return jp;
        }
    }
}
