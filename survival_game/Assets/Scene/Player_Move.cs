using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//左右ボタンの入力
		float h = Input.GetAxisRaw ("Horizontal");
		if (h > 0f) {
			//右ボタンの時
			rigidbody2D.AddForce (Vector2.right * Const.PLAYER_SIDE_SPEED);
		} else if (h < 0f) {
			//左ボタンの時
			rigidbody2D.AddForce (Vector2.right * Const.PLAYER_SIDE_SPEED * -1);
		} else {
			//左右入力の無い時
			//rigidbody2D.velocity.x = Vector2.right * 0;
		}
	}
}
