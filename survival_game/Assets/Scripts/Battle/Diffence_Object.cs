using UnityEngine;
using System.Collections;

public class Diffence_Object : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print (gameObject.tag);
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (gameObject.tag==Tag_Const.PLAYER_DIFFENCE) {
			if(collider.gameObject.tag == Tag_Const.ENEMY_ATTACK) {
				print ("Defense!!");
				Destroy(collider.gameObject);
			} else if (collider.gameObject.tag == Tag_Const.ENEMY_DIFFENCE_BREAK_ATTACK) {
				print ("defense is destroyed");
				Destroy(this.gameObject);
			}
		} else {
			if(collider.gameObject.tag == Tag_Const.PLAYER_ATTACK){
				print ("Defense!!");
				Destroy(collider.gameObject);
			} else if (collider.gameObject.tag == Tag_Const.PLAYER_DIFFENCE_BREAK_ATTACK) {
				print ("defense is destroyed");
				Destroy(this.gameObject);
			}
		}
	}
}
