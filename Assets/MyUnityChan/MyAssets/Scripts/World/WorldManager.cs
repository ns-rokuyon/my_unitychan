using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class WorldManager : SingletonObjectBase<WorldManager> {

        void Awake() {
            addTagToGroundObjects();
        }

        private void addTagToGroundObjects() {
            var objs = GetComponentsInChildren<Collider>()
                .Where(col => !col.isTrigger)
                .Select(col => col.gameObject)
                .Where(obj => obj.tag == "Untagged");
            foreach ( GameObject o in objs ) {
                o.tag = "Ground";
            }
        }
    }
}