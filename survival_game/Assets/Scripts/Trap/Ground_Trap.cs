/// <summary>
/// 床トラップ全般処理
/// </summary>

using UnityEngine;
using System.Collections;

public class Ground_Trap : MonoBehaviour {

	private const float DAMAGEPOINT = 50f;
	private bool trapFlg = false;

	// Use this for initialization
	void Start () {
		InvokeRepeating("setTrapFlg", 2, 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D (Collision2D collision) {
		if (trapFlg) {
			//衝突してきたオブジェクトがPlayer or Enemyの場合ダメージを与える
			if (collision.gameObject.tag.Equals(Tag_Const.PLAYER) || collision.gameObject.tag.Equals(Tag_Const.ENEMY)) {
				if (collision.contacts != null && collision.contacts.Length > 0) {
					//ダメージメソッド呼び出し
					collision.gameObject.SendMessage("onDamage", DAMAGEPOINT);
				}
			}
		}
	}

	/// <summary>
	/// trapFlgを反転させる.固定秒数ごとに呼び出される
	/// trapFlgに合わせてアニメーションも遷移させること　未実装！！
	/// </summary>
	void setTrapFlg () {
		trapFlg = (trapFlg ? false : true);
	}
}
