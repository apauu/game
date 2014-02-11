using UnityEngine;
using System.Collections;

public class Enemy_Base : MonoBehaviour {

	//ジャンプフラグ trueならできる
	private bool jmpFlg = true;
	//攻撃１フラグ
	private bool attack1Flg = false;
	//右向きフラグ true = 右向き
	private bool rightDirectionFlg = true;
	//停止フラグ true = 停止する
	private bool stopFlg = false;
	//プレイヤー発見フラグ true = 発見
	private bool noticeFlg = false;
	//プレイヤーオブジェクト
	private GameObject player;
	
	//オブジェクト初期位置
	private Vector3 firstPosition;
	//プレイヤーとのx距離
	private float distanceX;
	//プレイヤーとのy距離
	private float distanceY;

	// Use this for initialization
	void Start () {
		//playerタグを検索してplayerオブジェクトを取得する
		player = GameObject.FindGameObjectWithTag (Tag_Const.PLAYER);
		firstPosition = this.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		Vector2 playerPosition = new Vector2(player.transform.position.x,player.transform.position.y); 
		//プレイヤーとのｘ距離を計算
		distanceX = Mathf.Abs (firstPosition.x-playerPosition.x);
		//プレイヤーとのｙ距離を計算
		distanceY = Mathf.Abs (firstPosition.y-playerPosition.y);

		//プレイヤーが初期位置から閾値以上離れている時
		if (distanceX > Enemy_Const.IMMOBILE_DISTANCE ||
		    distanceY > Enemy_Const.IMMOBILE_DISTANCE) {
			stopFlg = true;
		} else {
			stopFlg = false;
		}

		//停止しないとき　メイン処理
		if (!stopFlg) {
			float nowDistanceX = this.transform.position.x - playerPosition.x;
			//プレイヤーがどちら側にいるか判定
			if ((nowDistanceX) < 0) { 
				rightDirectionFlg = true;
			} else {
				rightDirectionFlg = false;
			}
			Side_Move.SideMove(rigidbody2D,Enemy_Const.ENEMY_SIDE_SPEED * ((rightDirectionFlg) ? 1 : -1));
		} else {
			float distance = this.transform.position.x - firstPosition.x;
			print(distance);
			if(Mathf.Abs(distance) > 0.1) {
				Side_Move.SideMove(rigidbody2D,Enemy_Const.ENEMY_SIDE_SPEED * ((distance <= 0) ? 1 : -1));
			}
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		//接地判定
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			if (collision.contacts != null && collision.contacts.Length > 0) {
				Vector2 contactPoint = collision.contacts[0].point;
				float angle = Vector2.Angle(new Vector2(0,-1),contactPoint - 
					                            new Vector2(this.transform.position.x,this.transform.position.y));
				if(Mathf.Abs(angle) >= 80f && Mathf.Abs(angle) < 100f){
					jmpFlg = false;
					Jump.JumpMove(rigidbody2D,10f);
				} else {
					jmpFlg = true;
				}
			}
		}
	}
}
