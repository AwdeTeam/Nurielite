class PyAlgorithmInterface():

	algOptions = {
		"path" : "C\\",
		"XML" : "<options><option pythonkey='path' guitype='file_chooser' label='Path to File' description='The path to the file' default='C\\' /></options>"
		}

	metaData = {
		"Name" : "File Output",
		"Creator" : "Benjamin Croisdale",
		"Version" : "1.0.1",
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
exporter = FileOutputOutput.FileOutput()
exporter.write(IN_DATA, {VALUE})
		"""
		return code.format(VALUE=self.algOptions["path"])

	def generateCodeLibraries(self):
		f = open("FileOutput_class.py")
		FileOutputLibrary = f.read()
		libraries = { "FileOutputOutput":FileOutputLibrary };
		return libraries