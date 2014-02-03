using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {
	
	public GameObject player;
	float x;
	
	// Use this for initialization
	void Start () {
		x = player.transform.position.x - transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x - x, transform.position.y, transform.position.z);

		if(transform.position.x < 0){
			transform.position = new Vector3(0, transform.position.y, transform.position.z);
		}
		
		if(transform.position.x >= 18){
			transform.position = new Vector3(18, transform.position.y, transform.position.z);
		}
	}
}