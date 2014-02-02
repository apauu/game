import UnityEngine

class main (MonoBehaviour): 
	
	public maxstage as int
	
	private bar as GameObject
	private ball as GameObject
	private stagenumber as int = 1
	private stage as GameObject
	private blocknumber as int = 0
	private isstarted as bool = false
	private ismenu as bool = true
	

	def Start ():
		bar = GameObject.Find("bar")
		ball = GameObject.Find("ball")
		loadstage(stagenumber)
		
	def Update ():
		if not isstarted:
			if not ismenu:
				if Input.GetButtonDown('Fire1'):
					isstarted = true
					bar.GetComponent[of bar]().SetFirstVelocity()
	
	def OnGUI():
		if ismenu:
			if GUI.Button(Rect(0, Screen.height*3/4, Screen.width/4, Screen.height/4), "<<"):
				stagenumber--
				if stagenumber==0:
					stagenumber=maxstage
				loadstage(stagenumber)
			if GUI.Button(Rect(Screen.width*3/4, Screen.height*3/4, Screen.width/4, Screen.height/4), ">>"):
				stagenumber++
				if stagenumber==maxstage+1:
					stagenumber=1
				loadstage(stagenumber)
			if GUI.Button(Rect(Screen.width/4, Screen.height*3/4, Screen.width/2, Screen.height/4), "Start"):
				ismenu=false		
				
	def loadstage(number as int):
		if stage!=null:
			Destroy(stage)
		stagename = string.Format('stage{0:00}', number)
		Debug.Log(stagename)
		prefab = Resources.Load[of GameObject](stagename)
		stage = Instantiate(prefab, Vector3.zero, Quaternion.identity)
		if prefab == null:
			raise string.Format("No Such Stage.{0}",name)
		Debug.Log(stage.transform.childCount)
		blocknumber = stage.transform.childCount
		for i in range(stage.transform.childCount):
			child = stage.transform.GetChild(i)
			if child.gameObject.name=="block3":
				blocknumber--
			for j in range(child.childCount):
				if child.GetChild(i).gameObject.name=="block3":
					blocknumber--
		
	def BlockDestroyed():
		blocknumber--
		if blocknumber==0:
			Application.LoadLevel('Clear')

	def getBlockNumber() as int:
		return blocknumber

	def isStarted() as bool:
		return isstarted
		
	def isMenu() as bool:
		return ismenu

	def getStageNumber() as int:
		return stagenumber
				
	