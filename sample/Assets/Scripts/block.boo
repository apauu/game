import UnityEngine

class block (MonoBehaviour): 
	private main as main
	
	def Start ():
		main = GameObject.Find("main").GetComponent[of main]()
	
	def Update ():
		pass
	
	def OnCollisionEnter2D(collision as Collision2D):
		Debug.Log("hit $gameObject $(collision.gameObject.name)" )
		if collision.gameObject.name=="ball":
			GameObject.Find("Score").GetComponent[of score]().score += 100
			main.BlockDestroyed()
			Destroy(gameObject)