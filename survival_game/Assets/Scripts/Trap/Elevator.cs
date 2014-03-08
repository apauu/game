using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D collider) {
		float vertical = Input.GetAxis("Vertical");
		if (Input.GetButtonDown ("Vertical")) {
			//エレベーターメソッド呼び出し
			if(collider.gameObject.tag == Tag_Const.PLAYER) {
				//エレベータに入るときはtrueで呼び出し
				collider.gameObject.SendMessage ("EventElevator",true);
			}
		}
	}
}
