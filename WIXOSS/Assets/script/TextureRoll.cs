using UnityEngine;
using System.Collections;

public class TextureRoll : MonoBehaviour {

	public Texture[] textuerRoll; 
	private int position = 0; 

	// Use this for initialization
	void Start () {
		guiTexture.texture = textuerRoll [position];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void next () {
		nextPosition ();
		guiTexture.texture = textuerRoll [position];
	}
	
	void prev () {
		prevPosition ();
		guiTexture.texture = textuerRoll [position];
	}

	private void nextPosition() {
		position++;
		if (position >= textuerRoll.Length) {
			position = 0;
		}
	}
	
	private void prevPosition() {
		position--;
		if (position < 0) {
			position = textuerRoll.Length;
		}
	}
}
