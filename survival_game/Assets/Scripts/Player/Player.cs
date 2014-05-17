using UnityEngine;
using System.Collections;
using System;

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
		base.Start ();
		//初期HP
		hitPoint = Player_Const.HIT_POINT;
		//被弾時無敵時間
		mutekiTime = Player_Const.ATTACKED_MUTEKI;
		//防御プレハブタグ設定
		defensePrefab.gameObject.tag = Tag_Const.PLAYER_DIFFENCE;
		//パリィプレハブタグ設定
		parryPrefab.gameObject.tag = Tag_Const.PLAYER_PARRY;
		//攻撃プレハブタグ設定
		attack1Prefab.gameObject.tag = Tag_Const.PLAYER_ATTACK;
		//攻撃プレハブタグ設定
		attack2Prefab.gameObject.tag = Tag_Const.PLAYER_ATTACK;
	}
	
	/// <summary>
	/// 移動以外のアクション攻撃　パリィ　回避　防御
	/// </summary>
	void Update () {
		base.Update ();
		//防御
		if (Input.GetButtonDown ("Defense")) {
			base.Defense(Player_Const.DEFFENCE_BEFORE);
		}

		//パリィ
		if (Input.GetButtonDown ("Parry")) {
			base.Parry(Player_Const.PARRY_DESTROY, Player_Const.PARRY_STIFF);
		}

		//回避
		if (Input.GetButtonDown ("Avoid")) {
			float side = rightDirectionFlg ? 1 : -1;
			if(Input.GetButton("Horizontal")) {
				side = Input.GetAxisRaw ("Horizontal");
			}
			this.Avoid(Player_Const.AVOID_TIME, Player_Const.AVOID_STIFF, side);
		}

		//防御終了
		if(this.defenseFlg && Input.GetButtonUp ("Defense")) {
			base.DefenseEnd();
		}

		//攻撃
		//地上攻撃
		if (onGroundFlg) {
			if ((Input.GetButtonDown ("Fire4"))
					&&(skill1Flg
					|| skill2Flg)) {
				//必殺攻撃
				SuperSkill();

			} else if ((Input.GetButtonDown ("Fire2"))
					&&(neutralFlg
					|| attack1Flg
					|| attack2Flg
					|| attack3Flg
			   		|| skill2Flg)) {
				//技攻撃１
				Skill1();

			} else if ((Input.GetButtonDown ("Fire3"))
					&&(neutralFlg
					|| attack1Flg
					|| attack2Flg
					|| attack3Flg
					|| skill1Flg)) {
				//技攻撃２
				Skill1();

			} else if (Input.GetButtonDown ("Fire1")) {
				if (parryAttack1Flg) {
					//パリィ攻撃２
					ParryAttack2();

				} else if (parrySuccessFlg) {
					//パリィ攻撃１
					ParryAttack1();

				} else if (attack2Flg) {
					//通常攻撃３
					Attack3();

				} else if (attack1Flg) {
					//通常攻撃２
					Attack2();
			
				} else if (!attack3Flg && !parryAttack2Flg) {
					//通常攻撃１
					Attack1();
				}
			}
		} else {
			//空中攻撃
			if (Input.GetButtonDown ("Fire1")) {
				if (jumpAttack1Flg) {
					//ジャンプ攻撃２
					JumpAttack2();
				} else if (!jumpAttack2Flg) {
					//ジャンプ攻撃１
					JumpAttack1();
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
		base.FixedUpdate ();
		//地上動作
		if (onGroundFlg) {
			//左右ボタンの入力
			sideButton = Input.GetAxisRaw ("Horizontal");
			if (sideButton != 0) {
				if (sideButton == lastRawKey) {
					//ダッシュON
					lastRawKey = 0;
					walkFlg = false;
					dashFlg = true;
					print ("Dash!");
				}
				nowRawKey = sideButton;

				this.SideMove();

			} else {
				//左右ボタンを離した時
				if(Input.GetButtonUp ("Horizontal")) {
					lastRawKey = nowRawKey;
					lastKeyTimer = 0;
					walkFlg = false;
					dashFlg = false;
					animator.SetBool("walkFlg", false );
					animator.SetBool("dashFlg", false );
				}
			}

			//ジャンプ
			if (Input.GetButtonDown ("Jump")) {
				print ("Player Jump");
				if(jmpFlg == true) {
					jmpFlg = false;
					this.JumpMove(Player_Const.JUMP_SPEED);
				}
			}

		} else {
			//空中動作
			//硬直中の場合
			lastRawKey = 0;
			lastKeyTimer = 0;
			
			//左右ボタンの入力
			sideButton = Input.GetAxisRaw ("Horizontal");

			if (sideButton != 0) {
				this.SideMove();
			}

			//2段ジャンプ
			if (Input.GetButtonDown ("Jump")) {
				if (doubleJmpFlg == true) {
					doubleJmpFlg = false;
					this.JumpMove(Player_Const.JUMP_SPEED);
					print ("Player Jump");
				}
			}
		}
	}
	
	//攻撃１
	private void Attack1 () {
		print ("Player Attack1");
		InitAllFlg();
		attack1Flg = true;
		animator.SetBool("attack1Flg", true );
	}

	//攻撃２
	private void Attack2 () {
		print ("Player Attack2");
		InitAllFlg();
		attack2Flg = true;
		animator.SetBool("attack2Flg", true );
	}

	//攻撃３
	private void Attack3 () {
		print ("Player Attack3");
		InitAllFlg();
		attack3Flg = true;
		animator.SetBool("attack3Flg", true );
	}
	
	//パリィ攻撃１
	private void ParryAttack1 () {
		print ("Player ParryAttack1");
		InitAllFlg();
		parryAttack1Flg = true;
		animator.SetBool("parryAttack1Flg", true );
	}
	
	//パリィ攻撃２
	private void ParryAttack2 () {
		print ("Player ParryAttack2");
		InitAllFlg();
		parryAttack2Flg = true;
		animator.SetBool("parryAttack2Flg", true );
	}
	
	//空中攻撃１
	private void JumpAttack1 () {
		print ("Player JumpAttack1");
		InitAllFlg();
		jumpAttack1Flg = true;
		animator.SetBool("jumpAttack1Flg", true );
	}
	
	//空中攻撃２
	private void JumpAttack2 () {
		print ("Player JumpAttack2");
		InitAllFlg();
		jumpAttack2Flg = true;
		animator.SetBool("jumpAttack2Flg", true );
	}
	
	//スキル１
	private void Skill1 () {
		print ("Player Skill");
		InitAllFlg();
		skill1Flg = true;
		animator.SetBool("skill1Flg", true );
	}
	
	//スーパースキル
	private void SuperSkill () {
		print ("Player SuperSkill");
		InitAllFlg();
		superSkillFlg = true;
		animator.SetBool("superSkillFlg", true );
	}

	//横移動
	private void SideMove() {
		//歩きスピード
		float speed = Player_Const.SIDE_SPEED;
		if (dashFlg) {
			//ダッシュスピード
			speed = Player_Const.DASH_SPEED;
		}

		if (sideButton > 0) {
			rightDirectionFlg = true;	//右向きフラグ
		} else if (sideButton < 0) {
			rightDirectionFlg = false;	//左向きフラグ
		}

		this.SideMove(speed ,rightDirectionFlg);
	}

	/// <summary>
	/// エレベーターから呼び出される
	/// <param name="enterFlg">エレベーターに入るときtrue　出るときfalse</param>
	/// </summary>
	protected void EventElevator(bool enterFlg) {
		print ("elevator" + onGroundFlg);
		//イベント発生していいなら
		if(!enterFlg || CheckEventAwake()) {
			//移動TweenのHashTable
			Hashtable table = new Hashtable();
			table.Add ("z", 1 * (enterFlg? 1:-1));
			table.Add ("delay", .1);
			table.Add ("time", 2.0f);
			iTween.MoveBy(gameObject, table);

		}
	}
}
