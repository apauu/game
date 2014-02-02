import UnityEngine

class stage04 (MonoBehaviour): 
	private c as single = 0
	
	def Start ():
		pass
	
	def Update ():
		transform.position.x+=0.008*Mathf.Cos(c)
		c+=0.01
