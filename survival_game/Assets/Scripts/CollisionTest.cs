using UnityEngine;
using System.Collections;

public class CollisionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D collider) {
		Debug.Log ("destoy!!");
		}

	void OnTriggerEnter2D (Collider2D collider) {
		Debug.Log ("HitHit");
	}
}
