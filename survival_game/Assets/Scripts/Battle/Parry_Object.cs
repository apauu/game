using UnityEngine;
using System.Collections;

public class Parry_Object : MonoBehaviour {

	//パリィ時間
	public float destroyTime = 1f;
	//成功時ひるませ時間
	public float winceTime = 0.5f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject, destroyTime);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (this.gameObject.tag == Tag_Const.PLAYER_PARRY) {
			if (collider.gameObject.tag == Tag_Const.ENEMY_ATTACK) {
				print ("Parry!!");
				Destroy(collider.gameObject);

				//パリィ成功メソッド呼び出し
				this.gameObject.transform.parent.SendMessage("SuccessParry");
				//ひるみメソッド呼び出し
				GameObject parent = collider.gameObject.transform.parent.gameObject;
				parent.SendMessage("Wince", winceTime);
			} else {
				print ("failed parry");
			}
		} else {
			if (collider.gameObject.tag == Tag_Const.PLAYER_ATTACK) {
				print ("Parry!!");
				Destroy(collider.gameObject);
				
				//ひるみメソッド呼び出し
				GameObject parent = collider.gameObject.transform.parent.gameObject;
				parent.SendMessage("Wince", winceTime);
			} else {
				print ("failed parry");
			}
		}
	}
	
	/// <summary>
	/// パラメータ初期化
	/// </summary>
	/// <param name="destroyTime">DestroyTime.</param>
	/// <param name="winceTime">WinceTime.</param>
	void Init(float destroyTime, float winceTime) {
		SetDestroyTime(destroyTime);
		SetWinceTime (winceTime);
	}
	
	//消滅までの時間をセット
	void SetDestroyTime(float time)
	{
		destroyTime = time;
	}

	//成功時に敵をひるませる時間をセット
	void SetWinceTime(float time)
	{
		winceTime = time;
	}
}
