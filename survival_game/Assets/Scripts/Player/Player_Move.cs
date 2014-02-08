using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {

	//現在向いている方向
	private bool migiMukiFlg = true;

	//ジャンプフラグ trueならできる
	private bool jmpFlg = true;
	//二段ジャンプフラグ trueならできる
	private bool doubleJmpFlg = true;
	//防御フラグ
	private bool diffenceFlg = true;
	
	//防御プレハブ
	public GameObject diffencePrefab;
	//防御オブジェクト
	private GameObject diffenceObj;

	//前回のキーを離してからの時間
	private float lastKeyTimer = 0;
	//現在押されている左右移動キー
	private float nowRawKey = 0;
	//前回押した左右移動キー
	private float lastRawKey = 0;
	//ダッシュ中フラグ
	private bool fgDash = false;

	//攻撃１フラグ
	private bool attack1Flg = false;
	//攻撃２フラグ
	private bool attack2Flg = false;
	//攻撃３フラグ
	private bool attack3Flg = false;
	//ジャンプ中攻撃１フラグ
	private bool jumpAttack1Flg = false;
	//ジャンプ中攻撃２フラグ
	private bool jumpAttack2Flg = false;
	//パリィフラグ
	private bool parryFlg = false;
	//パリィ攻撃１フラグ
	private bool parryAttack1Flg = false;
	//パリィ攻撃２フラグ
	private bool parryAttack2Flg = false;
	//回避フラグ
	private bool avoidFlg = false;
	//技１フラグ
	private bool skill1Flg = false;
	//技２フラグ
	private bool skill2Flg = false;
	//必殺技フラグ
	private bool superSkillFlg = false;
	
	//体力
	private int hitPoint= 2;

	// Use this for initialization
	void Start () {
		lastKeyTimer = 0;     
		nowRawKey = 0;
		lastRawKey = 0;
		fgDash = false;
		//防御プレハブタグ設定
		diffencePrefab.gameObject.tag = "Player_Diffence";
	}
	
	// Update is called once per frame
	void Update () {

		lastKeyTimer += Time.deltaTime;
		//ジャンプ
		if (Input.GetButtonDown ("Jump")) {
			print ("jump");
			if(jmpFlg == true) {
				jmpFlg = false;
				Jump.JumpMove(rigidbody2D,10f);
				
			} else if (doubleJmpFlg == true) {
				doubleJmpFlg = false;
				Jump.JumpMove(rigidbody2D,10f);
			}
		}

		if (Input.GetButtonDown ("Diffence")) {
			if (diffenceFlg == true) { 
				
				diffenceObj = Instantiate(this.diffencePrefab, new Vector2(transform.position.x-2f, transform.position.y)
				                          , Quaternion.identity) as GameObject;
				diffenceFlg = false;
			}
		} else if(Input.GetButtonUp ("Diffence")) {
			Destroy(diffenceObj);
			diffenceFlg = true;
		}

		//左右ボタンの入力
		float h = Input.GetAxisRaw ("Horizontal");
		if (h != 0) {
			//左右ボタンの時
			if (h == lastRawKey) {
				//ダッシュON
				lastRawKey = 0;
				fgDash = true;
			}
			if (fgDash) {
				//ダッシュ移動
				Side_Move.SideMove(rigidbody2D,Const.PLAYER_DASH_SPEED * h);
			} else {
				//歩き移動
				nowRawKey = h;
				lastKeyTimer = 0;
				Side_Move.SideMove(rigidbody2D,Const.PLAYER_SIDE_SPEED * h);
			}
		} else {
			//左右入力の無い時
			lastRawKey = nowRawKey;
			Side_Move.SideMove(rigidbody2D,0);
			fgDash = false;
		}

		//ダッシュ用タイマーリセット
		if (lastKeyTimer > Const.DOUBLE_KEY_TIME) {
			//Action
			lastRawKey = 0;
			lastKeyTimer = 0;
		}
	}

	void OnCollisionEnter2D (Collision2D collider) {
		//接地判定
		if (collider.gameObject.tag == "Ground") {
			jmpFlg = true;
			doubleJmpFlg = true;
		}
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
	}
}
