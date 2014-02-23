/// <summary>
/// 敵オブジェクトの基本となるクラス
/// 各敵クラスは本クラスを継承して利用すること
/// </summary>

using UnityEngine;
using System.Collections;

public class Enemy_Base : Character_Base {
	
	//停止フラグ true = 停止する
	private bool stopFlg = false;
	//プレイヤー発見フラグ true = 発見
	private bool noticeFlg = false;
	//プレイヤー位置フラグ true = 左側
	private bool playerDirectionFlg = true;
	//itweenフラグ true = 動かしていい　動いてる間はfalse
	private bool iTweenFlg = false;
	//AI起動フラグ true = 攻撃,防御,パリィなどを行う
	private bool startUpAIFlg = false;
	//プレイヤーオブジェクト
	private GameObject player;
	
	//オブジェクト初期位置
	private Vector3 firstPosition;
	//プレイヤーとのx距離
	private float distanceX;
	//プレイヤーとのy距離
	private float distanceY;

	/*------------------------キャラクター固有設定----------------------------*/
	/*--------------- Startメソッド内で必ず初期化を行うこと -------------------*/
	/*--------------- 倍率系は距離判定や移動処理にすべて適応される -------------------*/
	//プレイヤーに気づくx距離の倍率　キャラ固有　baseは１
	private float noticeDistanceXMag;
	//プレイヤーに気づくy距離の倍率　キャラ固有　baseは１
	private float noticeDistanceYMag;
	//キャラクターの移動速度倍率　キャラ固有　baseは１
	private float enemySideSpeedMag;
	/*------------------------キャラクター固有設定----------------------------*/

	// Use this for initialization
	void Start () {
		//baseの初期化処理を実行
		base.Start ();

		//キャラクター固有設定を実行
		setCharacteristic ();

		//playerタグを検索してplayerオブジェクトを取得する
		player = GameObject.FindGameObjectWithTag (Tag_Const.PLAYER);
		firstPosition = this.transform.position;
		rightDirectionFlg = false;

		//移動TweenのHashTable
		Hashtable table = new Hashtable();
		table.Add ("x", 5);
		table.Add ("loopType", iTween.LoopType.pingPong);
		table.Add ("delay", .5);
		table.Add ("speed", 2.0f);
		table.Add ("easeType", iTween.EaseType.easeInOutExpo);
		table.Add ("onstart", "StartHandler");		// トゥイーン開始時にStartHandler()を呼ぶ

		//うろうろ動く
		iTween.MoveBy(gameObject, table);
	}

	/// <summary>
	/// iTweenの開始時にコールバックされる
	/// </summary>
	private void StartHandler()
	{		
		//向きをセット
		transform.localScale = new Vector3((rightDirectionFlg ? 1 : -1), 1, 1);
		//rightDirectionFlg更新
		rightDirectionFlg = rightDirectionFlg ? false : true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 playerPosition = new Vector2(player.transform.position.x,player.transform.position.y); 
		//プレイヤーとのｘ距離を計算
		distanceX = Mathf.Abs (firstPosition.x-playerPosition.x);
		//プレイヤーとのｙ距離を計算
		distanceY = Mathf.Abs (firstPosition.y-playerPosition.y);

		//プレイヤーが初期位置から閾値以上離れている時
		if (distanceX > Enemy_Const.IMMOBILE_DISTANCE ||
		    distanceY > Enemy_Const.IMMOBILE_DISTANCE) {
			stopFlg = true;
			noticeFlg = false;
		} else {
			stopFlg = false;
		}

		//停止しないとき　メイン処理
		if (!stopFlg) {

			//プレイヤーとの現在のx距離
			float nowDistanceX = this.transform.position.x - playerPosition.x;
			//プレイヤーとの現在のy距離
			float nowDistanceY = this.transform.position.y - playerPosition.y;
			//プレイヤーがどちら側にいるか判定
			//右にいるとき
			if ((nowDistanceX) < 0) { 
				playerDirectionFlg = false;
			//左にいるとき
			} else {
				playerDirectionFlg = true;
			}
			//気づいてないとき
			if(!noticeFlg) {

				//iTweenFlgがtrueの時、ふらふら移動を再開させる
				if(iTweenFlg) {
					iTween.Resume(gameObject,"move");
					iTweenFlg = false;
				}

				//プレイヤーに気づくか判定
				//右向きの時
				if (rightDirectionFlg) {
					//プレイヤーが右にいる時
					if(playerDirectionFlg) {
						setNoticeFlg(nowDistanceX,nowDistanceY);
					}
				//左向きの時
				} else {
					//プレイヤーが左にいる時
					if(!playerDirectionFlg) {
						setNoticeFlg(nowDistanceX,nowDistanceY);
					}
				}
			//気づいてるとき
			} else {
				//iTweenFlgがtrueの時、ふらふら移動を停止させる
				if(!iTweenFlg) {
					iTween.Pause(gameObject,"move");
				}

				//プレイヤーとの距離が閾値以下になっていたら攻撃などのアクションを行う
				if(Mathf.Abs(nowDistanceX) < 5f) {
					startUpAIFlg = true;
				//攻撃できる位置にいない場合は近づく
				} else {
					startUpAIFlg = false;
				}
			}
		}
	}

	//物理演算利用系のUpdate処理はこちらへ
	private void FixedUpdate () {
		//停止しないとき　メイン処理
		if (!stopFlg) { 
			//プレイヤーに気付いているとき
			if (noticeFlg) {
				//プレイヤーがいる方向を向く
				transform.localScale = new Vector3 ((playerDirectionFlg ? 1 : -1), 1, 1);
				//rightDirectionFlg更新
				rightDirectionFlg = (playerDirectionFlg ? false : true);

				//AIが起動している場合
				if (startUpAIFlg) {
				enemyAI();
				//AIが起動していない場合
				} else {
						//プレイヤーに近づく
						//速度＝基本速度＊固有速度倍率＊プレイヤーの位置(1 or -1)
						Side_Move.SideMove (rigidbody2D,
	               				Enemy_Const.ENEMY_SIDE_SPEED *
								enemySideSpeedMag * 
								((playerDirectionFlg) ? -1 : 1));
				}
			}
		//停止させるとき　初期ポジションに戻す
		} else {
			float distance = this.transform.position.x - firstPosition.x;
			if(Mathf.Abs(distance) > 0.1) {
				transform.localScale = new Vector3(((distance <= 0) ? -1 : 1), 1, 1);
				//rightDirectionFlg更新
				rightDirectionFlg = ((distance <= 0) ? true : false);
				Side_Move.SideMove(rigidbody2D,Enemy_Const.ENEMY_SIDE_SPEED * ((distance <= 0) ? 1 : -1));
			} else {
				iTweenFlg = true;
			}
		}
	}

	/// <summary>
	/// プレイヤーに気づくかどうか判定する
	/// </summary>
	/// <param name="nowDistanceX">プレイヤーとの距離：x</param>
	/// <param name="nowDistanceY">プレイヤーとの距離：y</param>
	private void setNoticeFlg(float nowDistanceX,float nowDistanceY) {
		//プレイヤーとのX・Y距離が閾値以内だったら
		if(Mathf.Abs(nowDistanceX) < Enemy_Const.NOTICE_DISTANCE_X * noticeDistanceXMag &&
		   Mathf.Abs(nowDistanceY) < Enemy_Const.NOTICE_DISTANCE_Y * noticeDistanceYMag) {
			noticeFlg = true;
		}
	}

	/// <summary>
	/// AIメソッド　実装は継承先で？
	/// </summary>
	private void enemyAI(){
		if(!attack1Flg) { 
			//通常攻撃１
			this.InitAttackFlg();
			attack1Flg = true;
			base.Attack (2f, attack1Prefab);
			StartCoroutine (setWaitForSeconds(2.0f));
		}
	}

	/// <summary>
	/// オブジェクトに衝突した瞬間に呼ばれるコールバック関数
	/// </summary>
	/// <param name="collision">Collision.</param>
	private void OnCollisionEnter2D (Collision2D collision) {
		//接地判定
		setFlgOnGround (collision);
		//ダメージ判定
		onAttaked (collision);
		//接地判定
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			if (collision.contacts != null && collision.contacts.Length > 0) {
				Vector2 contactPoint = collision.contacts[0].point;
				float angle = Vector2.Angle(new Vector2(0,-1),contactPoint - 
					                            new Vector2(this.transform.position.x,this.transform.position.y));
				print (angle + " angle");
				if(Mathf.Abs(angle) >= 80f && Mathf.Abs(angle) < 100f){
					print ("jumpenemy");
					jmpFlg = false;
					Jump.JumpMove(rigidbody2D,10f);
				} else {
					jmpFlg = true;
				}
			}
		}
	}

	private IEnumerator setWaitForSeconds(float time)
	{
		yield return new WaitForSeconds(time);
		attack1Flg = false;
	}

	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
	private void setCharacteristic() {
		noticeDistanceXMag = 1f;
		noticeDistanceYMag = 1f;
		enemySideSpeedMag = 1f;
	}
}
