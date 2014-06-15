using UnityEngine;
using System.Collections;

public class Flow : MonoBehaviour {

	private float moveSpeed = 6f;
	private float rotationSpeed =6f;

	private Transform player;
	// Use this for initialization
	void Start () {
		//上下運動
		iTween.MoveBy(this.gameObject, iTween.Hash("y", -1f, "looptype",iTween.LoopType.pingPong,"time", 3f,"easetype", iTween.EaseType.easeInOutSine,"islocal", true));
	}

	void Update() {
		if(player != null){
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(player.position - transform.position), rotationSpeed*Time.deltaTime);
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
			if(Vector3.Distance(this.player.position,this.transform.position) <= 0.5f){
				Destroy(this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		//プレイヤーなら
		print ("itemget");

		if(collider.tag == Tag_Const.PLAYER) {
			//上下運動終了
			iTween.Stop();
			
			player = collider.transform;

		}
	}
}
