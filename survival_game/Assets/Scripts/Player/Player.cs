using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//現在向いている方向:右向きならtrue
	private bool rightDirectionFlg = true;

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
	//現在押されている左右移動キー：1が右、-1が左
	private float sideButton = 0;
	//直前に押された左右移動キー
	private float nowRawKey = 0;
	//前回押した左右移動キー
	private float lastRawKey = 0;
	//歩き中フラグ
	private bool walkFlg = false;
	//ダッシュ中フラグ
	private bool dashFlg = false;
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

	//立ちモーションフラグ
	private bool neutralFlg = false;
	//接地状態フラグ
	private bool onGroundFlg = false;
	//パリィ成功フラグ
	private bool parrySuccessFlg = false;
	
	//体力
	private int hitPoint= 2;

	// Use this for initialization
	void Start () {
		//防御プレハブタグ設定
		diffencePrefab.gameObject.tag = "Player_Diffence";
	}
	
	// Update is called once per frame
	void Update () {
		
		//防御
		if (Input.GetButtonDown ("Diffence")) {
			print ("Player Diffence");
			if (diffenceFlg == true) { 
				//盾オブジェクト生成
				diffenceObj = Instantiate(this.diffencePrefab, new Vector2(transform.position.x-2f, transform.position.y)
				                          , Quaternion.identity) as GameObject;
				diffenceFlg = false;
			}
		} else if(Input.GetButtonUp ("Diffence")) {
			//防御終了
			Destroy(diffenceObj);
			diffenceFlg = true;
		}
		
		//回避
		if (Input.GetButtonDown ("Avoid")) {
			print ("Player Avoid");
			avoidFlg = true;
			//Avoid.Avoid(rigidbody2D, rightDirectionFlg);
		}

		//パリィ
		if (Input.GetButtonDown ("Parry")) {
			print ("Player Parry");
			parryFlg = true;
			//Parry.Parry(rigidbody2D, rightDirectionFlg);
		}

		//攻撃
		if ((Input.GetButtonDown ("Fire4"))
				&&(skill1Flg
				|| skill2Flg)) {
			//必殺攻撃
			print ("Player Fire4");
			this.InitAttackFlg();
			superSkillFlg = true;
			//Attack.SuperSkill(rigidbody2D, rightDirectionFlg, destroyTime);
		} else if ((Input.GetButtonDown ("Fire2"))
				&&(neutralFlg
				|| attack1Flg
				|| attack2Flg
				|| attack3Flg
				|| skill2Flg)) {
			//技攻撃１
			print ("Player Fire3");
			this.InitAttackFlg();
			skill1Flg = true;
			//Attack.Skill1(rigidbody2D, rightDirectionFlg, destroyTime);
		} else if ((Input.GetButtonDown ("Fire3"))
				&&(neutralFlg
				|| attack1Flg
				|| attack2Flg
				|| attack3Flg
				|| skill1Flg)) {
			//技攻撃２
			print ("Player Fire2");
			this.InitAttackFlg();
			skill2Flg = true;
			//Attack.Skill2(rigidbody2D, rightDirectionFlg, destroyTime);
		} else if (Input.GetButtonDown ("Fire1")) {
			print ("Player Fire1");
			if (!onGroundFlg) {
				if (jumpAttack1Flg) {
					//ジャンプ攻撃２
					this.InitAttackFlg();
					jumpAttack2Flg = true;
					//Attack.JumpAttack2(rigidbody2D, rightDirectionFlg, destroyTime);
				} else {
					//ジャンプ攻撃１
					this.InitAttackFlg();
					jumpAttack1Flg = true;
					//Attack.JumpAttack1(rigidbody2D, rightDirectionFlg, destroyTime);
				}
			} else if (parryAttack1Flg) {
				//パリィ攻撃２
				this.InitAttackFlg();
				parryAttack2Flg = true;
				//Attack.ParryAttack2(rigidbody2D, rightDirectionFlg, destroyTime);
			} else if (parrySuccessFlg) {
				//パリィ攻撃１
				this.InitAttackFlg();
				parryAttack1Flg = true;
				//Attack.ParryAttack1(rigidbody2D, rightDirectionFlg, destroyTime);
			} else if (attack2Flg) {
				//通常攻撃３
				this.InitAttackFlg();
				attack3Flg = true;
				//Attack.Attack3(rigidbody2D, rightDirectionFlg, destroyTime);
			} else if (attack1Flg) {
				//通常攻撃３
				this.InitAttackFlg();
				attack2Flg = true;
				//Attack.Attack2(rigidbody2D, rightDirectionFlg, destroyTime);
			} else {
				//通常攻撃１
				this.InitAttackFlg();
				attack1Flg = true;
				//Attack.Attack1(rigidbody2D, rightDirectionFlg, destroyTime);
			}
		}

		//ジャンプ
		if (Input.GetButtonDown ("Jump")) {
			print ("Player Jump");
			if(jmpFlg == true) {
				jmpFlg = false;
				Jump.JumpMove(rigidbody2D,10f);
				
			} else if (doubleJmpFlg == true) {
				//2段ジャンプ
				doubleJmpFlg = false;
				Jump.JumpMove(rigidbody2D,10f);
			}
		}

		//左右ボタンの入力
		sideButton = Input.GetAxisRaw ("Horizontal");
		if (sideButton != 0) {
			//
			if (sideButton == lastRawKey) {
				//ダッシュON
				lastRawKey = 0;
				walkFlg = false;
				dashFlg = true;
			}
			if (dashFlg) {
				//ダッシュ移動
				Side_Move.SideMove(rigidbody2D,Player_Const.PLAYER_DASH_SPEED * sideButton);
				if (sideButton > 0) {
					rightDirectionFlg = true;	//右向きフラグ
				} else if (sideButton < 0) {
					rightDirectionFlg = false;	//左向きフラ
				}
			} else {
				//歩き移動
				walkFlg = true;
				nowRawKey = sideButton;
				Side_Move.SideMove(rigidbody2D,Player_Const.PLAYER_SIDE_SPEED * sideButton);
				if (sideButton > 0) {
					rightDirectionFlg = true;	//右向きフラグ
				} else if (sideButton < 0) {
					rightDirectionFlg = false;	//左向きフラ
				}
			}
		} else {
			//左右ボタンを離した時
			if(Input.GetButtonUp ("Horizontal")) {
				lastRawKey = nowRawKey;
				lastKeyTimer = 0;
			}
			//左右入力の無い時
			lastKeyTimer += Time.deltaTime;
			walkFlg = false;
			dashFlg = false;
			Side_Move.SideMove(rigidbody2D, 0);
		}

		//ダッシュキー待機時間終了時
		if (lastKeyTimer > Player_Const.DOUBLE_KEY_TIME) {
			//最終横方向キー初期化
			lastRawKey = 0;
			//タイマー初期化
			lastKeyTimer = 0;
		}
	}

	void OnCollisionEnter2D (Collision2D collider) {
		//接地判定
		if (collider.gameObject.tag == "Ground") {
			onGroundFlg = true;
			jmpFlg = true;
			doubleJmpFlg = true;
		}
	}

	void OnCollisionExit2D (Collision2D collider) {
		//接地判定
		if (collider.gameObject.tag == "Ground") {
			onGroundFlg = false;
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
	}

	//攻撃関係のフラグを全て初期化する
	void InitAttackFlg () {
		attack1Flg = false;
		attack2Flg = false;
		attack3Flg = false;
		skill1Flg = false;
		skill2Flg = false;
		superSkillFlg = false;
		jumpAttack1Flg = false;
		jumpAttack2Flg = false;
	}
}
