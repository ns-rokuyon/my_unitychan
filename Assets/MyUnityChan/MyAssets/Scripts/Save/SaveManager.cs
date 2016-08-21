using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SaveManager : SingletonObjectBase<SaveManager> {
        public SaveDataSerializer serializer { get; private set; }

        void Awake() {
            serializer = new SaveDataConsoleTextSerializer();
        }

        public static void save() {
            Instance.serializer.serialize();
        }

        public static void load() {
            Instance.serializer.deserialize();
        }
    }
}