using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyUnityChan {
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    public class Area : ObjectBase {
        // set true to limit player moving in this area
        public bool roof;
        public bool floor;
        public bool left_wall;
        public bool right_wall;
        public bool auto_zoom;
        public bool limit_by_viewport_corners;

        public float baselineZ = float.NaN;

        public List<GameObject> bound_objects;  // TODO: Remove
        public List<GameObject> area_objects;

        private Dictionary<string, bool> ins;
        protected List<AreaConnection> connections;

        protected List<Area> connected_areas;
        public Dictionary<string, GameObject> gameobjects { get; protected set; }

        protected float x_harf;
        protected float y_harf;
        protected float z_harf;

        // Camera position
        [SerializeField] public PlayerCameraPosition camera_position;

        // Area Status
        public bool passed { get; set; }


        void Awake() {
            ins = new Dictionary<string, bool>();
            connections = new List<AreaConnection>();
            connected_areas = new List<Area>();
            gameobjects = new Dictionary<string, GameObject>();
            Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;
            x_harf = (float)(bounds.size.x / 2.0);
            y_harf = (float)(bounds.size.y / 2.0);
            z_harf = (float)(bounds.size.z / 2.0);

            passed = false;
        }

        void Start() {
            labeling();

            // Auto player labeling
            this.ObserveEveryValueChanged(_ => AreaManager.self().now_area_name)
                .Subscribe(now_area_name => {
                    Player p = GameStateManager.getPlayer();
                    if ( !p )
                        return;
                    p.manager.players.Values.ToList().ForEach(obj => {
                        relabel(obj, now_area_name);
                    });
                });
        }

        public void relabel(GameObject obj, string now_area_name) {
            string key = gameobject2Key(obj);
            if ( now_area_name == gameObject.name ) {
                ObjectBase[] comps = obj.GetComponents<ObjectBase>();
                comps.ToList().ForEach(comp => { comp.parent_area = this; });
                if ( !gameobjects.ContainsKey(key) ) {
                    gameobjects.Add(key, obj);
                }
            }
            else {
                if ( gameobjects.ContainsKey(key) ) {
                    gameobjects.Remove(key);
                }
            }
        }

        public bool isIn(string name) {
            if ( !ins.ContainsKey(name) ) {
                return false;
            }
            return ins[name];
        }

        public bool isIn(Player player) {
            return isIn(player.gameObject.name);
        }

        public bool isIn(Vector3 pos) {
            MeshCollider mesh_collider = GetComponent<MeshCollider>();
            if ( mesh_collider ) {
                // If this area has MeshCollider
                return mesh_collider.bounds.Contains(pos);
            }

            Vector3 area_center = transform.position;
            if ( area_center.x - x_harf > pos.x || area_center.x + x_harf < pos.x ) {
                return false;
            }
            if ( area_center.y - y_harf > pos.y || area_center.y + y_harf < pos.y ) {
                return false;
            }
            if ( area_center.z - z_harf > pos.z || area_center.z + z_harf < pos.z ) {
                return false;
            }
            return true;
        }

        public void labeling(GameObject parent = null) {
            if ( parent == null ) {
                labeling(transform.parent.gameObject);
                return;
            }

            var obs = parent.GetComponents<ObjectBase>();
            if ( obs.Length > 0 ) {
                if ( gameobjects.ContainsKey(gameobject2Key(parent))) {
                    DebugManager.warn("Key(" + gameobject2Key(parent) + ") already exists");
                }
                else {
                    gameobjects.Add(gameobject2Key(parent), parent);
                }
            }
            obs.ToList().ForEach(ob => {
                ob.parent_area = this;
            });

            for ( int i = 0; i < parent.transform.childCount; i++ ) {
                GameObject child = parent.transform.GetChild(i).gameObject;
                obs = child.GetComponents<ObjectBase>();
                if ( obs.Length > 0 ) {
                    if ( gameobjects.ContainsKey(gameobject2Key(child))) {
                        DebugManager.warn("Key(" + gameobject2Key(child) + ") already exists");
                    }
                    else {
                        gameobjects.Add(gameobject2Key(child), child);
                    }
                }
                obs.ToList().ForEach(ob => {
                    ob.parent_area = this;
                });
                if ( child.transform.childCount > 0 ) {
                    labeling(parent: child);
                }
            }
        }

        public string gameobject2Key(GameObject o) {
            return o.getHierarchyPath();
        }

        public int getActiveEnemyCount() {
            return gameobjects.Count(pair => {
                var en = pair.Value.GetComponent<Enemy>();
                if ( !en )
                    return false;
                return en.gameObject.activeSelf;
            });
        }

        public bool isSetBounds() {
            if ( bound_objects.Count == 0 ) return false;
            return true;
        }

        public float getBaselineZ() {
            return baselineZ;
        }

        public bool isEmptyBaselineZ() {
            return float.IsNaN(baselineZ);
        }

        public bool isAutoZoom() {
            return auto_zoom;
        }

        public bool isPassed() {
            return passed;
        }

        public float limitLeft() {
            return gameObject.transform.position.x - x_harf;
        }

        public float limitRight() {
            return gameObject.transform.position.x + x_harf;
        }

        public float limitRoof() {
            return gameObject.transform.position.y + y_harf;
        }

        public float limitFloor() {
            return gameObject.transform.position.y - y_harf;
        }

        private void register(string name) {
            if ( !ins.ContainsKey(name) ) {
                ins[name] = false;
            }
        }

        public List<AreaConnection> getAreaConnections() {
            return connections;
        }

        public void addAreaConnections(GameObject from, GameObject to) {
            AreaConnection conn = new AreaConnection(from, to);
            connections.Add(conn);
            AreaConnection.mergeUndirectedConnection(connections, conn);
        }

        public void addConnectedArea(Area area) {
            connected_areas.Add(area);
        }

        public List<Area> getNeighborhoodArea() {
            return connected_areas;
        }

        public void activateGameObjects() {
            List<GameObject> disabled_objs = 
                gameobjects.Values.ToList().FindAll(obj => obj.GetComponent<NPCharacter>() && obj.GetComponent<NPCharacter>().getHP() > 0);
            disabled_objs.ForEach(obj => obj.SetActive(true));

            List<GameObject> respawn_objs =
                gameobjects.Values.ToList().FindAll(obj => obj.GetComponent<Spawnable>() && 
                    obj.GetComponent<Enemy>() && obj.GetComponent<Enemy>().getHP() == 0);
            respawn_objs.ForEach(obj => { obj.SetActive(true); obj.GetComponent<Enemy>().spawn(); });
        }

        public void deactivateGameObjects() {
            gameobjects.Values
                .Where(o => o.GetComponent<NPCharacter>() != null)
                .ToList()
                .ForEach(obj => obj.SetActive(false));
        }

        public virtual void OnTriggerEnter(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                // Player entered into this area or player switched in this area
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                if ( !player.playable )
                    return;
                string before_area = player.getAreaName();
                player.setAreaName(this.gameObject.name);
                string name = player.gameObject.name;
                register(name);
                ins[name] = true;
                passed = true;

                AreaManager.self().reportPlayerEntered(this);

                // Update player's position in area change
                player.last_entrypoint = player.gameObject.transform.position;

                // Adjust player's camera position to that in this area
                var camera = player.getPlayerCamera();
                if ( camera )
                    camera.setPositionInArea(this);

                if ( before_area != player.getAreaName() ) {
                    // Activate, Activate(respawn), or Deactivate enemies
                    AreaManager.self().manageGameObjectsInArea(this, connected_areas);
                }
            }
            else if ( colliderInfo.gameObject.tag == "Enemy" ) {
                Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                enemy.setAreaName(this.gameObject.name);
            }
        }

        public void OnTriggerExit(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ||
                 colliderInfo.gameObject.tag == "Enemy" ) {
                string name = colliderInfo.gameObject.name;
                ins[name] = false;
            }
        }

    #if UNITY_EDITOR
        public virtual void sceneGUI() {
            if ( !isEmptyBaselineZ() ) {
                Vector3 area_center = transform.position;
                Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;
                x_harf = (float)(bounds.size.x / 2.0);
                y_harf = (float)(bounds.size.y / 2.0);
                Vector3 left_point = new Vector3(area_center.x - x_harf, area_center.y, baselineZ);
                Vector3 right_point = new Vector3(area_center.x + x_harf, area_center.y, baselineZ);
                left_point = Handles.PositionHandle(left_point, Quaternion.identity);
                right_point = Handles.PositionHandle(right_point, Quaternion.identity);
                Vector3[] points = new Vector3[] { left_point, right_point };
                Handles.color = Color.blue;
                Handles.DrawAAPolyLine(10, points);
            }
        }
    #else
        public virtual void sceneGUI() {
        }
    #endif

        public void inspectorGUI(){}

    }
}
