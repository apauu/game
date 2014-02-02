import UnityEngine

class newGame (MonoBehaviour): 

	def Start ():
		pass
	
	def Update ():
		pass

	def OnGUI() as void:
		if GUI.Button(Rect(Screen.width/2-200, Screen.height*3/4, 400, 80), 'new game'):
			Application.LoadLevel('main')