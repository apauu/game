using UnityEngine;
using System.Collections;

public class CountUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown(){
		GameObject  parent =  gameObject.transform.parent.gameObject;
		parent.SendMessage ("next");
	}
}
