class PyAlgorithmInterface():

	algOptions = {
		'option1': 3, 
		'option2': "THINGY"
		};

	metaData = {
		'Name': "THE THING",
		'Creator': "Nathan",
		'Version': "1.0.0",
		'Accuracy': ".0009"
		};

	def getMetaData(self):
		return self.metaData

	def getOptions(self):
		return self.algOptions
		
	def setOptions(self, options):
		self.algOptions = options

	def generateRunnableCode(self):
		return "<CODE FOR OPERATION TEST " + str(self.algOptions["option2"]) + " " + str(self.algOptions["option1"]) + ">"

	def generateCodeLibraries(self):
		return {}
