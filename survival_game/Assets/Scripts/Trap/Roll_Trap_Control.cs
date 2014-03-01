using UnityEngine;
using System.Collections;

public class Roll_Trap_Control : MonoBehaviour {

	//回転床プレハブ
	public GameObject rollTrapPrefab;
	//回転床オブジェクト
	private GameObject rollTrapobj;
	//回転床オブジェクト配置位置　もう少しいい管理方法がほしい
	private Vector3[] rollTrapPlace = {new Vector3(315,9,0),new Vector3(300,13,0),new Vector3(285,17,0),new Vector3(270,21,0)};
	//
	private const float WAIT_TIME = 2.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (setWaitForSeconds(WAIT_TIME));

	}

	private IEnumerator setWaitForSeconds(float time)
	{
		foreach(Vector3 place in rollTrapPlace){
			//回転床生成
			rollTrapobj = Instantiate (rollTrapPrefab, new Vector2 (place.x, place.y)
			                           , Quaternion.identity) as GameObject;
			//次の床を生成するのは少し待つ
			yield return new WaitForSeconds(time);
		}

		//役目を終えたら処理しないようにする
		gameObject.SetActiveRecursively(false);
	}
}
