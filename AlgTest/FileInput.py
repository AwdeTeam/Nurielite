# SIMPLE FILE INPUT
class PyAlgorithmInterface():
	
	algOptions = { "File Path": "" }

	metaData = {
		"Name": "Simple file input",
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
fileInput = FileInput.FileInputNode()
fileInput.loadFile("{FILE_NAME}")
OUT_DATA = fileInput.getData()
		"""
		return code.format(FILE_NAME=self.algOptions["File Path"])
		
	def generateCodeLibraries(self):
		f = open("FileInput_class.py")
		FileInputLibContent = f.read()
		libraries = { "FileInput":FileInputLibContent, "sys":"" };
		return libraries
