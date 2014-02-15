using UnityEngine;
using System.Collections;

public class Parry_Object : MonoBehaviour {
	
	public float destroyTime = 1f;

	// Use this for initialization
	void Start () {
		print (gameObject.tag);
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject, destroyTime);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (gameObject.tag==Tag_Const.PLAYER_PARRY) {
			if(collider.gameObject.tag == Tag_Const.ENEMY_ATTACK){
				print ("Parry!!");
				Destroy(collider.gameObject);
				//敵ひるみメソッド呼び出し
			} else if (collider.gameObject.tag == Tag_Const.ENEMY_LONG_ATTACK){
				print ("failed parry");
			}
		} else {
			if(collider.gameObject.tag == Tag_Const.PLAYER_ATTACK){
				print ("Parry!!");
				Destroy(collider.gameObject);
				//プレイヤーひるみメソッド呼び出し
			} else if (collider.gameObject.tag == Tag_Const.PLAYER_LONG_ATTACK){
				print ("failed parry");
			}
		}
	}
	
	//消滅までの時間を受け取る
	void setDestroyTime(float time)
	{
		destroyTime = time;
	}
}
