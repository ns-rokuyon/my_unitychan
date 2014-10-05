using UnityEngine;
using System.Collections;

public class EffectBase : ObjectBase {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void init(Vector3 pos) {
		transform.position = pos;
	}
}
