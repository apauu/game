/// <summary>
/// 回転床のトラップ.
/// 回転自体はiTweenで
/// それだけじゃキャラクターが吹っ飛ばないので
/// キャラクターのVelocityを変更させて
/// 強引にフットバス
/// </summary>

using UnityEngine;
using System.Collections;

public class Roll_Trap : MonoBehaviour {

	//回転床が回転を始めるまでの時間
	private const float DELAY_TIME = 0.5f;
	//回転床が回転する時間
	private const float ANIMATE_TIME = 0.5f;
	//回転床が消えたり出現したりする時間
	private const float REPEAT_TIME = 5.0f;
	//移動TweenのHashTable
	private Hashtable table;
	//回転床起動フラグ true = 回転できます
	private bool rollFlg = true;
	//回転床出現フラグ true = 出現中
	private bool appearFlg = true;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("repeat",DELAY_TIME,REPEAT_TIME);

		//移動TweenのHashTable
		table = new Hashtable();
		table.Add ("z", 180);
		table.Add ("delay", DELAY_TIME);
		table.Add ("time", ANIMATE_TIME);
		table.Add ("easeType", iTween.EaseType.easeInOutExpo);
		table.Add ("oncomplete", "EndHandler");		// トゥイーン開始時にEndHandler()を呼ぶ
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if(rollFlg) { 
			//衝突してきたオブジェクトがPlayer or Enemyの場合、床が回転する
			if (collision.gameObject.tag.Equals(Tag_Const.PLAYER) || collision.gameObject.tag.Equals(Tag_Const.ENEMY)) {
				if (collision.contacts != null && collision.contacts.Length > 0) {
					Vector2 contactPoint = collision.contacts[0].point;
					float angle = Vector2.Angle(new Vector2(0,-1),contactPoint - 
					                            new Vector2(collision.transform.position.x,collision.transform.position.y));
					//上から床を踏んだ場合
					if(Mathf.Abs(angle) < 20f){
						rollFloorTween();
					}
				}
			}
		}
	}

	void OnCollisionStay2D (Collision2D collision) {
		if (!rollFlg) {
			//collision.gameObject.rigidbody2D.AddForce(new Vector2(-10000f,5000f));
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		print ("exit");
	}

	void rollFloorTween(){
		Invoke ("setRollFlg",DELAY_TIME);
		iTween.RotateAdd(gameObject, table);
	}

	void repeat(){
		renderer.enabled = appearFlg;
		collider2D.isTrigger = !appearFlg;
		appearFlg = appearFlg ? false : true;
	}

	void setRollFlg() {
		rollFlg = false;
	}

	/// <summary>
	/// iTweenの開始時にコールバックされる
	/// </summary>
	private void EndHandler()
	{
		rollFlg = true;
	}
}
