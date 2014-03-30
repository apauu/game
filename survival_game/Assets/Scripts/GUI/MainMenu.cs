using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public GameObject[] itemList;
	public float incX = 3;
	public float incY = 3;
	
	private int selected = 0;
	private float key;
	private bool selectableFlg = true;

	public int stageNo = 1;
	
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
	
	void PlusSelected (int cnt) {
		selected += cnt;
		if (selected > itemList.Length - 1) {
			selected = 0;
		} else if (selected < 0) {
			selected = itemList.Length - 1;
		}
	}
	
	void SelectItem () {
		Rect tmp = itemList[selected].guiTexture.pixelInset;
		tmp.x -= incX;
		tmp.y += incY;
		itemList[selected].guiTexture.pixelInset = tmp;
	}
	
	void UnSelectItem () {
		Rect tmp = itemList[selected].guiTexture.pixelInset;
		tmp.x += incX;
		tmp.y -= incY;
		itemList[selected].guiTexture.pixelInset = tmp;
	}
	
	void EnterItem () {
		Rect tmp = itemList[selected].guiTexture.pixelInset;
		tmp.x += incX * 2;
		tmp.y -= incY * 2;
		itemList[selected].guiTexture.pixelInset = tmp;
		
		//　ネクストステージ
		if (selected == 0) {
			Application.LoadLevel("Stage" + GetStageNo());
			Debug.Log("Now scene is " + Application.loadedLevelName);
			
			//　ショップ
		} else if (selected == 1) {
			Application.LoadLevel("Shop");
			Debug.Log("Now scene is " + Application.loadedLevelName);
			
			//　タイトル
		} else if (selected == 2) {
			Application.LoadLevel("Title");
			Debug.Log("Now scene is " + Application.loadedLevelName);

		}
	}

	string GetStageNo () {
		if (stageNo.ToString ().Length == 1) {
			return "0" + stageNo.ToString ();
		}
		return stageNo.ToString ();

	}
}
