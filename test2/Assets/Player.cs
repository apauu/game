using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private bool isRight = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//左右キーの入力
		float h = Input.GetAxis("Horizontal");
		rigidbody2D.AddForce(Vector2.right * h * 30f);
		
		// 炎オブジェクトを生成して方向フラグをsend
		//GameObject fire = Instantiate (firePrefab, new Vector3 (transform.position.x, transform.position.y - 0.5f, 1), Quaternion.identity) as GameObject;
		//fire.gameObject.SendMessage("setDirection", isRight);

	}
}
