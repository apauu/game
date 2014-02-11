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
			if(collider.gameObject.tag == "Enemy_Attack"){
				print ("Parry!!");
				Destroy(collider.gameObject);
				//敵ひるみメソッド呼び出し
			}
		} else {
			if(collider.gameObject.tag == "Player_Attack"){
				print ("Parry!!");
				Destroy(collider.gameObject);
				//プレイヤーひるみメソッド呼び出し
			}
		}
	}
	
	//消滅までの時間を受け取る
	void setDestroyTime(float time)
	{
		destroyTime = time;
	}
}
