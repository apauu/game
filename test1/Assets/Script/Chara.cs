using UnityEngine;
using System.Collections;

public class Chara : MonoBehaviour {

	//publicなグローバル変数にしておくとGUI上で調整できる
	public float walkForce = 30f;
	public float flyForce = 50f;
	public float maxWalkSpeed = 3f;
	public float maxFlySpeed = 5f;

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
		float v = Input.GetAxis("Vertical");
		
		//右を向いていて、左の入力があったとき、もしくは左を向いていて、右の入力があったとき
		if((h > 0 && !facingRight) || (h < 0 && facingRight))
		{
			//右を向いているかどうかを、入力方向をみて決める
			facingRight = (h > 0);
			//localScale.xを、右を向いているかどうかで更新する
			transform.localScale = new Vector3((facingRight ? 1 : -1), 1, 1);
		}

		//制限速度以下だったら、という条件を追加
		if(rigidbody2D.velocity.x < maxWalkSpeed)
		{
			rigidbody2D.AddForce(Vector2.right * h * walkForce);
		}
		
		//制限速度以下だったら、という条件を追加
		if(v > 0 && rigidbody2D.velocity.y < maxFlySpeed)
		{
			rigidbody2D.AddForce(Vector2.up * v * flyForce);
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
}
