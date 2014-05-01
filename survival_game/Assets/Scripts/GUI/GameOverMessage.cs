using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GameOverMessage : MonoBehaviour {

	// 表示するテキストファイルのタイトル
	public string messageTitle;
	// 次の１行を表示するまでの時間
	private float showLineTime = 1f;
	// １行の最大表示文字数
	private int maxLineLength = 30;

	private string[] lineList;
	private float timer;
	private int nowLine = 0;

	// Use this for initialization
	void Start () {
		TextAsset txt = Resources.Load(messageTitle) as TextAsset;
		Debug.Log(txt.text);

		StringReader sr;
		sr = new StringReader (txt.text);
		List<string> list = new List<string> (); 
		string line;
		
		while ((line = sr.ReadLine()) != null) list.Add (line);

		lineList = list.ToArray();
		timer = 0f;

		if (lineList.Length > 0) this.guiText.text = lineList [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) //このシーンを破棄。マップの最初に戻る

		if (timer > showLineTime) {
			if (nowLine < lineList.Length) ShowMessage();

			timer  = 0;

		} else {
			timer += Time.deltaTime;
		}
	}

	void ShowMessage () {
		string message = lineList[++nowLine];

		//　一行の最大文字数を超えた場合折り返し。
		if (message.Length > maxLineLength) {
			int count = message.Length / maxLineLength;
			for (int i = count ; i > 0; i--) {
				message.Insert(i * maxLineLength, "\r\n");
			}
		}

		this.guiText.text += message;
	}

	public void setMessageTitle(string str) {
		messageTitle = str;
	}
}
