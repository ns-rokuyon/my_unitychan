using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Setting<T> : StructBase where T : struct {
        public T value { get; set; }
        private T default_value;
        public GameText title { get; private set; }
        public GameText desc { get; private set; }

        public Setting(T v, GameText t, GameText d) {
            value = v;
            default_value = v;
            title = t;
            if ( d == null ) {
                desc = title;
            }
            else {
                desc = d;
            }
        }

        public void setDefault() {
            value = default_value;
        }
    }
}