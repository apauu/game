using UnityEngine;
using System.Collections;

/// <summary>
/// 1面：近距離攻撃MOB
/// </summary>
public class LuluSky : Enemy_Base {
	
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

	//物理演算利用系のUpdate処理はこちらへ
	protected void FixedUpdate () {
		//重力計算しないようになにもしない
	}

	/// <summary>
	/// プレイヤーに気付いたあとの処理はここ
	/// </summary>
	protected override void enemyAI ()
	{
		//動けるかどうか
		if(this.CheckEventAwake()) {
		//３択　無意味に近づく、無意味に遠ざかる、攻撃する
			//フラグが立ってる場合はこっち
			if(getAttackFlg) {
				this.GetAttack();
			}
			else { 
				//1~10のランダム値を作る
				System.Random random = new System.Random();
				float a = Mathf.Round(((float)(random.NextDouble() * 10)));
				//何もしない
				if(7 <= a) {
					//硬直時間
					StopCoroutine("WaitForStiffTime");
					StartCoroutine(WaitForStiffTime (0.5f));
				}
				//攻撃する
				else {
					getAttackFlg = true;
					this.GetAttack();
				}
			}
		}
	}
	
	protected virtual void GetAttack() {
		print ("LuluGetAttack!!");
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
		this.approachDistance = Enemy_Const.LULU_APPROACH_DISTANCE;
		this.attackDistance = Enemy_Const.LULU_ATTACK_DISTANCE;
		this.noticeDistanceXMag = Enemy_Const.LULU_NOTICE_DISTANCE_MAG;
		this.noticeDistanceYMag = Enemy_Const.LULU_NOTICE_DISTANCE_MAG;
		this.enemySideSpeedMag = Enemy_Const.LULU_SPEED_MAG;;
		this.hitPoint = Enemy_Const.LULU_HP;
	}
}
