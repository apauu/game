using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour {
	
	public GameObject hpbar;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D col){
		//  on damage
		if(col.gameObject.tag == "Enemy"){
			print("EnemyHit");
			hpbar.gameObject.SendMessage("onDamage", 0.5);
		}
	}
}