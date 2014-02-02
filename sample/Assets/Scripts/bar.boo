import UnityEngine

class bar (MonoBehaviour): 
	public speed as single = 1
	
	private count as int =0
	private main as main
	private ball as GameObject
	
	def Start ():
		main = GameObject.Find("main").GetComponent[of main]()
		ball = GameObject.Find("ball")
	
	def Update ():
		if main.isMenu():
			pass
		else:
			p = Camera.main.ScreenToViewportPoint(Input.mousePosition)
			transform.position = Vector2((p.x-0.5)*6,transform.position.y)
			if not main.isStarted():
				ball.transform.position.x = transform.position.x
	
	def SetFirstVelocity():
		ball.rigidbody2D.velocity = Vector2(speed * Mathf.Sin(0), speed * Mathf.Cos(0 ))		

	def OnCollisionEnter2D(collision as Collision2D):
		#Debug.Log("###")
		if collision.gameObject.name=="ball":
			r as Rigidbody2D = collision.gameObject.rigidbody2D
			barpos = transform.position
			ballpos = collision.gameObject.transform.position
			d = -barpos.x + ballpos.x
			#Debug.Log("d=$d")
			r.velocity = Vector2(speed * Mathf.Sin(d), speed * Mathf.Cos(d))
			count++
			Debug.Log("count = $count, speed = $speed")
			if count%32==0:
				speed *= 1.3
			if count%256==0:
				speed /= (1.3**6) 
				
		
	