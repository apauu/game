using UnityEngine;
using System.Collections;

public class Side_Move {
	
	public static void SideMove(Rigidbody2D rigidbody2D, float sideSpeed) {
		Vector2 vctMove = new Vector2();
		vctMove.x = sideSpeed;
		vctMove.y = rigidbody2D.velocity.y;

		rigidbody2D.velocity = vctMove;
	}

	public static void SideMove(Rigidbody2D rigidbody2D, float sideSpeed, float virticalSpeed) {
		Vector2 vctMove = new Vector2();
		vctMove.x = sideSpeed;
		vctMove.y = virticalSpeed;
		
		rigidbody2D.velocity = vctMove;
	}
}
