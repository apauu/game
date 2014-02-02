import UnityEngine

class gameover (MonoBehaviour): 

	def Start ():
		pass
	
	def Update ():
		pass
	
	def OnTriggerEnter2D(other as Collider2D):
		if other.gameObject.name=="ball":
			Application.LoadLevel('gameover')