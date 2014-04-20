using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public GameObject[] itemList;
	public GameObject selectedIcon;
	public float incX = 3;
	public float incY = 3;

	// 現在選択中のボタン番号
	private int selected = 0;
	private float key;
	private bool selectableFlg = true;

	// Use this for initialization
	void Start () {
		SelectItem();
	}
	
	// Update is called once per frame
	void Update () {
		key = Input.GetAxisRaw ("Vertical");
		if (selectableFlg && key < 0) {
			UnSelectItem();
			PlusSelected(1);
			SelectItem();
			selectableFlg = false;

		} else if (selectableFlg && key > 0) {
			UnSelectItem();
			PlusSelected(-1);
			SelectItem();
			selectableFlg = false;

		} else if (selectableFlg && Input.GetButtonDown("Fire1")) {
			EnterItem();
			
		} else if (key == 0) {
			selectableFlg = true;

		}
	}

	// 選択中のボタン番号を変更する
	void PlusSelected (int cnt) {
		selected += cnt;
		if (selected > itemList.Length - 1) {
			selected = 0;
		} else if (selected < 0) {
			selected = itemList.Length - 1;
		}
	}

	// 選択中のボタンに選択エフェクトを表示する
	void SelectItem () {
		//Rect tmp = itemList[selected].guiTexture.pixelInset;
		//tmp.x -= incX;
		//tmp.y += incY;
		//itemList[selected].guiTexture.pixelInset = tmp;
		Vector3 tmp = itemList[selected].transform.position;
		Vector3 tmp2 = selectedIcon.transform.position;
		tmp2.y = tmp.y;
		selectedIcon.transform.position = tmp2;
	}

	// 選択中以外のボタンの選択エフェクトを終了する
	void UnSelectItem () {
		//Rect tmp = itemList[selected].guiTexture.pixelInset;
		//tmp.x += incX;
		//tmp.y -= incY;
		//itemList[selected].guiTexture.pixelInset = tmp;
	}

	// 決定ボタン押下時の処理を行う
	void EnterItem () {
		Rect tmp = itemList[selected].guiTexture.pixelInset;
		tmp.x += incX * 2;
		tmp.y -= incY * 2;
		itemList[selected].guiTexture.pixelInset = tmp;

		//　スタート
		if (selected == 0) {
			Application.LoadLevel("Menu");
			Debug.Log("Now scene is " + Application.loadedLevelName);

			//　コンティニュー
		} else if (selected == 1) {
			Application.LoadLevel("Menu");
			Debug.Log("Now scene is " + Application.loadedLevelName);

			//　終了
		} else if (selected == 2) {
			Debug.Log("Application is quit");
			Application.Quit ();
		}
	}
}
