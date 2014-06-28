using UnityEngine;
using System.Collections;

public class SaitoSky : Enemy_Base {
	
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

		if(!getAttackFlg) {
			setNoticeFlg();
			if (!stiffFlg) {
				//停止しないとき　メイン処理
				if (!this.stopFlg) {
					//プレイヤーに気付いているとき
					if (this.noticeFlg) {
						enemyAI();
					}
				}
			}
		}
	}
	
	protected void FixedUpdate () {
		if(getAttackFlg) {
			base.FixedUpdate ();

			if(onGroundFlg) {
				skill1Exe();
			}
		}

		if(skill1Obj != null) {
			//爆発あたり判定を徐々に大きく
			skill1Obj.collider2D.transform.localScale = new Vector3(skillScale.x + 0.3f, skillScale.y + 0.3f, skillScale.z);
		}
		
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
			}
			//爆発する
			else if(!getAttackFlg) {
				getAttackFlg = true;
				this.GetAttack();
			}
		}
	}
	
	private void GetAttack() {
		print ("SaitoGetAttack!!");
		getAttackFlg = true;
		base.Skill1();
	}
	
	/// <summary>
	/// スキル１　爆発する
	/// </summary>
	protected virtual void skill1Exe ()
	{
		mutekiFlg = true;
		//爆発オブジェクト生成
		if(true) {
			this.skill1Obj = Instantiate(this.skill1Prefab, new Vector2(transform.position.x, transform.position.y)
			                             , Quaternion.identity) as GameObject;
			//あたり判定を取得
			skillScale = skill1Obj.collider2D.transform.localScale;
		}
		skill1End ();
	}
	
	protected void skill1End() {
		this.mutekiFlg = false;
		base.OnDamage (1000,0);
	}
	
	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
	protected override void setCharacteristic() {
		this.approachDistance = Enemy_Const.SAITO_APPROACH_DISTANCE;
		this.attackDistance = Enemy_Const.SAITO_ATTACK_DISTANCE;
		this.noticeDistanceXMag = Enemy_Const.SAITO_NOTICE_DISTANCE_MAG;
		this.noticeDistanceYMag = Enemy_Const.SAITO_NOTICE_DISTANCE_MAG;
		this.enemySideSpeedMag = Enemy_Const.SAITO_SPEED_MAG;;
		this.hitPoint = Enemy_Const.SAITO_HP;
	}
}
