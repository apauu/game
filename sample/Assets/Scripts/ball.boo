import UnityEngine

class ball (MonoBehaviour): 

	private main as main
	
	def Start ():
		main = GameObject.Find("main").GetComponent[of main]()
	
	
	def Update ():
		if main.isStarted():
			t = rigidbody2D.velocity.y
			if -0.5<t and t<0.5:
				if t<=0:
					rigidbody2D.AddForce(Vector2(0,-3))
					Debug.Log("AddForce <0")
				else:
					rigidbody2D.AddForce(Vector2(0,3))
					Debug.Log("AddForce >0")
			
