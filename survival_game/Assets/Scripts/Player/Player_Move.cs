using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {

	//ジャンプフラグ trueならできる
	private bool jmpFlg = true;
	//二段ジャンプフラグ trueならできる
	private bool doubleJmpFlg = true;
	//防御フラグ
	private bool diffenceFlg = true;
	
	//防御プレハブ
	public GameObject diffencePrefab;
	//防御オブジェクト
	private GameObject diffenceObj;

	//移動ベクトル
	private Vector2 vctMove;
	//前回押した左右移動キー
	private float lastSideKey;
	float lastKeyTimer;     
	float nowRawKey;
	float lastRawKey;
	bool fgDash;
	
	// Use this for initialization
	void Start () {
		lastKeyTimer = 0;     
		nowRawKey = 0;
		lastRawKey = 0;
		fgDash = false;
		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath("MovePath"),"time",3,"easetype",iTween.EaseType.easeOutSine));
		//防御プレハブタグ設定
		diffencePrefab.gameObject.tag = "Player_Diffence";
	}
	
	// Update is called once per frame
	void Update () {
		
		lastKeyTimer += Time.deltaTime;
		//ジャンプ
		if (Input.GetButtonDown ("Jump")) {
			print ("jump");
			if(jmpFlg == true) {
				jmpFlg = false;
				Jump.JumpMove(rigidbody2D,10f);
				
			} else if (doubleJmpFlg == true) {
				doubleJmpFlg = false;
				Jump.JumpMove(rigidbody2D,10f);
			}
		}
		
		if (Input.GetButtonDown ("Diffence")) {
			if (diffenceFlg == true) { 
				
				diffenceObj = Instantiate(this.diffencePrefab, new Vector2(transform.position.x-2f, transform.position.y)
				                          , Quaternion.identity) as GameObject;
				diffenceFlg = false;
			}
		} else if(Input.GetButtonUp ("Diffence")) {
			Destroy(diffenceObj);
			diffenceFlg = true;
		}
		//左右ボタンの入力
		float h = Input.GetAxisRaw ("Horizontal");
		if (h != 0) {
			//左右ボタンの時
			if (h == lastRawKey) {
				//ダッシュON
				lastRawKey = 0;
				fgDash = true;
			}
			if (fgDash) {
				//ダッシュ移動
				vctMove.x = Const.PLAYER_DASH_SPEED * h;
			} else {
				//歩き移動
				nowRawKey = h;
				lastKeyTimer = 0;
				vctMove.x = Const.PLAYER_SIDE_SPEED * h;
			}
		} else {
			//左右入力の無い時
			lastRawKey = nowRawKey;
			vctMove.x = 0;
			fgDash = false;
		}

		//加速度のセット
		rigidbody2D.velocity = vctMove;

		//ダッシュ用タイマーリセット
		if (lastKeyTimer > Const.DOUBLE_KEY_TIME) {
			//Action
			lastRawKey = 0;
			lastKeyTimer = 0;
		}
	}

	void OnCollisionEnter2D (Collision2D collider) {
		if (collider.gameObject.tag == "Ground") {
			jmpFlg = true;
			doubleJmpFlg = true;
		}
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
	}
}
