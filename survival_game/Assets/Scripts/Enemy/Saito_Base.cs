using UnityEngine;
using System.Collections;

public class Saito_Base : Enemy_Base {

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

	protected void FixedUpdate () {
		base.FixedUpdate ();

		if(skill1Obj != null) {
			skill1Obj.collider2D.transform.localScale = new Vector3(skillScale.x + 0.3f, skillScale.y + 0.3f, skillScale.z);
		}

	}

	/// <summary>
	/// プレイヤーに気付いたあとの処理はここ
	/// </summary>
	protected override void enemyAI ()
	{
		//動けるかどうか
		if(!this.CheckEventAwake()) {
			//プレイヤーとの距離が遠すぎるときは近づく
			if (Mathf.Abs(this.nowDistanceX) > approachDistance) {
				MoveExe();
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

	protected virtual void MoveExe() {
		this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
	}

	protected void GetAway() {
		print ("EnemyGetAway!!");
		if(Mathf.Abs(nowDistanceX) < this.randomAwayDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, this.playerDirectionFlg);
		}
		else {
			this.getAwayFlg = false;
		}
	}

	protected void GetNear() {
		print ("EnemyGetNear!!");
		if(Mathf.Abs(nowDistanceX) > this.randomNearDistance) {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
		else {
			this.getNearFlg = false;
		}
	}

	protected virtual void GetAttack() {
		print ("EnemyGetAttack!!");
		if(Mathf.Abs(nowDistanceX) < attackDistance) {
			base.Skill1();
			this.getAttackFlg = false;
		} 
		else {
			this.SideMove(Enemy_Const.ENEMY_SIDE_SPEED * this.enemySideSpeedMag, !this.playerDirectionFlg);
		}
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

		//硬直時間
		StopCoroutine("WaitForStiffTime");
		StartCoroutine(WaitForStiffTime (2f));
		skill1End ();
		}
	}

	/// <summary>
	/// スキル1終了
	/// </summary>
	protected void skill1End() {
		skill1Obj.GetComponent<BoxCollider2D>().enabled = false;
	}

	protected void skill1FinalEnd() {
		if(this.skill1Obj == null) {
			Destroy(this.skill1Obj);
		}
		this.mutekiFlg = false;
		base.OnDamage (1000);
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
