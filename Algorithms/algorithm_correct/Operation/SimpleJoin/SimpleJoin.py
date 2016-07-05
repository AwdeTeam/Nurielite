# JOIN
class PyAlgorithmInterface():

	algOptions = {
		"XML" : "<options> </options>"
	}

		
	metaData = {
		"Name" : "Simple Join",
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
joiner = SimpleJoinOperation.Joiner()
OUT_DATA = joiner.join(IN_DATA)
		"""
		return code

	def generateCodeLibraries(self):
		f = open("SimpleJoin_class.py")
		JoinOperationLibrary = f.read()
		libraries = { "SimpleJoinOperation":JoinOperationLibrary };
		return libraries
