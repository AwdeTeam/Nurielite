# SCALAR MULTIPLY
class PyAlgorithmInterface():
	
	algOptions = {
		"scalarVal" : 0, 
		"XML" : "<options><option pythonkey='scalarVal' guitype='txtbox' label='Scalar Value' description='The scalar to multiply with incoming data' default='0' /></options>"
		}
		
	metaData = {
		"Name" : "Scalar Multiply",
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
multiplier = ScalarMultiplyOperation.ScalarMultiplier()
OUT_DATA = multiplier.multiply(IN_DATA, {VALUE})
		"""
		return code.format(VALUE=self.algOptions["scalarVal"])

	def generateCodeLibraries(self):
		f = open("ScalarMultiply_class.py")
		ScalarMultiplyLibrary = f.read()
		libraries = { "ScalarMultiplyOperation":ScalarMultiplyLibrary };
		return libraries
