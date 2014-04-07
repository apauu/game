/// <summary>
/// 敵オブジェクトの基本となるクラス
/// 各敵クラスは本クラスを継承して利用すること
/// </summary>

using UnityEngine;
using System.Collections;

public class Enemy_Base : Character_Base {
	
	//停止フラグ true = 停止する
	protected bool stopFlg = false;
	//プレイヤー発見フラグ true = 発見
	protected bool noticeFlg = false;
	//プレイヤー位置フラグ true = 左側
	protected bool playerDirectionFlg = true;
	//itweenフラグ true = 動かしていい　動いてる間はfalse
	protected bool iTweenFlg = false;
	//プレイヤーオブジェクト
	protected GameObject player;
	
	//オブジェクト初期位置
	protected Vector3 firstPosition;
	//プレイヤーとの初期位置からのx距離
	protected float distanceX;
	//プレイヤーとの初期位置からのy距離
	protected float distanceY;
	//プレイヤーとの現在のx距離
	protected float nowDistanceX;
	//プレイヤーとの現在のy距離
	protected float nowDistanceY;

	/*------------------------キャラクター固有設定----------------------------*/
	/*--------------- Startメソッド内で必ず初期化を行うこと -------------------*/
	/*--------------- 倍率系は距離判定や移動処理にすべて適応される -------------------*/
	//プレイヤーに気づくx距離の倍率　キャラ固有　baseは１
	protected float noticeDistanceXMag;
	//プレイヤーに気づくy距離の倍率　キャラ固有　baseは１
	protected float noticeDistanceYMag;
	//キャラクターの移動速度倍率　キャラ固有　baseは１
	protected float enemySideSpeedMag;
	/*------------------------キャラクター固有設定----------------------------*/

	// Use this for initialization
	protected void Start () {
		//baseの初期化処理を実行
		base.Start ();
		//向きの初期化
		this.transform.localScale = new Vector3 ((this.rightDirectionFlg ? -1 : 1), 1, 1);
		//キャラクター固有設定を実行
		setCharacteristic ();

		//playerタグを検索してplayerオブジェクトを取得する
		this.player = GameObject.FindGameObjectWithTag (Tag_Const.PLAYER);
		this.firstPosition = this.transform.position;
		this.rightDirectionFlg = false;
		
	}

	// Update is called once per frame
	protected void Update () {
		Vector2 playerPosition = new Vector2(this.player.transform.position.x,this.player.transform.position.y); 
		//プレイヤーとのｘ距離を計算
		this.distanceX = Mathf.Abs (this.firstPosition.x-playerPosition.x);
		//プレイヤーとのｙ距離を計算
		this.distanceY = Mathf.Abs (this.firstPosition.y-playerPosition.y);

		//プレイヤーが初期位置から閾値以上離れている時
		if (this.distanceX > Enemy_Const.IMMOBILE_DISTANCE ||
		    this.distanceY > Enemy_Const.IMMOBILE_DISTANCE) {
			//停止する
			this.stopFlg = true;
			//気づいていない
			this.noticeFlg = false;
		} else {
			//停止しない
			this.stopFlg = false;
		}

		//停止しないとき　メイン処理
		if (!this.stopFlg) {

			//プレイヤーとの現在のx距離
			this.nowDistanceX = this.transform.position.x - playerPosition.x;
			//プレイヤーとの現在のy距離
			this.nowDistanceY = this.transform.position.y - playerPosition.y;
			//プレイヤーがどちら側にいるか判定
			//右にいるとき
			if ((this.nowDistanceX) < 0) { 
				this.playerDirectionFlg = false;
			//左にいるとき
			} else {
				this.playerDirectionFlg = true;
			}
			//気づいてないとき
			if(!this.noticeFlg) {

				//プレイヤーに気づくか判定
				//右向きの時
				if (this.rightDirectionFlg) {
					//プレイヤーが右にいる時
					if(this.playerDirectionFlg) {
						setNoticeFlg();
					}
				//左向きの時
				} else {
					//プレイヤーが左にいる時
					if(!this.playerDirectionFlg) {
						setNoticeFlg();
					}
				}
			}
		}
	}

	//物理演算利用系のUpdate処理はこちらへ
	protected void FixedUpdate () {
		//停止しないとき　メイン処理
		if (!this.stopFlg) { 
			//プレイヤーに気付いているとき
			if (this.noticeFlg) {
				if(!this.stiffFlg) {
					enemyAI();
				}
			}
		//停止させるとき　初期ポジションに戻す
		} else {
			float distance = this.transform.position.x - firstPosition.x;
			if(Mathf.Abs(distance) > 0.1) {
				this.transform.localScale = new Vector3(((distance <= 0) ? -1 : 1), 1, 1);
				//rightDirectionFlg更新
				this.rightDirectionFlg = ((distance <= 0) ? true : false);
				Side_Move.SideMove(rigidbody2D,Enemy_Const.ENEMY_SIDE_SPEED * ((distance <= 0) ? 1 : -1));
			}
		}
	}

	/// <summary>
	/// プレイヤーに気づくかどうか判定する
	/// </summary>
	protected void setNoticeFlg() {
		//プレイヤーとのX・Y距離が閾値以内だったら
		if(Mathf.Abs(this.nowDistanceX) < Enemy_Const.NOTICE_DISTANCE_X * this.noticeDistanceXMag &&
		   Mathf.Abs(this.nowDistanceY) < Enemy_Const.NOTICE_DISTANCE_Y * this.noticeDistanceYMag) {
			print ("EnemyNotice!!");
			this.noticeFlg = true;
		}
	}

	/// <summary>
	/// プレイヤーに気付いたあとの処理はここ
	/// </summary>
	protected virtual void enemyAI(){
	}

	/// <summary>
	/// オブジェクトに衝突した瞬間に呼ばれるコールバック関数
	/// </summary>
	/// <param name="collision">Collision.</param>
	protected void OnCollisionEnter2D (Collision2D collision) {
		//接地判定
		this.SetFlgOnGround (collision);
		//接地判定
		if (collision.gameObject.tag == Tag_Const.GROUND) {
			if (collision.contacts != null && collision.contacts.Length > 0) {
				Vector2 contactPoint = collision.contacts[0].point;
				float angle = Vector2.Angle(new Vector2(0,-1),contactPoint - 
					                            new Vector2(this.transform.position.x,this.transform.position.y));
				if(Mathf.Abs(angle) >= 80f && Mathf.Abs(angle) < 100f){
					print ("jump enemy");
					this.jmpFlg = false;
					Jump.JumpMove(rigidbody2D,10f);
				} else {
					this.jmpFlg = true;
				}
			}
		}
	}

	/// <summary>
	/// キャラクター固有のステータスを初期化する
	/// </summary>
	protected virtual void setCharacteristic() {
	}
}
