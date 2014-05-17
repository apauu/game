using UnityEngine;
using System.Collections;

public class Attack1_Object : MonoBehaviour {

	// 左右方向
	public bool is_right_local = true;
	// 進行スピード
	public float spd = 0.01f;
	// 威力
	public float damagePoint = 1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//受け取った方向を元に攻撃方向を決定
		if(is_right_local){
			transform.Translate(Vector3.right * spd);
		}else{
			transform.Translate(Vector3.left * spd);
		}
	}

	// HIT時処理
	void OnTriggerEnter2D (Collider2D collider) {
		if (this.gameObject.tag == Tag_Const.PLAYER_ATTACK) {
			if (collider.gameObject.tag == Tag_Const.ENEMY) {
				print ("Hit!!");
				//ダメージメソッド呼び出し
				collider.gameObject.SendMessage ("OnDamage", damagePoint);
				Destroy(this.gameObject);
			}
		} else {
			if (collider.gameObject.tag == Tag_Const.PLAYER) {
				print ("Hit!!");
				
				//ダメージメソッド呼び出し
				collider.gameObject.SendMessage ("OnDamage", damagePoint);
				Destroy(this.gameObject);
			}
		}
	}

	//方向を受け取る
	void SetDirection(bool isRight)
	{
		is_right_local = isRight;
	}
}