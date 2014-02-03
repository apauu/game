using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private bool isRight = true;
	private bool isAttack = false;

	public GameObject firePrefab;

	GameObject groundedOn = null;
	bool isGrounded = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//攻撃フラグをAnimatorに設定する
		//GetComponent<Animator>().SetBool("isAttack",isAttack);
	}

	void FixedUpdate ()
	{
		//  エンターキーが押されたら攻撃フラグを立てる
		if(Input.GetKeyDown("return")){	
			print ("getKey return");

			isAttack = true;
			StartCoroutine("WaitForAttack");

			// 炎オブジェクトを生成して方向フラグをsend
			GameObject fire = Instantiate (firePrefab, new Vector3 (transform.position.x+1, transform.position.y, 1), Quaternion.identity) as GameObject;
			fire.gameObject.SendMessage("setDirection", isRight);
		}


			//左右キーの入力
			float h = Input.GetAxis ("Horizontal");
			if (h * 30f > 10f) {
				print ("top speed Right");
				rigidbody2D.AddForce (Vector2.right * 30f);
			} else if (h * 30f < -5f) {
				print ("top speed Left");
				rigidbody2D.AddForce (Vector2.right * -30f);
			} else {
				rigidbody2D.AddForce (Vector2.right * h * 100f);
			}

		
		if (isGrounded) {
			if (Input.GetKeyDown ("up")) {	
				rigidbody2D.AddForce (Vector2.up * 800f);
			}
		}
		
		//右を向いていて、左の入力があったとき、もしくは左を向いていて、右の入力があったとき
		/*if((h > 0 && !isRight) || (h < 0 && isRight))
		{
			//右を向いているかどうかを、入力方向をみて決める
			isRight = (h > 0);
			//localScale.xを、右を向いているかどうかで更新する
			transform.localScale = new Vector3((isRight ? 2 : -2), 2, 2);
		}*/
	}

	IEnumerator WaitForAttack()
	{
		yield return new WaitForSeconds(0.5f);
		isAttack = false;
	}

	void OnCollisionEnter2D(Collision2D theCollision) {
		if (theCollision.gameObject.tag == "Ground") {
			isGrounded = true;
			groundedOn = theCollision.gameObject;
		}
	}
	
	void OnCollisionExit2D(Collision2D theCollision) {
		if (theCollision.gameObject == groundedOn) {
			groundedOn = null;
			isGrounded = false;
		}
	}
}
