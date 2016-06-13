class PyAlgorithmInterface():

	algOptions = {
		"path" : "C\\",
		"XML" : "<options><option pythonkey='path' guitype='txtbox' label='Path to File' description='The path to the file' default='C\\' /></options>"
		}

	metaData = {
		"Name" : "File Input",
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
importer = FileInputInput.FileInput()
OUT_DATA = importer.loadCSV({VALUE})
		"""
		return code.format(VALUE=self.algOptions["path"])

	def generateCodeLibraries(self):
		f = open("FileInput_class.py")
		FileInputLibrary = f.read()
		libraries = { "FileInputInput":FileInputLibrary };
		return libraries