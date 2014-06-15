using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour {

	private string message;
	private float f = 0f;
	private int maxLL = 36;
	
	// Use this for initialization
	void Start () {
		//TODO 死亡区分を受け取り該当メッセージをセット
		message = "「ここにメッセージを表示します。死亡区分ごとにメッセージが異なります。36文字を超える場合このようになります。\n改行する場合は”\\n”を設定して下さい。最大文字数は画面サイズを考慮して指定して下さい」";
		if (maxLL <= message.Length) {
			int i = 0;
			string checkStr = null;
			for (int j = 0 ; j < message.Length; j++) {
				checkStr = message.Substring(j, 1);
				print(checkStr);
				if (checkStr.Equals("\n")) {
					i = 0;
				} else {
					i++;
				}
				if (i > maxLL) {
					message = message.Insert(j , "\n");
					i = 0;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//　1/0.2フレームで一文字表示
		f += 0.2f;

		// 全て表示済みの状態でボタンを押すとシーン破棄
		if (f >= message.Length && Input.GetButtonDown ("Fire1")) {
			//TODO 親シーンの初期化を行う
			Destroy(this.gameObject);
		}

		// 攻撃ボタンが押された場合全部表示
		else if (Input.GetButtonDown ("Fire1")) {
			f = message.Length;
		}

		int i = (int) f;
		if (i <= message.Length) {
			gameObject.guiText.text = message.Substring (0, i);
		}
//		if (f >= message.Length) {
//			//TODO ステージへ戻るボタン（メッセージ）を表示？
//		}
	}
}
