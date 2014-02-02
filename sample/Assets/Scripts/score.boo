import UnityEngine

class score (MonoBehaviour): 

	public score as int
	private main as main
	
	def Start ():
		main = GameObject.Find("main").GetComponent[of main]()
	
	def OnGUI():
		n = main.getBlockNumber()
		guiText.text = "Stage $(main.getStageNumber()), Score $score ($n blocks)."