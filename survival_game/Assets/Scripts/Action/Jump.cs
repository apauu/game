using UnityEngine;
using System.Collections;

public class Jump{
	
	public static void JumpMove(Rigidbody2D rigidbody2D, float jumpSpeed) {
		Vector2 vctMove = new Vector2();
		vctMove.x = rigidbody2D.velocity.x;
		vctMove.y = jumpSpeed;
		rigidbody2D.velocity = vctMove;
	}
}
