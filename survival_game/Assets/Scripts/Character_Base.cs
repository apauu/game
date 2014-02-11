using UnityEngine;
using System.Collections;

public class Character_Base : MonoBehaviour {
	
	//現在向いている方向:右向きならtrue
	protected bool rightDirectionFlg = true;
	//体力
	protected int hitPoint;
	//接地状態フラグ
	protected bool onGroundFlg = false;
	//何もしていないフラグ
	protected bool neutralFlg = false;
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

	void OnCollisionEnter2D (Collision2D collider) {
		//接地判定
		if (collider.gameObject.tag == "Ground") {
			onGroundFlg = true;
			jmpFlg = true;
			doubleJmpFlg = true;
		}
	}
	
	void OnCollisionExit2D (Collision2D collider) {
		//接地判定
		if (collider.gameObject.tag == "Ground") {
			onGroundFlg = false;
		}
	}

	//防御
	protected void Defense () {
		float h = 0;

		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}
		//盾オブジェクト呼び出し
		//盾オブジェクト生成
		defenseObj = Instantiate(this.defensePrefab, new Vector2(transform.position.x + (2f * h), transform.position.y)
		                          , Quaternion.identity) as GameObject;
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
