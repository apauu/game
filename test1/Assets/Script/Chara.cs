using UnityEngine;
using System.Collections;

public class Chara : MonoBehaviour {

	//publicなグローバル変数にしておくとGUI上で調整できる
	public float walkForce = 30f;
	public float flyForce = 300f;
	public float maxWalkSpeed = 3f;
	private bool jump_flg = false;
	private bool double_jump_flg = false;

	//右を向いているかどうか（初期値はtrue）
	private bool facingRight = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		
		//右を向いていて、左の入力があったとき、もしくは左を向いていて、右の入力があったとき
		if((h > 0 && !facingRight) || (h < 0 && facingRight)){
			//右を向いているかどうかを、入力方向をみて決める
			facingRight = (h > 0);
			//localScale.xを、右を向いているかどうかで更新する
			transform.localScale = new Vector3((facingRight ? 1 : -1), 1, 1);
		}

		//制限速度以下だったら、という条件を追加
		if(rigidbody2D.velocity.x < maxWalkSpeed){
			rigidbody2D.AddForce(Vector2.right * h * walkForce);
		}

		//ジャンプ
		if (Input.GetButtonDown ("Jump") || Input.GetButtonDown("Vertical")) {
			Debug.Log(jump_flg);
			Debug.Log (double_jump_flg);
			//ジャンプ1回目
			if(jump_flg){
				jump_flg = false;
				rigidbody2D.AddForce(Vector2.up * flyForce);
			//ジャンプ2回目
			} else if(double_jump_flg) {
				double_jump_flg = false;
				rigidbody2D.AddForce(Vector2.up * flyForce);
			}
		}
		//Debug.Log (Mathf.Abs (rigidbody2D.velocity.y));
		if (Mathf.Abs(rigidbody2D.velocity.y) <= 0f) {
			jump_flg = true;
			double_jump_flg = true;
		}

		//制限速度より大きかったら
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxWalkSpeed)
		{
			//自分の速度を制限速度に合わせる
			//飛ぶ動きは、重力がかかって勝手に減速するので、そのまま
			//Mathf.Sign -> 値の符号を取得
			rigidbody2D.velocity = new Vector2(
				Mathf.Sign(rigidbody2D.velocity.x) * maxWalkSpeed,
				rigidbody2D.velocity.y
				);
		}

		GetComponent<Animator>().SetBool("left",facingRight);
	}

	void OnCollisionEnter2D (Collision2D collider) {
		if (collider.gameObject.tag == "enemy") {
			GameObject.DestroyObject(collider.gameObject);
			Debug.Log("destoy!!");
	}
		// 削除
		// GameObject.DestroyObject(collider.gameObject);
	}
}
