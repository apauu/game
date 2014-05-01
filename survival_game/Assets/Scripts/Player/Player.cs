using UnityEngine;
using System.Collections;
using System;

public class Player : Character_Base {
	
	protected Animator animator;

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
		animator = GetComponent<Animator>();
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
		// 硬直中以外の動作
		if(!stiffFlg) {
			//ジャンプ中、回避中、防御中はできない
			if (this.jmpFlg && !this.avoidFlg && !this.defenseFlg) {
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
					StartCoroutine(this.Avoid(Player_Const.AVOID_TIME, Player_Const.AVOID_STIFF, side));
				}
			}

			//防御終了
			if(this.defenseFlg && Input.GetButtonUp ("Defense")) {
				base.DefenseEnd();
			}

			//攻撃
			//回避中はできない
			if (!this.avoidFlg) {
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
						Attack2(Player_Const.SKILL1_DESTROY, 0, Player_Const.SKILL1_STIFF);
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
							//通常攻撃２
							this.InitAttackFlg();
					
							//Attack.Attack2(rigidbody2D, rightDirectionFlg, destroyTime);
						} else {
							//通常攻撃１
							this.InitAttackFlg();
							attack1Flg = true;
							this.Attack1();
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
		}else if(!defenseFlg && Input.GetButtonUp ("Defense")) {
		}
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		
		// 硬直中以外の動作
		if(!stiffFlg) {
			//移動系の処理は防御中、パリィ中、攻撃中、回避中（これで全部か？）にできないようにする
			//とりあえず防御時は動けないように
			if (!defenseFlg && !this.avoidFlg) { 

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

						SideMove();

					} else {
						//左右ボタンを離した時
						if(Input.GetButtonUp ("Horizontal")) {
							lastRawKey = nowRawKey;
							lastKeyTimer = 0;
							walkFlg = false;
							dashFlg = false;
						}

						//地上、入力無しの場合、x,y軸停止
						//Side_Move.SideMove(rigidbody2D, 0, 0);
					}

					//ジャンプ
					if (Input.GetButtonDown ("Jump")) {
						print (this.avoidFlg.ToString());
						print ("Player Jump");
						if(jmpFlg == true) {
							jmpFlg = false;
							onGroundFlg = false;
							Jump.JumpMove(rigidbody2D, Player_Const.JUMP_SPEED);
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
						SideMove();
					}

					//2段ジャンプ
					if (Input.GetButtonDown ("Jump")) {
						if (doubleJmpFlg == true) {
							doubleJmpFlg = false;
							onGroundFlg = false;
							Jump.JumpMove(rigidbody2D,Player_Const.JUMP_SPEED);
							print ("Player Jump");
						}
					}
				}
			}
		} else {

			//硬直中の場合
			lastRawKey = 0;
			lastKeyTimer = 0;
			if (walkFlg || dashFlg) {
				walkFlg = false;
				dashFlg = false;
			}
			
			//空中以外はx,y軸停止
			if (onGroundFlg) {
				//Side_Move.SideMove(rigidbody2D, 0, 0);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		print ("----------------OnGround!--------------");
		base.OnCollisionEnter2D (collision);
	}

	void OnCollisionExit2D (Collision2D collision) {
		print ("----------------ExitGround!--------------");
		base.OnCollisionExit2D (collision);
	}

	void OnTriggerEnter2D (Collider2D collider) {
	}
	
	//攻撃１
	void Attack1 () {
		animator.SetBool("attack1Flg", true );
		base.Attack (attack1Prefab, Player_Const.ATTACK1_DESTROY, Player_Const.ATTACK1_BEFORE, Player_Const.ATTACK1_STIFF);
	}

	//攻撃２
	private void Attack2 (float destroyTime,float beforeActionTime,  float stiffTime) {
		for (int i = 0 ; i < 5; i++) {
			base.Attack (attack2Prefab, destroyTime, beforeActionTime, stiffTime);
		}
	}

	//横移動
	private void SideMove() {
		//歩きスピード
		float speed = Player_Const.SIDE_SPEED;
		if (dashFlg) {
			//ダッシュスピード
			speed = Player_Const.DASH_SPEED;
		}

		//歩き移動
		//transform使用に変更
		//Side_Move.SideMove(rigidbody2D,Player_Const.SIDE_SPEED * sideButton);
		Vector3 temp;
		temp.x = transform.position.x + speed * sideButton * 0.05f;
		temp.y = transform.position.y;
		temp.z = transform.position.z;
		transform.position = temp;

		if (sideButton > 0) {
			rightDirectionFlg = true;	//右向きフラグ
		} else if (sideButton < 0) {
			rightDirectionFlg = false;	//左向きフラグ
		}

		this.transform.localScale = new Vector3 ((rightDirectionFlg ? -1 : 1), 1, 1);
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

	/// <summary>
	/// プレイヤーがイベントを起こせるかどうかチェックする
	/// </summary>
	private bool CheckEventAwake() {
		return !(!onGroundFlg || attack1Flg || attack2Flg || attack3Flg || jumpAttack1Flg || jumpAttack2Flg ||
		         parryFlg || parryAttack1Flg || parryAttack2Flg || avoidFlg || skill1Flg || skill2Flg || superSkillFlg);
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetDamageFlgFalse() {
		animator.SetBool("damageFlg", false );
	}

	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetWalkFlgFalse() {
		animator.SetBool("walkFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetDashFlgFalse() {
		animator.SetBool("dashFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAirFlgFalse() {
		animator.SetBool("airFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetParryFlgFalse() {
		animator.SetBool("parryFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetDefenceFlgFalse() {
		animator.SetBool("defenceFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAvoidFlgFalse() {
		animator.SetBool("avoidFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetSkill1FlgFalse() {
		animator.SetBool("skill1Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetSuperSkillFlgFalse() {
		animator.SetBool("superSkillFlg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAttack1FlgFalse() {
		animator.SetBool("attack1Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAttack2FlgFalse() {
		animator.SetBool("attack2Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAttack3FlgFalse() {
		animator.SetBool("attack3Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAirAttack1FlgFalse() {
		animator.SetBool("airAttack1Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetAirAttack2FlgFalse() {
		animator.SetBool("airAttack2Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetParryAttack1FlgFalse() {
		animator.SetBool("parryAttack1Flg", false );
	}
	
	/// <summary>
	/// アニメーションコントローラー用メソッド
	/// </summary>
	private void SetParryAttack2FlgFalse() {
		animator.SetBool("parryAttack2Flg", false );
	}
}