using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour {

	public int count = 0;

	// Use this for initialization
	void Start () {
		guiText.text = count.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void next () {
		count++;
		guiText.text = count.ToString();
	}

	void prev () {
		count--;
		guiText.text = count.ToString();
	}
}
