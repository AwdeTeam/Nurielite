class PyAlgorithmInterface():

	algOptions = {
		"path" : "C\\",
		"delim" : ",",
		"titleLine" : "false",
		"XML" : "<options>"+
		"<option pythonkey='path' guitype='file_chooser' label='Path to File' description='The path to the file' default='C\\' />" +
		"<option pythonkey='delim' guitype='text_box' label='String Delimeter' description='The string delimiter' default=',' />" +
		"<option pythonkey='titleLine' guitype='check_box' label='First Row Titles?' description='Whether the first row is the title for the columns' default='false'>" +
		"</options>"
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
OUT_DATA = importer.loadCSV({PATH}, {DELIM}, {TITLELN})
		"""
		return code.format(PATH=self.algOptions["path"], DELIM=self.algOptions["delim"], TITLELN=self.algOptions["titleLine"])

	def generateCodeLibraries(self):
		f = open("FileInput_class.py")
		FileInputLibrary = f.read()
		libraries = { "FileInputInput":FileInputLibrary };
		return libraries