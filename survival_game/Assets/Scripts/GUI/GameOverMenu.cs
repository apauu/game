using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour {

	private string message = "";
	private float f = 0f;
	private int maxLL = 36;

	// 現在のステージ名
	private string stageName = "";
	
	// Use this for initialization
	void Start () {
		//死亡区分を受け取り該当メッセージをセット
//		message = "「ここにメッセージを表示します。死亡区分ごとにメッセージが異なります。36文字を超える場合このようになります。\n改行する場合は”\\n”を設定して下さい。最大文字数は画面サイズを考慮して指定して下さい」";
		string deadTag = "";
		try {
			GameObject gobj = GameObject.Find("GameController");
			if (gobj != null) {
				GameController gcon = gobj.GetComponent<GameController>();
				deadTag = gcon.GetDeadTag();
				stageName = gcon.GetStageName();
				gcon.SetDeadTag("");
			}

		} finally {
			switch (deadTag)
			{
				case GUI_Const.ENEMY_SAITO:
					message = "自爆するしか能のない哀れな操り人形\nそれにやられる俺はさらに哀れってわけか";
					break;
				case GUI_Const.ENEMY_YODA:
					message = "殴っても殴っても殴るのをやめない\n俺に何の恨みがあるっていうんだ";
					break;
				case GUI_Const.ENEMY_LULU:
					message = "遠距離から撃ってくるだけなんて\nズルいじゃないですか・・・";
					break;
				case GUI_Const.ENEMY_YUKARI:
					message = "未だに過去の栄光にすがっているのか\nアバズレが・・・\n次こそはステージから引き摺り下ろす！";
					break;
				case GUI_Const.TRAP_ELECT:
					message = "何のために設置されたのだろうか\n焼き殺された死体の数からみてロクな理由じゃなさそうだ\nまだこいつらの仲間になるわけには・・・";
					break;
				case GUI_Const.TRAP_HOLL:
					message = "穴に落ちるなんて、迂闊だった・・・\n下ばかり見てきたってのに・・・";
					break;
				case GUI_Const.TRAP_GROUND:
					message = "衝突とともに訪れる死への誘い\nたまには飛び降り自殺も悪くない";
					break;
				default:
					break;
			}

		}

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

		else if (f >= message.Length) {
			//TODO ステージへ戻るボタン（メッセージ）を表示？
			if (Input.GetButtonDown("Fire1")) {
				if (stageName != null && !stageName.Equals("")) {
					Application.LoadLevel(stageName);
				} else {
					Application.LoadLevel(GUI_Const.MAINMENU);
				}
			}
			
		}

		int i = (int) f;
		if (i <= message.Length) {
			gameObject.guiText.text = message.Substring (0, i);
		}
	}
}
