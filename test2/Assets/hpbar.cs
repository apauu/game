using UnityEngine;
using System.Collections;

public class hpbar : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void onDamage(float damage)
	{
		print("on Damage");
		float width = transform.localScale.x - damage;
		if(width <= 0){
			width = 0;
		}
		transform.localScale = new Vector3(width,transform.localScale.y,transform.localScale.z);
	}
}