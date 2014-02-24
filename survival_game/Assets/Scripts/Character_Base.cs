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
	
	/// <summary>
	/// 硬直中フラグ	true：硬直	false：別動作可能
	/// </summary>
	protected bool stiffFlg = false;
	// デフォルトの被弾時無敵時間
	protected float mutekiTime = 0f;

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
		SetTagAttaked ();
	}

	protected void OnCollisionEnter2D (Collision2D collision) {
		//接地判定
		SetFlgOnGround (collision);
	}

	protected void OnCollisionExit2D (Collision2D collision) {
		//接地判定
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			onGroundFlg = false;
		}
	}
	
	/// <summary>
	/// 接地判定
	/// </summary>
	/// <param name="collision">接触オブジェクト</param>
	protected void SetFlgOnGround(Collision2D collision) {
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			if (collision.contacts != null && collision.contacts.Length > 0) {
				Vector2 contactPoint = collision.contacts[0].point;
				float angle = Vector2.Angle(new Vector2(0,-1),contactPoint - 
				                            new Vector2(this.transform.position.x,this.transform.position.y));
				// 横（壁）に接触した場合は処理を終了
				if(Mathf.Abs(angle) >= 80f && Mathf.Abs(angle) < 100f){

					return;
				} 
			}
			
			//接地処理
			onGroundFlg = true;
			jmpFlg = true;
			doubleJmpFlg = true;
		}
	}
	
	/// <summary>
	/// 接触判定を取るオブジェクトタグの初期化
	/// </summary>
	protected void SetTagAttaked() {
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
	
	/// <summary>
	/// 被ダメージ判定
	/// </summary>
	/// <param name="collision">接触オブジェクト</param>
	/// <param name="damagePoint">ダメージ値</param>
	/// <param name="stiffTime">被弾硬直時間</param>
	public void OnAttaked(Collision2D collision, float damagePoint, float stiffTime) {
		if (collision.gameObject.tag == attackedTag) {
			OnDamage(damagePoint);
		}
	}

	/// <summary>
	/// 被ダメージ判定。硬直時間無し
	/// 攻撃スクリプトからメッセージ呼び出し
	/// </summary>
	/// <param name="damagePoint">ダメージ値</param>
	protected void OnDamage(float damagePoint) {
		if (!mutekiFlg) {
			print ("Get " + damagePoint + " Damage!!");
			hitPoint -= damagePoint;

			GameObject gui = GameObject.FindGameObjectWithTag ("GUIText");
			if (gui != null) gui.SendMessage("ShowMessage", damagePoint.ToString () + "!");

			//HPが0以下ならキャラクターを破壊
			if (hitPoint <= 0) {
				print ("You Dead");
				/* Destroyで削除すると他からオブジェクト参照している場合エラー発生するので活動停止で画面から消す　*/
				/* Update関数なども呼ばれなくなる　オブジェクト検索もできなくなる模様 */
				gameObject.SetActiveRecursively(false);
			} else {
				//無敵時間
				StartCoroutine(WaitForStiffTime (mutekiTime));
			}
		}
	}

	/// <summary>
	/// 被ダメージ判定。硬直時間有
	/// 攻撃スクリプトからメッセージ呼び出し
	/// </summary>
	/// <param name="damagePoint">ダメージ値</param>
	/// <param name="stiffTime">被弾硬直時間</param>
	protected void OnDamage(float damagePoint, float stiffTime) {
		if (!mutekiFlg) {
			print ("Get " + damagePoint + " Damage!!");
			hitPoint -= damagePoint;
			
			GameObject gui = GameObject.FindGameObjectWithTag ("GUIText");
			if (gui != null) gui.SendMessage("ShowMessage", damagePoint.ToString () + "!");
			
			//HPが0以下ならキャラクターを破壊
			if (hitPoint <= 0) {
				print ("You Dead");
				/* Destroyで削除すると他からオブジェクト参照している場合エラー発生するので活動停止で画面から消す　*/
				/* Update関数なども呼ばれなくなる　オブジェクト検索もできなくなる模様 */
				gameObject.SetActiveRecursively(false);
			} else {
				//硬直時間
				StartCoroutine(WaitForStiffTime (stiffTime));
				//無敵時間
				StartCoroutine(WaitForStiffTime (mutekiTime));
			}
		}
	}
	
	/// <summary>
	/// 防御
	/// </summary>
	/// <param name="stiffTime">前硬直時間</param>
	protected IEnumerator Defense (float stiffTime) {
		print ("Defense!!");
		
		//硬直時間
		StartCoroutine(WaitForStiffTime (stiffTime));

		//発動待機時間
		yield return new WaitForSeconds (stiffTime);

		float h = 0;

		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}

		//盾オブジェクト生成
		defenseObj = Instantiate(this.defensePrefab, new Vector2(transform.position.x + (1f * h), transform.position.y)
		                          , Quaternion.identity) as GameObject;

		//キャラクターのTagを判定して防御オブジェクトにTagをセット
		if (gameObject.tag.Equals(Tag_Const.PLAYER)) {
			defenseObj.tag = Tag_Const.PLAYER_DIFFENCE;
		} else {
			defenseObj.tag = Tag_Const.ENEMY_DIFFENCE;
		}

		//発動までにボタンを離していたら破壊
		if (defenseFlg) {
			Destroy(this.defenseObj);
		}
	}
	
	/// <summary>
	/// 回避
	/// </summary>
	/// <param name="avoidTime">無敵時間</param>
	/// <param name="stiffTime">硬直時間</param>
	protected IEnumerator Avoid (float avoidTime, float stiffTime) {
		print ("Avoid!!");
		avoidFlg = true;
		mutekiFlg = true;
		
		//硬直時間
		StartCoroutine(WaitForStiffTime (stiffTime));

		//無敵時間
		yield return new WaitForSeconds (avoidTime);

		avoidFlg = false;
		mutekiFlg = false;
	}
	
	/// <summary>
	/// パリィ
	/// </summary>
	/// <param name="destroyTime">判定の出ている時間</param>
	/// <param name="stiffTime">硬直時間</param>
	protected void Parry (float destroyTime, float stiffTime) {
		print ("Parry!!");
		parryFlg = true;

		float h = 0;
		
		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}
		//パリィオブジェクト生成
		parryObj = Instantiate(this.parryPrefab, new Vector2(transform.position.x + (1f * h), transform.position.y)
		                         , Quaternion.identity) as GameObject;

		//キャラクターのTagを判定してパリィオブジェクトにTagをセット
		if (gameObject.tag.Equals(Tag_Const.PLAYER)) {
			parryObj.tag = Tag_Const.PLAYER_PARRY;
		} else {
			parryObj.tag = Tag_Const.ENEMY_PARRY;
		}

		//消滅時間のセット
		parryObj.gameObject.SendMessage("setDestroyTime", destroyTime);
		
		//硬直時間
		StartCoroutine(WaitForStiffTime (stiffTime));
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	/// <param name="destroyTime">判定の出ている時間</param>
	/// <param name="prefab">攻撃Prefab</param>
	/// <param name="stiffTime">硬直時間</param>
	protected void Attack (float destroyTime, GameObject prefab, float stiffTime) {
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

		//キャラクターのTagを判定して攻撃オブジェクトにTagをセット
		//本当は遠距離攻撃かどうかの判定も必要！あとで実装！
		if (gameObject.tag.Equals(Tag_Const.PLAYER)) { 
			attackObj.tag = Tag_Const.PLAYER_ATTACK;
		} else {
			attackObj.tag = Tag_Const.ENEMY_ATTACK;
		}
		//攻撃方向のセット
		attackObj.gameObject.SendMessage("setDirection", rightDirectionFlg);
		//消滅時間のセット
		attackObj.gameObject.SendMessage("setDestroyTime", destroyTime);

		//硬直時間
		StartCoroutine(WaitForStiffTime (stiffTime));
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

	//硬直時間設定フラグ
	protected IEnumerator WaitForStiffTime (float time) {
		if (!stiffFlg) {
			stiffFlg = true;
			yield return new WaitForSeconds (time);
			stiffFlg = false;
		}
	}

	//無敵時間設定フラグ
	protected IEnumerator WaitForMutekiTime (float time) {
		if (!mutekiFlg) {
			mutekiFlg = true;
			yield return new WaitForSeconds (time);
			mutekiFlg = false;
		}
	}
}
