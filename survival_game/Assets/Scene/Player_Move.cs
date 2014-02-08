using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () {
		
		lastKeyTimer += Time.deltaTime;

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
}
