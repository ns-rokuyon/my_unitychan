using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {

    // Simple struct for connection between 2 areas
    public class AreaConnection : StructBase {
        public enum Graph {
            UNDIRECTED,
            A2B
        }

        public Vector3 pointA;
        public Vector3 pointB;
        public Graph graph;

        public AreaConnection(Vector3 pa, Vector3 pb, Graph graph_=Graph.A2B) {
            pointA = pa;
            pointB = pb;
            graph  = graph_;
        }

        public AreaConnection(GameObject obja, GameObject objb, Graph graph_=Graph.A2B) {
            pointA = obja.transform.position;
            pointB = objb.transform.position;
            graph  = graph_;
        }

        public static void mergeUndirectedConnection(List<AreaConnection> list, AreaConnection target) {
            int rnum = list.RemoveAll(conn => conn.pointB == target.pointA && conn.pointA == target.pointB);
            if ( rnum == 0 ) {
                return;
            }

            bool isRemoved = list.Remove(target);
            if ( isRemoved ) {
                list.Add(new AreaConnection(target.pointA, target.pointB, Graph.UNDIRECTED));
            }
            else {
                Debug.LogError("target could not be removed");
            }
        }
    }
}
