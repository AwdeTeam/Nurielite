# SCALAR ADD
class PyAlgorithmInterface():

	algOptions = {
		"scalarVal" : 0,
		"XML" : "<options><option pythonkey='scalarVal' guitype='txtbox' label='Scalar Value' description='The scalar to add to incoming data' default='0' /></options>"
		}

	metaData = {
		"Name" : "Scalar Add",
		"Creator" : "Nathan Martindale",
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
adder = ScalarAddOperation.ScalarAdder()
OUT_DATA = adder.add(IN_DATA, {VALUE})
		"""
		return code.format(VALUE=self.algOptions["scalarVal"])

	def generateCodeLibraries(self):
		f = open("ScalarAdd_class.py")
		ScalarAddLibrary = f.read()
		libraries = { "ScalarAddOperation":ScalarAddLibrary };
		return libraries
