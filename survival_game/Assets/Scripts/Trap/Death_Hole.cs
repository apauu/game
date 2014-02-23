using UnityEngine;
using System.Collections;

public class Death_Hole : MonoBehaviour {

	private const float DAMAGEPOINT = 1000f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D (Collision2D collision) {
		//衝突してきたオブジェクトがPlayer or Enemyの場合ダメージを与える
		if (collision.gameObject.tag.Equals(Tag_Const.PLAYER) || collision.gameObject.tag.Equals(Tag_Const.ENEMY)) {
			if (collision.contacts != null && collision.contacts.Length > 0) {
				//ダメージメソッド呼び出し
				collision.gameObject.SendMessage("onDamage", DAMAGEPOINT);
			}
		}
	}
}
