using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class MapConnectionVisualizerElement : MapElement {

        public GameObject pointA { get; set; }
        public GameObject pointB { get; set; }
        public float time_diff;

        void Awake() {
            var renderer = GetComponent<Renderer>();
            renderer.sortingOrder = 900;
        }

        void Start() {
            Observable.EveryUpdate()
                .Where(t => PauseManager.isPausing())
                .Subscribe(t => {
                    var time = (float)t * 0.01f - time_diff;
                    float pingpong = Mathf.PingPong(time, 1.0f);
                    Vector3 newpos = new Vector3(
                        Mathf.Lerp(pointA.transform.position.x, pointB.transform.position.x, pingpong),
                        Mathf.Lerp(pointA.transform.position.y, pointB.transform.position.y, pingpong),
                        Mathf.Lerp(pointA.transform.position.z, pointB.transform.position.z, pingpong));
                    transform.position = newpos.add(0, 0, 0.01f);
                }
            );
        }

        public void brighten() {
            var mat = GetComponent<MeshRenderer>().materials[0];
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
        }
    }
}