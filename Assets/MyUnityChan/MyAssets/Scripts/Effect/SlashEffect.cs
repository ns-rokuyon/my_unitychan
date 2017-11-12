using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class SlashEffect : Effect {
        [SerializeField]
        public List<Rot> rots;

        [System.Serializable]
        public class Rot : KV<Const.ID.SlashType, Vector3> {
            public Rot(Const.ID.SlashType k, Vector3 v) : base(k, v) {
            }

            public Vector3 vector {
                get { return value; }
            }
        }

        public void rotate(Const.ID.SlashType st, bool mirror = false) {
            Rot rot = rots.First(r => r.key == st);
            if ( mirror )
                transform.rotation = Quaternion.Euler(rot.vector.add(0, 180, 0));
            else
                transform.rotation = Quaternion.Euler(rot.vector);
        }
    }
}