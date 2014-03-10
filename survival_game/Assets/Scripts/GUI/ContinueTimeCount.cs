using UnityEngine;
using System;

public class ContinueTimeCount: MonoBehaviour {

	private float timeCount = 30;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float timer = timeCount - Time.time;

		gameObject.guiText.text = string.Format("{0:00}:{1:00}",Math.Floor(timer % 60f), timer % 1 * 100);
	}
}
