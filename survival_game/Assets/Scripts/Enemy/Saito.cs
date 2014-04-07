using UnityEngine;
using System.Collections;

public class Saito : Enemy_Base {

	private bool getAttackFlg = false;
	private bool getAwayFlg = false;
	private bool getNearFlg = false;
	private float randomAwayDistance;
	private float randomNearDistance;

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

	protected override void enemyAI ()
	{
		//プレイヤーとの距離が遠すぎるときは近づく
		if (Mathf.Abs(this.nowDistanceX) > Enemy_Const.SAITO_APPROACH_DISTANCE) {
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
				//攻撃する
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

	private void GetAway() {
		print ("EnemyGetAway!!");
		if(Mathf.Abs(nowDistanceX) < this.randomAwayDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, this.playerDirectionFlg);
		}
		else {
			this.getAwayFlg = false;
		}
	}

	private void GetNear() {
		print ("EnemyGetNear!!");
		if(Mathf.Abs(nowDistanceX) > this.randomNearDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
		else {
			this.getNearFlg = false;
		}
	}

	private void GetAttack() {
		print ("EnemyGetAttack!!");
			if(Mathf.Abs(nowDistanceX) < Enemy_Const.SAITO_ATTACK_DISTANCE) {
			base.Attack (attack1Prefab, Enemy_Const.SAITO_ATTACK1_DESTROY, Enemy_Const.SAITO_ATTACK1_BEFORE, Enemy_Const.SAITO_ATTACK1_STIFF);
			this.getAttackFlg = false;
		} 
		else {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
	}


	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
	protected override void setCharacteristic() {
		this.noticeDistanceXMag = Enemy_Const.SAITO_NOTICE_DISTANCE_MAG;
		this.noticeDistanceYMag = Enemy_Const.SAITO_NOTICE_DISTANCE_MAG;
		this.enemySideSpeedMag = Enemy_Const.SAITO_SPEED_MAG;;
		this.hitPoint = Enemy_Const.SAITO_HP;
	}
}
