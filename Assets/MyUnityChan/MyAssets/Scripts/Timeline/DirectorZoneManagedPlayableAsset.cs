using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System;

namespace MyUnityChan {
    public class DirectorZoneManagedPlayableAsset : PlayableAsset {
        public ExposedReference<GameObject> clip;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var zone = owner.GetComponent<DirectorZone>();
            var behaviour = new DirectorZoneManagedPlayableBehaviour();
            behaviour.clip = clip.Resolve(graph.GetResolver());
            behaviour.director_zone = zone;
            return ScriptPlayable<DirectorZoneManagedPlayableBehaviour>.Create(graph, behaviour);
        }
    }
}