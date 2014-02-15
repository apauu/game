using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour {

	private GameObject player;
	private Vector3 playerPositon;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x,player.transform.position.y+5f, transform.position.z);
		
	}
}
