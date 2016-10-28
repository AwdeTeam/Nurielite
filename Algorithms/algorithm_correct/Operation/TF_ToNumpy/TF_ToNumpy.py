class PyAlgorithmInterface():

	algOptions = {}

	metaData = {
		"Name" : "TF To Numpy",
		"Creator" : "Benjamin Croisdale",
		"Version" : "1.0.0",
		"Default" : "<default><inputs><input>calculated data</input></inputs><output></output></default>"
		}

	def getMetaData(self):
		return self.metaData

	def getOptions(self):
		return self.algOptions

	def setOptions(self, options):
		self.algOptions = options

	def generateRunnableCode(self):
		code = """
print(IN_DATA)
		"""
		return code.format(VALUE=self.algOptions["path"])

	def generateCodeLibraries(self):
		f = open("ConsoleOutput_class.py")
		ConsoleOutputLibrary = f.read()
		libraries = { "ConsoleOutputOutput":ConsoleOutputLibrary };
		return libraries
