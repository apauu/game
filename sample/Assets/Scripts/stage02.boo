import UnityEngine

class stage02 (MonoBehaviour): 
	private c as single = 0
	
	def Start ():
		pass
	
	def Update ():
		transform.position.y+=0.005*Mathf.Cos(c)
		c+=0.01
