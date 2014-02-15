using UnityEngine;
using System.Collections;

public class Character_Base : MonoBehaviour {
	
	//現在向いている方向:右向きならtrue
	protected bool rightDirectionFlg = true;
	//体力
	protected float hitPoint = 1f;
	//接地状態フラグ
	protected bool onGroundFlg = false;
	//何もしていないフラグ
	protected bool neutralFlg = true;
	//歩き中フラグ
	protected bool walkFlg = false;
	//ダッシュ中フラグ
	protected bool dashFlg = false;
	//攻撃１フラグ
	protected bool attack1Flg = false;
	//攻撃２フラグ
	protected bool attack2Flg = false;
	//攻撃３フラグ
	protected bool attack3Flg = false;
	//ジャンプ中攻撃１フラグ
	protected bool jumpAttack1Flg = false;
	//ジャンプ中攻撃２フラグ
	protected bool jumpAttack2Flg = false;
	//パリィフラグ
	protected bool parryFlg = false;
	//パリィ攻撃１フラグ
	protected bool parryAttack1Flg = false;
	//パリィ攻撃２フラグ
	protected bool parryAttack2Flg = false;
	//回避フラグ
	protected bool avoidFlg = false;
	//技１フラグ
	protected bool skill1Flg = false;
	//技２フラグ
	protected bool skill2Flg = false;
	//必殺技フラグ
	protected bool superSkillFlg = false;
	//パリィ成功フラグ
	protected bool parrySuccessFlg = false;
	//無敵フラグ
	protected bool mutekiFlg = false;

	//ジャンプフラグ trueならできる
	protected bool jmpFlg = true;
	//二段ジャンプフラグ trueならできる
	protected bool doubleJmpFlg = true;
	//防御フラグ
	protected bool defenseFlg = true;
	//防御プレハブ
	public GameObject defensePrefab;
	//防御オブジェクト
	protected GameObject defenseObj;
	//パリィプレハブ
	public GameObject parryPrefab;
	//パリィオブジェクト
	protected GameObject parryObj;
	//攻撃１プレハブ
	public GameObject attack1Prefab;
	//攻撃オブジェクト
	protected GameObject attackObj;
	
	//被ダメージ攻撃タグ
	protected string attackedTag;
	//被ダメージ遠距離攻撃タグ
	protected string attackedLongTag;
	//被ダメージ防御破壊攻撃タグ
	protected string attackedBreakTag;

	protected void Start () {
		//接触判定を取るオブジェクトタグを初期化
		setTagAttaked ();
	}

	protected void OnCollisionEnter2D (Collision2D collision) {
		//接地判定
		setFlgOnGround (collision);
		//ダメージ判定
		onAttaked (collision);
	}

	protected void OnCollisionExit2D (Collision2D collision) {
		//接地判定
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			onGroundFlg = false;
		}
	}

	//接触判定を取るオブジェクトタグを初期化
	protected void setTagAttaked() {
		if (this.gameObject.tag == Tag_Const.PLAYER) {
			attackedTag = Tag_Const.ENEMY_ATTACK;
			attackedLongTag = Tag_Const.ENEMY_LONG_ATTACK;
			attackedBreakTag = Tag_Const.ENEMY_DIFFENCE_BREAK_ATTACK;
		} else if (this.gameObject.tag == Tag_Const.ENEMY) {
			attackedTag = Tag_Const.PLAYER_ATTACK;
			attackedLongTag = Tag_Const.PLAYER_LONG_ATTACK;
			attackedBreakTag = Tag_Const.PLAYER_DIFFENCE_BREAK_ATTACK;
		}
		print ("init attaked Tags");
		print ("attakedTag : " + attackedTag);
		print ("attakedLongTag : " + attackedLongTag);
		print ("attakedBreakTag : " + attackedBreakTag);
	}

	//接地判定
	protected void setFlgOnGround(Collision2D collision) {
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			onGroundFlg = true;
			jmpFlg = true;
			doubleJmpFlg = true;
		}
	}
	
	//ダメージ判定
	protected void onAttaked(Collision2D collision) {
		if (collision.gameObject.tag == attackedTag) {
			print ("Get Damage!");
			onDamage(1f);
		}
	}
	
	//被ダメージ処理
	protected void onDamage(float damagePoint) {
		print ("Get " + damagePoint + " Damage!!");
		hitPoint -= damagePoint;
		
		//HPが0以下ならキャラクターを破壊
		if (hitPoint <= 0) {
			print ("You Dead");
			Destroy (this.gameObject);
		}
	}

	//防御
	protected void Defense () {
		print ("Defense!!");
		float h = 0;

		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}
		//盾オブジェクト生成
		defenseObj = Instantiate(this.defensePrefab, new Vector2(transform.position.x + (2f * h), transform.position.y)
		                          , Quaternion.identity) as GameObject;
	}

	//回避
	protected IEnumerator Avoid (float time) {
		print ("Avoid!!");
		avoidFlg = true;
		mutekiFlg = true;
		yield return new WaitForSeconds(time);
		avoidFlg = false;
		mutekiFlg = false;
	}
	
	//パリィ
	protected void Parry (float destroyTime) {
		print ("Parry!!");
		parryFlg = true;

		float h = 0;
		
		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}
		//パリィオブジェクト生成
		parryObj = Instantiate(this.parryPrefab, new Vector2(transform.position.x + (2f * h), transform.position.y)
		                         , Quaternion.identity) as GameObject;
		
		//消滅時間のセット
		parryObj.gameObject.SendMessage("setDestroyTime", destroyTime);
	}

	//攻撃
	protected void Attack (float destroyTime, GameObject prefab) {
		print ("Attack!!");
		attack1Flg = true;
		
		float h = 0;
		
		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}
		//攻撃生成
		attackObj = Instantiate(prefab, new Vector2(transform.position.x , transform.position.y)
		                         , Quaternion.identity) as GameObject;

		//攻撃方向のセット
		attackObj.gameObject.SendMessage("setDirection", rightDirectionFlg);
		//消滅時間のセット
		attackObj.gameObject.SendMessage("setDestroyTime", destroyTime);
	}

	//攻撃関係のフラグを全て初期化する
	protected void InitAttackFlg () {
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
