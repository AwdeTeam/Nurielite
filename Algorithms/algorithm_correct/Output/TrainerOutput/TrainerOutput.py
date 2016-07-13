class PyAlgorithmInterface():

	algOptions = {
		"path" : "\"C\\\\\"",
		"XML" : "<options><option pythonkey='path' guitype='file_chooser' label='Path to File' description='The path to the file' default='C\\' /></options>"
		}

	metaData = {
		"Name" : "Trainer Output",
		"Creator" : "Benjamin Croisdale",
		"Version" : "1.0.0"
		}

	def getMetaData(self):
		return self.metaData

	def getOptions(self):
		return self.algOptions

	def setOptions(self, options):
		self.algOptions = options

	def generateRunnableCode(self):
		code = """
trainer = TrainerOutputOutput.TrainerOutput()
trainer.loadAndDump(IN_DATA[0][0], IN_DATA[0][1], IN_DATA[1], {VALUE}, IN_DATA[0][2] )
		"""
		return code.format(VALUE=self.algOptions["path"])

	def generateCodeLibraries(self):
		f = open("TrainerOutput_class.py")
		TrainerOutputLibrary = f.read()
		libraries = { "TrainerOutputOutput":TrainerOutputLibrary };
		return libraries