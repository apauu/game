using UnityEngine;
using System.Collections;

public class GUIText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ShowMessage(string message){
		this.guiText.text = message;

		//フェードアウト
		iTween.FadeTo (this.gameObject,iTween.Hash("alpha", 0, "time", 0.5f));

		StartCoroutine(SetWaitForSeconds ());
	}

	//メッセージを表示し、0.5秒後に削除
	private IEnumerator SetWaitForSeconds(){
		yield return new WaitForSeconds (0.5f);
		this.guiText.text = "";
	}
}
