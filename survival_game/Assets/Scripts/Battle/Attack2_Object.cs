using UnityEngine;
using System.Collections;

public class Attack2_Object : MonoBehaviour {
	
	public bool is_right_local = true;
	public float spd = 0.000000001f;
	public float destroyTime = 1f;
	public float damagePoint = 1f;
	private float initialTime = 3f;
	private Vector3 direction = new Vector3(1 ,1 ,0);
	private float randomRangeX = 10f;
	private float randomRangeY = 8f;

	//キャラクターオブジェクト
	private GameObject character;
	
	// Use this for initialization
	void Start () {
		print (gameObject.tag);
		// このコードを適用したgameObjectを5秒掛けて(xyz)の位置まで移動させる
		iTween.MoveTo(gameObject, getRandomVector(randomRangeX, randomRangeY), 3.0f);
		Invoke("changeVector", initialTime);
	}
	
	//ランダムベクトル生成
	Vector3 getRandomVector (float x, float y) {
		Vector3 vec3 =  new Vector3(Random.value * x - (x / 2), Random.value * y, 0);
		return transform.position + vec3;
	}

	//ディレイアクション
	void changeVector () {
		//ENEMYタグを検索してENEMYオブジェクトを取得する
		character = GameObject.FindGameObjectWithTag (Tag_Const.ENEMY);
		if (character != null) {
			direction = character.transform.position;
			iTween.MoveBy(gameObject, direction - transform.position, 3.0f);
		} else {
			direction = transform.position;
			// 現在角度からy軸260度までランダムに回転する
			//iTween.ShakePosition(gameObject, iTween.Hash("y", 1, "time", 3.0f));
			iTween.ShakePosition(gameObject, iTween.Hash("x", .1, "y", .1,"z", .1, "time", 1.0f, "delay", 2.5f));
			iTween.ShakeRotation(gameObject, iTween.Hash("y", 360, "x", 360, "time", 3.0f));
			//フェードアウト
			iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", 5f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//受け取った方向を元に攻撃方向を決定
		if(is_right_local){
			//transform.Translate(direction * spd);
		}else{
			//transform.Translate(new Vector3(direction.x * spd * -1, direction.y, direction.z) * spd);
			//transform.Translate(direction * spd);
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