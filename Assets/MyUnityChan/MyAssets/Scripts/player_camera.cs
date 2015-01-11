using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class player_camera : MonoBehaviour {
        GameObject player;

        // Use this for initialization
        void Start() {
            player = GameObject.Find("unitychan");
        }

        // Update is called once per frame
        void Update() {
            //transform.LookAt (player.transform);
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10, transform.position.z);
        }
    }
}
