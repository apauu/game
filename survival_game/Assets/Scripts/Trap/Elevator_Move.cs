using UnityEngine;
using System.Collections;

public class Elevator_Move : MonoBehaviour {

	//エレベーターが2階にいるかどうか １階 = false
	private bool elevatorMoveFlg = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	// FixedUpdate is called once per frame
	void FixedUpdate () {
		//rigidbody2D.velocity =  new Vector2(500f,0);
	}

	void OnTriggerStay2D(Collider2D collider) {
		if(collider.tag.Equals(Tag_Const.PLAYER) && collider.transform.position.z > 0.5f) {
			//playerのレイヤーを変更 Event_Player　Groundと接触しないようにするため
			collider.gameObject.layer = 13;
			//
			gameObject.collider2D.isTrigger = false;
			//移動TweenのHashTable
			Hashtable table = new Hashtable();
			table.Add ("y", 35f);
			table.Add ("delay", .5);
			table.Add ("time", 5.0f);
			table.Add ("easetype", iTween.EaseType.easeInOutSine);
			table.Add ("oncomplete", "EndHandler");		// トゥイーン開始時にEndHandler()を呼ぶ
			table.Add ("oncompleteparams", collider);	
			iTween.MoveBy(gameObject, table);
		}
	}

	private void EndHandler(Collider2D collider) {
		gameObject.collider2D.isTrigger = true;
		//Playerのレイヤーを戻す
		collider.gameObject.layer = 8;
		//エレベータから出るときはfalseで呼び出し
		collider.gameObject.SendMessage ("EventElevator",false);
	}
}
