﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 1面：近距離攻撃MOB
/// </summary>
public class Yoda : Enemy_Base {
	
	/*----------AI用フラグ---------*/
	protected bool getAttackFlg = false;
	protected bool getAwayFlg = false;
	protected bool getNearFlg = false;
	/*----------AI用フラグ---------*/
	protected float randomAwayDistance;
	protected float randomNearDistance;
	//スキル１プレハブ
	public GameObject skill1Prefab;
	//スキル１オブジェクト
	protected GameObject skill1Obj;
	protected Vector3 skillScale;
	
	// Use this for initialization
	void Start () {
		base.Start ();
		randomAwayDistance = Random.Range(3,4);
		randomNearDistance = Random.Range(1,2);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	/// <summary>
	/// プレイヤーに気付いたあとの処理はここ
	/// </summary>
	protected override void enemyAI ()
	{
		//動けるかどうか
		if(this.CheckEventAwake()) {
			//プレイヤーとの距離が遠すぎるときは近づく
			if (Mathf.Abs(this.nowDistanceX) > approachDistance) {
				this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
			}
			//ある程度近づいたら３択　無意味に近づく、無意味に遠ざかる、攻撃する
			else {
				//フラグが立ってる場合はこっち
				if(getAttackFlg) {
					this.GetAttack();
				}
				else if(getNearFlg) {
					this.GetNear();
				}
				else if(getAwayFlg) {
					this.GetAway();
				} 
				else { 
					//1~10のランダム値を作る
					System.Random random = new System.Random();
					float a = Mathf.Round(((float)(random.NextDouble() * 10)));
					//無意味に近づく
					if(a <= 1) {
						getNearFlg = true;
						this.GetNear();
					}
					//何もしない
					else if(7 <= a) {
						//硬直時間
						StopCoroutine("WaitForStiffTime");
						StartCoroutine(WaitForStiffTime (0.5f));
					}
					//爆発する
					else if(3 <= a) {
						getAttackFlg = true;
						this.GetAttack();
					}
					//無意味に遠ざかる
					else {
						getAwayFlg = true;
						this.GetAway();
					}
				}
			}
		}
	}
	
	protected void GetAway() {
		print ("YodaGetAway!!");
		if(Mathf.Abs(nowDistanceX) < this.randomAwayDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, this.playerDirectionFlg);
		}
		else {
			this.getAwayFlg = false;
		}
	}
	
	protected void GetNear() {
		print ("YodaGetNear!!");
		if(Mathf.Abs(nowDistanceX) > this.randomNearDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
		else {
			this.getNearFlg = false;
		}
	}
	
	protected virtual void GetAttack() {
		print ("YodaGetAttack!!");
		if(Mathf.Abs(nowDistanceX) < attackDistance) {
			this.getAttackFlg = false;
			base.Attack1();
		} 
		else {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
	}
	
	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
	protected override void setCharacteristic() {
		this.approachDistance = Enemy_Const.YODA_APPROACH_DISTANCE;
		this.attackDistance = Enemy_Const.YODA_ATTACK_DISTANCE;
		this.noticeDistanceXMag = Enemy_Const.YODA_NOTICE_DISTANCE_MAG;
		this.noticeDistanceYMag = Enemy_Const.YODA_NOTICE_DISTANCE_MAG;
		this.enemySideSpeedMag = Enemy_Const.YODA_SPEED_MAG;;
		this.hitPoint = Enemy_Const.YODA_HP;
	}
}
