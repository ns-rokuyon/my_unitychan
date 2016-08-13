using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class MapConnectionPointElement : MapElement {
        public GameObject pair;     // Preseted by MapBuilder

        public GameObject connection_visualizer { get; set; }
        public AreaGate gate { get; set; }

        void Awake() {
            connection_visualizer = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_VISUALIZER"],
                                                              MapBuilder.self().build_to);
            connection_visualizer.transform.position = transform.position;
            connection_visualizer.transform.localScale = transform.localScale;
            connection_visualizer.layer = MapBuilder.self().layer();
            var comp = connection_visualizer.GetComponent<MapConnectionVisualizerElement>();
            comp.pointA = this.gameObject;
            comp.pointB = pair;
        }

        void Start() {
            gate.ObserveEveryValueChanged(g => g.pass).Subscribe(_ => {
                connection_visualizer.GetComponent<MapConnectionVisualizerElement>().brighten();
            });
        }

    }
}
