using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class Tuple<T1,T2> where T1 : class where T2 : class {
        public T1 _1;
        public T2 _2;

        public Tuple (T1 _a, T2 _b) {
            _1 = _a;
            _2 = _b;
        }

        public int size() {
            return 2;
        }
    }

    [System.Serializable]
    public class KV<TKey, TValue> {
        [SerializeField] public TKey key;
        [SerializeField] public TValue value;

        public KV(TKey k, TValue v) {
            key = k;
            value = v;
        }
    }
}