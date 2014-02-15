using UnityEngine;
using System.Collections;

public class Attack1_Object : MonoBehaviour {
	
	public bool is_right_local = true;
	public float spd = 0.3f;
	public float destroyTime = 1f;
	public float damagePoint = 1f;

	// Use this for initialization
	void Start () {
		print (gameObject.tag);
	}
	
	// Update is called once per frame
	void Update () {

		//受け取った方向を元に攻撃方向を決定
		if(is_right_local){
			transform.Translate(Vector3.right * spd);
		}else{
			transform.Translate(Vector3.left * spd);
		}
		Destroy(gameObject, destroyTime);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		//プレイヤー
		if (this.gameObject.tag==Tag_Const.PLAYER_ATTACK) {
			if(collider.gameObject.tag == Tag_Const.ENEMY){
				print ("Hit!!");
				//ダメージメソッド呼び出し
				collider.gameObject.SendMessage("onDamage", damagePoint);
			}

			
		//エネミー
		} else if (this.gameObject.tag==Tag_Const.ENEMY_ATTACK) {
			if(collider.gameObject.tag == Tag_Const.PLAYER){
				print ("Hit!!");
				//ダメージメソッド呼び出し
				collider.gameObject.SendMessage("onDamage", damagePoint);
			}
		}
	}

	//方向を受け取る
	void setDirection(bool isRight)
	{
		is_right_local = isRight;
	}
	
	//消滅までの時間を受け取る
	void setDestroyTime(float time)
	{
		destroyTime = time;
	}
}