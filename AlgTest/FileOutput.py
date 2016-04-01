class PyAlgorithmInterface():
	
	algOptions = { "File Path": "" }

	metaData = {
		"Name": "Simple file output",
		"Creator": "Nathan",
		"Version": "1.0.0"
		}

	def getMetaData(self):
		return self.metaData

	def getOptions(self):
		return self.algOptions

	def setOptions(self, options):
		self.algOptions = options

	def generateRunnableCode(self):
		code = """
fileOut = open("{FILE_PATH}", "w")
for dataPoint in IN_DATA: 
	fileOut.write(str(dataPoint) + str("\\n"))
		"""
		return code.format(FILE_PATH=self.algOptions["File Path"])

	def generateCodeLibraries(self):
		return {}
