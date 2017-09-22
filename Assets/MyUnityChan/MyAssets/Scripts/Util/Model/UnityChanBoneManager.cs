using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class UnityChanBoneManager : ObjectBase {
        public Dictionary<Const.ID.UnityChanBone, Bone> bones { get; protected set; }
        public Player player { get; protected set; }

        void Awake() {
            player = GetComponent<Player>();
            bones = new Dictionary<Const.ID.UnityChanBone, Bone>();

            Bone.register(Const.ID.UnityChanBone.HEAD, this);

            Bone.register(Const.ID.UnityChanBone.LEFT_SHOULDER, this);
            Bone.register(Const.ID.UnityChanBone.LEFT_ARM, this);
            Bone.register(Const.ID.UnityChanBone.LEFT_FORE_ARM, this);
            Bone.register(Const.ID.UnityChanBone.LEFT_HAND, this);

            Bone.register(Const.ID.UnityChanBone.RIGHT_SHOULDER, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_ARM, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_FORE_ARM, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND, this);

            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_THUMB_1, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_THUMB_2, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_THUMB_3, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_THUMB_4, this);

            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_INDEX_1, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_INDEX_2, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_INDEX_3, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_INDEX_4, this);

            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_MIDDLE_1, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_MIDDLE_2, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_MIDDLE_3, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_MIDDLE_4, this);

            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_RING_1, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_RING_2, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_RING_3, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_RING_4, this);

            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_PINKY_1, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_PINKY_2, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_PINKY_3, this);
            Bone.register(Const.ID.UnityChanBone.RIGHT_HAND_PINKY_4, this);
        }

        public Bone bone(Const.ID.UnityChanBone name) {
            return bones[name];
        }

        public Vector3 position(Const.ID.UnityChanBone name) {
            return bones[name].transform.position;
        }

        public string rootPath() {
            return "unitychan_dynamic/Character1_Reference";
        }
    }

    public class Bone : StructBase {
        public Const.ID.UnityChanBone id { get; protected set; }
        public string path { get; protected set; }
        public Transform transform { get; protected set; }

        private Bone(Const.ID.UnityChanBone _id, UnityChanBoneManager manager) {
            id = _id;
            path = manager.rootPath() + "/" + Const.UnityChanBone[id];
            transform = manager.player.transform.Find(path);
            manager.bones.Add(id, this);
        }

        public static Bone register(Const.ID.UnityChanBone _id, UnityChanBoneManager manager) {
            if ( !Const.UnityChanBone.ContainsKey(_id) ) {
                return null;
            }
            return new Bone(_id, manager);
        }
    }
}