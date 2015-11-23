using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    // http://answers.unity3d.com/questions/706525/is-there-anyway-to-resetclear-trail-renderer.html
    public static class TrailRendererExtension {
        public static void reset(this TrailRenderer trail, ObjectBase instance) {
            instance.StartCoroutine(resetTrail(trail));
        }

        private static IEnumerator resetTrail(TrailRenderer trail) {
            var time = trail.time;
            trail.time = 0;
            yield return new WaitForEndOfFrame();
            trail.time = time;
        }

    }
}
