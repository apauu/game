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
	private float delayTime = 0.5f;
	//移動TweenのHashTable
	private Hashtable table;
	//回転床起動フラグ true = 回転できます
	private bool rollFlg = true;

	// Use this for initialization
	void Start () {
		//移動TweenのHashTable
		table = new Hashtable();
		table.Add ("z", 1080);
		table.Add ("delay", .5);
		table.Add ("time", 2.0f);
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
					print (angle + " angle");
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
			collision.gameObject.transform.Translate(new Vector3(-10,5,0));
		}
	}

	void rollFloorTween(){
		rollFlg = false;
		iTween.RotateAdd(gameObject, table);
	}

	/// <summary>
	/// iTweenの開始時にコールバックされる
	/// </summary>
	private void EndHandler()
	{
		rollFlg = true;
	}
}
