#pragma strict

var speed : float = 6.0;
var jumpSpeed : float = 8.0;
var gravity : float = 20.0;

private var moveDirection : Vector3 = Vector3.zero;
private var controller:CharacterController;

function Start () {

	controller = GetComponent(CharacterController);
}

function Update () {
	//地面についているかどうか
	if (controller.isGrounded) {
		
		//移動方向を取得
		moveDirection = Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		
		//UPボタン押下
		if (Input.GetKey("up")) {
			Debug.Log("Push UP Key");
			moveDirection.y += jumpSpeed;
		}
	}
	// 重力を計算
	moveDirection.y -= gravity * Time.deltaTime;
	
	// 移動
	controller.Move(moveDirection * Time.deltaTime);
}

function OnCollisionEnter(col : Collision) {
	Debug.Log("atatat!");
	//衝突判定用の処理をする
	if(col.gameObject.name == "Enemy"){
		//それと衝突した
	}
}