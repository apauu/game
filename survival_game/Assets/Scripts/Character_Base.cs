using UnityEngine;
using System.Collections;
using System;

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

	//ジャンプフラグ falseならできる
	protected bool jmpFlg = false;
	//二段ジャンプフラグ falseならできる
	protected bool doubleJmpFlg = false;
	//防御フラグ falseならできる
	protected bool defenseFlg = false;
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

	//球体判定
	protected CircleCollider2D CircleCollider = new CircleCollider2D();

	//重力計算用
	private float vi = 0,vf,t;

	private float jumpSpeed = 0;

	protected void Start () {
		//
		this.rigidbody2D.isKinematic = true;

		//球体判定を取得
		this.CircleCollider = this.gameObject.GetComponent<CircleCollider2D>();

		if(this.CircleCollider == null) {
			print ("Collider is null");
		}
		//接触判定を取るオブジェクトタグを初期化	
		SetTagAttaked ();
	}

	protected void Update(){
		SetFlgOnGround ();
	}

	protected void FixedUpdate() {

		if(!jumpSpeed.Equals(0)) 
		{
			vi = jumpSpeed;
			jumpSpeed = 0;
		}
		t = Time.deltaTime;
		vf= vi + Physics_Const.GRAVITY_ACCELERATION * t;            //最終速度＝初速度＋加速度×時間
		float deltaY = 0.5f*(vi+vf)*t;    //変位＝１／２（初速度＋最終速度）×時間
		//移動量が正の場合
		if(deltaY >= 0f) {
			//上にあるものの距離を取得
			float upDirection = RayCastDistance(Vector2.up);
			if(!float.IsNegativeInfinity(upDirection) && Mathf.Abs(deltaY) >= upDirection) {
				deltaY = upDirection;
				vi = 0;
			}
		}
		else {
			//下にあるものの距離を取得
			float downDirection = RayCastDistance(-Vector2.up);
			if(!float.IsNegativeInfinity(downDirection) && Mathf.Abs(deltaY) >= downDirection) {
				deltaY = -downDirection;
				vi = 0;
			}
		}
		vi = vf;
		this.transform.Translate (new Vector3(0,deltaY,0));
	}


	/// <summary>
	/// 上下左右にあるGroundオブジェクトとの距離を調べる
	/// </summary>
	/// <returns>The cast distance.</returns>
	/// <param name="direction">Direction.</param>
	protected float RayCastDistance(Vector2 direction) {
		float distance = float.NegativeInfinity;

		RaycastHit2D hit = new RaycastHit2D();
		LayerMask mask = -1 - 1 << gameObject.layer;
		//１M以内にあるものを検索
		hit = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y),direction,float.PositiveInfinity,mask);
		if(hit != null && hit.collider != null){
			if(hit.collider.gameObject.tag.Equals(Tag_Const.GROUND)){
				distance = hit.fraction;
			}
		}
		return distance - this.CircleCollider.radius;
	}
	
	/// <summary>
	/// 接地判定
	/// </summary>
	/// <param name="collision">接触オブジェクト</param>
	protected void SetFlgOnGround() {
		RaycastHit2D hit = new RaycastHit2D();
		LayerMask mask = -1 - 1 << gameObject.layer;

		if(this.CircleCollider != null) {
			hit = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y),-Vector2.up,this.CircleCollider.radius,mask);
			if(hit != null && hit.collider != null){
				if(hit.collider.gameObject.tag.Equals(Tag_Const.GROUND)){
					onGroundFlg = true;
					jmpFlg = true;
					doubleJmpFlg = true;
					vi = 0;
				}
				else
				{
					onGroundFlg = false;
				}
			}
			else
			{
				onGroundFlg = false;
			}
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
	protected void Defense (float stiffTime) {
		print ("Defense!!");
		defenseFlg = true;
		//硬直時間
		StartCoroutine(WaitForStiffTime (stiffTime));

		float h = rightDirectionFlg ? 1 : -1;

		if(this.defenseFlg) {
			//盾オブジェクト生成
			defenseObj = Instantiate(this.defensePrefab, new Vector2(transform.position.x + (1f * h), transform.position.y)
			                         , Quaternion.identity) as GameObject;
			//親を設定
			defenseObj.transform.parent = this.transform;
			//キャラクターのTagを判定して防御オブジェクトにTagをセット
			if (gameObject.tag.Equals(Tag_Const.PLAYER)) {
				defenseObj.tag = Tag_Const.PLAYER_DIFFENCE;
			} else {
				defenseObj.tag = Tag_Const.ENEMY_DIFFENCE;
			}
		}
	}

	/// <summary>
	/// 防御終了
	/// </summary>
	protected void DefenseEnd () {
		print ("DefenseEnd!!");
		defenseFlg = false;
		if(this.defenseObj != null) {
			Destroy(this.defenseObj);
		}
	}
	
	/// <summary>
	/// 回避
	/// </summary>
	/// <param name="avoidTime">無敵時間</param>
	/// <param name="stiffTime">硬直時間</param>
	protected IEnumerator Avoid (float avoidTime, float stiffTime, float side) {
		print ("Avoid!!");
		avoidFlg = true;
		mutekiFlg = true;
		//移動TweenのHashTable
		Hashtable table = new Hashtable();
		table.Add ("x", 15f * side);
		//無敵時間より少し短い時間で移動する
		table.Add ("time", avoidTime-0.03f);
		table.Add ("easetype", iTween.EaseType.easeInOutSine);
		iTween.MoveBy(gameObject, table);
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
		//親を設定
		parryObj.transform.parent = this.transform;
		//キャラクターのTagを判定してパリィオブジェクトにTagをセット
		if (gameObject.tag.Equals(Tag_Const.PLAYER)) {
			parryObj.tag = Tag_Const.PLAYER_PARRY;
		} else {
			parryObj.tag = Tag_Const.ENEMY_PARRY;
		}

		//消滅時間のセット
		parryObj.gameObject.SendMessage("SetDestroyTime", destroyTime);
		
		//硬直時間
		StartCoroutine(WaitForStiffTime (stiffTime));
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	/// <param name="prefab">攻撃Prefab</param>
	/// <param name="destroyTime">判定の出ている時間</param>
	/// <param name="beforeActionTime">行動前硬直</param>
	/// <param name="stiffTime">硬直時間</param>
	protected void Attack (GameObject prefab, float destroyTime, float beforeActionTime, float stiffTime) {
		print ("Attack!!");
		attack1Flg = true;
		float h = 0;
		
		if (rightDirectionFlg) {
			h = 1;
		} else {
			h = -1;
		}
		//硬直時間
		StopCoroutine("WaitForStiffTime");
		StartCoroutine(WaitForStiffTime (stiffTime));
		//攻撃生成
		attackObj = Instantiate(prefab, new Vector2(transform.position.x + 1 * h , transform.position.y)
		                         , Quaternion.identity) as GameObject;
		//親を設定
		attackObj.transform.parent = this.transform;
		//キャラクターのTagを判定して攻撃オブジェクトにTagをセット
		//本当は遠距離攻撃かどうかの判定も必要！あとで実装！
		if (gameObject.tag.Equals(Tag_Const.PLAYER)) { 
			attackObj.tag = Tag_Const.PLAYER_ATTACK;
		} else {
			attackObj.tag = Tag_Const.ENEMY_ATTACK;
		}

		//攻撃方向のセット
		attackObj.gameObject.SendMessage("SetDirection", rightDirectionFlg);
		//消滅時間のセット
		attackObj.gameObject.SendMessage("SetDestroyTime", destroyTime);
		print ("After!!");


	}

	/// <summary>
	/// パリィ成功処理
	/// </summary>
	protected void SuccessParry(){
		print ("SuccessParry!!");
		this.parrySuccessFlg = true;
		//パリィ可能時間
		
		DateTime now = DateTime.Now;
		this.parrySuccessFlg = false;
		print ((DateTime.Now - now).TotalMilliseconds.ToString());
	}

	/// <summary>
	/// 被パリィののけぞり処理
	/// </summary>
	/// <param name="winceTime">のけぞり時間</param>
	protected void Wince(float winceTime){
		print ("Wince!!");

	}

	/// <summary>
	/// 横移動
	/// </summary>
	/// <param name="speed">移動速度</param>
	/// <param name="direction">進む方向　右　＝　true</param>
	protected void SideMove(float speed,bool direction){

		float deltaX = speed * t *(direction ? 1 : -1);

		if(deltaX >= 0) {
			//右にあるものの距離を取得
			float rightDirection = RayCastDistance(Vector2.right);
			if(!float.IsNegativeInfinity(rightDirection) && Mathf.Abs(deltaX) >= rightDirection) {
				deltaX = rightDirection;
			}
		}
		else {
			//左にあるものの距離を取得
			float leftDirection = RayCastDistance(-Vector2.right);
			if(!float.IsNegativeInfinity(leftDirection) && Mathf.Abs(deltaX) >= leftDirection) {
				deltaX = leftDirection;
			}
		}
		this.rightDirectionFlg = direction;
		this.transform.Translate (new Vector3(deltaX,0,0));
		this.transform.localScale = new Vector3 ((rightDirectionFlg ? -1 : 1), 1, 1);
	}

	/// <summary>
	/// Jumps the move.
	/// </summary>
	/// <param name="speed">Speed.</param>
	protected void JumpMove(float speed) {
		print ("jump");
		//ジャンプ速度を設定
		jumpSpeed = speed;
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
		stiffFlg = true;
		yield return new WaitForSeconds (time);
		stiffFlg = false;
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
