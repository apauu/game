using UnityEngine;
using System.Collections;

public class Yukari : Enemy_Base {
	
	private bool getAttackFlg = false;
	private bool getAttackFlg2 = false;
	private bool getAttackFlg3 = false;
	private bool getAwayFlg = false;
	private bool getNearFlg = false;
	private bool getSummonFlg = false;
	private float randomAwayDistance;
	private float randomNearDistance;

	//雑魚プレハブ
	public GameObject zakoPrefab;
	//雑魚オブジェクト
	protected GameObject zakoObj;

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
		if (Mathf.Abs(this.nowDistanceX) > Enemy_Const.YUKARI_APPROACH_DISTANCE) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
		//ある程度近づいたら３択　無意味に近づく、無意味に遠ざかる、攻撃する
		else {
			//フラグが立ってる場合はこっち
			if(getAttackFlg) {
				this.GetAttack();
			}
			else if(getAttackFlg2) {
				this.GetAttack2();
			}
			else if(getAttackFlg3) {
				this.GetAttack3();
			}
			else if(getNearFlg) {
				this.GetNear();
			}
			else if(getAwayFlg) {
				this.GetAway();
			} 
			else if(getSummonFlg) {
				this.GetSummon();
			}
			else { 
				//1~10のランダム値を作る
				System.Random random = new System.Random();
				float a = Mathf.Round(((float)(random.NextDouble() * 10)));
				//無意味に遠ざかる
				if(a <= 1) {
					getAwayFlg = true;
					this.GetAway();
				}
				//攻撃する
				else if(3 <= a) {
					getAttackFlg = true;
					this.GetAttack();
				}
				//攻撃する2
				else if(5 <= a) {
					getAttackFlg2 = true;
					this.GetAttack();
				}
				//攻撃する3
				else if(7 <= a) {
					getAttackFlg3 = true;
					this.GetAttack();
				}
				//雑魚を召喚する
				else if(8 <= a) {
					getAttackFlg3 = true;
					this.GetSummon();
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
		print ("YukariGetAway!!");
		if(Mathf.Abs(nowDistanceX) < this.randomAwayDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, this.playerDirectionFlg);
		}
		else {
			this.getAwayFlg = false;
		}
	}
	
	private void GetNear() {
		print ("YukariGetNear!!");
		if(Mathf.Abs(nowDistanceX) > this.randomNearDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
		else {
			this.getNearFlg = false;
		}
	}
	
	private void GetAttack() {
		print ("YukariGetAttack!!");
		if(Mathf.Abs(nowDistanceX) < Enemy_Const.YUKARI_ATTACK_DISTANCE) {
			base.Attack (attack1Prefab, Enemy_Const.YUKARI_ATTACK1_DESTROY, Enemy_Const.YUKARI_ATTACK1_BEFORE, Enemy_Const.YUKARI_ATTACK1_STIFF);
			this.getAttackFlg = false;
		} 
		else {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
	}	
	private void GetAttack2() {
		print ("YukariGetAttack2!!");
		if(Mathf.Abs(nowDistanceX) < Enemy_Const.YUKARI_ATTACK_DISTANCE) {
			base.Attack (attack1Prefab, Enemy_Const.YUKARI_ATTACK1_DESTROY, Enemy_Const.YUKARI_ATTACK1_BEFORE, Enemy_Const.YUKARI_ATTACK1_STIFF);
			this.getAttackFlg2 = false;
		} 
		else {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
	}	
	private void GetAttack3() {
		print ("YukariGetAttack3!!");
		if(Mathf.Abs(nowDistanceX) < Enemy_Const.YUKARI_ATTACK_DISTANCE) {
			base.Attack (attack1Prefab, Enemy_Const.YUKARI_ATTACK1_DESTROY, Enemy_Const.YUKARI_ATTACK1_BEFORE, Enemy_Const.YUKARI_ATTACK1_STIFF);
			this.getAttackFlg3 = false;
		} 
		else {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
	}
	private void GetSummon() {
		print ("YukariGetSummon!!");
		//硬直時間
		StopCoroutine("WaitForStiffTime");
		StartCoroutine(WaitForStiffTime (Enemy_Const.YUKARI_SUMMON_STIFF));

		//召喚生成
		zakoObj = Instantiate(zakoPrefab, new Vector2(transform.position.x + 1 * (this.rightDirectionFlg ? -1 : 1) , transform.position.y)
		                        , Quaternion.identity) as GameObject;
		zakoObj.SendMessage("NoticePlayer");
		this.getSummonFlg = false;
	}
	
	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
	protected override void setCharacteristic() {
		this.noticeDistanceXMag = Enemy_Const.YUKARI_NOTICE_DISTANCE_MAG;
		this.noticeDistanceYMag = Enemy_Const.YUKARI_NOTICE_DISTANCE_MAG;
		this.enemySideSpeedMag = Enemy_Const.YUKARI_SPEED_MAG;;

		this.hitPoint = Enemy_Const.YUKARI_HP;
	}
}
