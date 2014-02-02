using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour {
	
	public float spd = 0.01f;
	public bool is_right_local = true;
	
	// Update is called once per frame
	void Update() {
		//受け取った方向を元に炎を飛ばす方向を決定
		if(is_right_local){
			transform.Translate(Vector3.right * spd);
		}else{
			transform.Translate(Vector3.left * spd);
			//炎オブジェクトを反転させる
			transform.localScale = new Vector3(-2, 2, 0);
		}
		//transform.Translate(Vector3.right * spd);
		Destroy(gameObject, 5.0f);
	}

	//方向を受け取る
	void setDirection(bool isRight = true)
	{
		is_right_local = isRight;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		print ("Start OnTriggerEnter2D");
		if(col.gameObject.tag == "Enemy"){
			Destroy(col.gameObject);
		}
	}
}