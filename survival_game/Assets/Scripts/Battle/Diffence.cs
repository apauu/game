using UnityEngine;
using System.Collections;

public class Diffence : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print (gameObject.tag);
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (gameObject.tag=="Player_Diffence") {
			if(collider.gameObject.tag == "Enemy_Attack"){
				Destroy(collider.gameObject);
			}
		} else {
			if(collider.gameObject.tag == "Player_Attack"){
				Destroy(collider.gameObject);
			}
		}
		print ("diffence!!");
	}
}
