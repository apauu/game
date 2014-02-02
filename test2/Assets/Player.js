#pragma strict

var spd : float = 0.1;
var jumpSpeed : float = 8.0;
var gravity : float = 20.0;


function Start () {
}

function Update () {

	if (Input.GetKey("right")) {
		transform.position.x += spd;
	}
	else if (Input.GetKey("left")) {
		transform.position.x -= spd;
	}
	
	
	if (Input.GetKey("up")) {
		transform.position.y += spd;
	}
	else if (Input.GetKey("down")) {
		transform.position.y -= spd;
	}
		
}

function OnTriggerEnter2D (col : Collider2D){
	Debug.Log("Hit!!");
	if(col.gameObject.tag == 'Enemy'){
		Destroy(col.gameObject);
	}
}