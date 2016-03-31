class PyAlgorithmInterface():

	algOptions = {
		'option1': 3, 
		'option2': "THINGY"
		};

	def getMetaData(self):
		return "This is ma data of meta!" 

	def getOptions(self):
		return self.algOptions;
		
	def setOptions(self, options):
		self.algOptions = options

	def generateCode(self):
		return "<CODE FOR OPERATION TEST>"
