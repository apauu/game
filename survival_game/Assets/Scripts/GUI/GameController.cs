using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public string deadTag = "";
	public string stageName = "";

	// Use this for initialization
	void Start () {
		//このオブジェクトはシーン間を受け継ぐ
		DontDestroyOnLoad(this);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//死亡メッセージを表示したい場合死亡タグを受け取る
	public void SetDeadTag(string deadTag)
	{
		this.deadTag = deadTag;
	}

	//死亡タグを返す
	public string GetDeadTag()
	{
		return deadTag;
	}
	
	//現在のステージ名を設定する
	public void SetStageName(string stageName)
	{
		this.stageName = stageName;
	}

	//現在のステージ名を返す
	public string GetStageName()
	{
		return stageName;
	}
}
