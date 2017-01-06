using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class MapConnectionPointElement : MapElement {
        public GameObject pair;     // Preseted by MapBuilder
        public string gate_path; 
        
        public GameObject connection_visualizer { get; set; }
        public GameObject gate { get; set; }
        public AreaGate _gate { get; set; }

        void Awake() {
            connection_visualizer = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_VISUALIZER"],
                                                              MapBuilder.self().build_to);
            connection_visualizer.transform.position = transform.position;
            connection_visualizer.transform.localScale = transform.localScale;
            connection_visualizer.layer = MapBuilder.self().layer();
            var comp = connection_visualizer.GetComponent<MapConnectionVisualizerElement>();
            comp.pointA = this.gameObject;
            comp.pointB = pair;

            gate = GameObject.Find(gate_path);
            _gate = gate.GetComponent<AreaGate>();
        }

        void Start() {
            var r = GetComponent<Renderer>();
            DebugManager.log(r.sortingOrder);
            this.ObserveEveryValueChanged(_ => _gate.pass)
                .Where(b => b)
                .Subscribe(_ => {
                    connection_visualizer.GetComponent<MapConnectionVisualizerElement>().brighten();
                });
        }

    }
}
