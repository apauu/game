using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

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
	
	// Use this for initialization
	void Start () {
		diffencePrefab.gameObject.tag = "Player_Diffence";
	}
	
	// Update is called once per frame
	void Update () {
		//ジャンプ
		if (Input.GetButtonDown ("Jump")) {
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
