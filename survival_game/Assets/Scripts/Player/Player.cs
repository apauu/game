using UnityEngine;
using System.Collections;

public class Player : Character_Base {

	//前回のキーを離してからの時間
	private float lastKeyTimer = 0;
	//現在押されている左右移動キー：1が右、-1が左
	private float sideButton = 0;
	//直前に押された左右移動キー
	private float nowRawKey = 0;
	//前回押した左右移動キー
	private float lastRawKey = 0;

	//攻撃２プレハブ
	public GameObject attack2Prefab;

	// Use this for initialization
	void Start () {
		//初期HP
		hitPoint = 2f;
		//防御プレハブタグ設定
		defensePrefab.gameObject.tag = Tag_Const.PLAYER_DIFFENCE;
		//パリィプレハブタグ設定
		parryPrefab.gameObject.tag = Tag_Const.PLAYER_PARRY;
		//攻撃プレハブタグ設定
		attack1Prefab.gameObject.tag = Tag_Const.PLAYER_ATTACK;
		//攻撃プレハブタグ設定
		attack2Prefab.gameObject.tag = Tag_Const.PLAYER_ATTACK;
	}
	
	// Update is called once per frame
	void Update () {

		//防御
		if (Input.GetButtonDown ("Defense")) {
			if (defenseFlg == true) {
				this.Defense();
				defenseFlg = false;
			}
		} else if(Input.GetButtonUp ("Defense")) {
			//防御終了
			Destroy(defenseObj);
			defenseFlg = true;
		}
		
		//回避
		if (Input.GetButtonDown ("Avoid")) {
			Avoid(1f);
		}

		//パリィ
		if (Input.GetButtonDown ("Parry")) {
			Parry(1f);
		}

		//攻撃
		//地上攻撃
		if (onGroundFlg) {
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
				Attack2(6f);
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
				if (parryAttack1Flg) {
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
					Attack1(5f);
				}
			}
		} else {
			//空中攻撃
			if (Input.GetButtonDown ("Fire1")) {
				print ("Player Air Fire1");
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
			}
		}
		
		//左右入力の無い時
		lastKeyTimer += Time.deltaTime;
		//ダッシュキー待機時間終了時
		if (lastKeyTimer > Player_Const.DOUBLE_KEY_TIME) {
			//最終横方向キー初期化
			lastRawKey = 0;
			//タイマー初期化
			lastKeyTimer = 0;
		}
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {

		//左右ボタンの入力
		sideButton = Input.GetAxisRaw ("Horizontal");
		if (sideButton != 0) {
			if (sideButton == lastRawKey
			    && onGroundFlg) {
				//ダッシュON
				lastRawKey = 0;
				walkFlg = false;
				dashFlg = true;
			}
			if (dashFlg) {
				//ダッシュ移動
				Side_Move.SideMove(rigidbody2D,Player_Const.DASH_SPEED * sideButton);
				if (sideButton > 0) {
					rightDirectionFlg = true;	//右向きフラグ
				} else if (sideButton < 0) {
					rightDirectionFlg = false;	//左向きフラ
				}
			} else {
				//歩き移動
				walkFlg = true;
				nowRawKey = sideButton;
				Side_Move.SideMove(rigidbody2D,Player_Const.SIDE_SPEED * sideButton);
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
				walkFlg = false;
				dashFlg = false;
			}

			//空中以外はx,y軸停止
			if (onGroundFlg) {
				Side_Move.SideMove(rigidbody2D, 0, 0);
			}
		}
		
		//ジャンプ
		if (Input.GetButtonDown ("Jump")) {
			print ("Player Jump");
			if(jmpFlg == true) {
				jmpFlg = false;
				onGroundFlg = false;
				Jump.JumpMove(rigidbody2D, Player_Const.JUMP_SPEED);
				
			} else if (doubleJmpFlg == true) {
				//2段ジャンプ
				doubleJmpFlg = false;
				onGroundFlg = false;
				Jump.JumpMove(rigidbody2D,Player_Const.JUMP_SPEED);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		print ("----------------OnGround!--------------");
		base.OnCollisionEnter2D (collision);

		if (collision.gameObject.tag == Tag_Const.ENEMY) {
			GameObject gui = GameObject.Find ("GUI Text");
			if (gui != null) gui.guiText.text = "Hit !";
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		print ("----------------ExitGround!--------------");
		base.OnCollisionExit2D (collision);
	}

	void OnTriggerEnter2D (Collider2D collider) {
	}
	
	//攻撃１
	void Attack1 (float destroyTime) {
		base.Attack (destroyTime, attack1Prefab);
	}

	//攻撃２
	void Attack2 (float destroyTime) {
		for (int i = 0 ; i < 5; i++) {
			base.Attack (destroyTime, attack2Prefab);
		}
	}
}
