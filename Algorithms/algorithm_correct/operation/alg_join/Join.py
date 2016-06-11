# JOIN
class PyAlgorithmInterface():

	algOptions = {
		"ColOrder" : [],
		"XML" : "<options><option pythonkey='ColOrder' guitype='txtbox' label='Column Order' description='List indicies of columns (comma delimited) in the order they should be combined' /></options>"
		}

		
	metaData = {
		"Name" : "Join",
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
joiner = JoinOperation.Joiner()
OUT_DATA = joiner.join(IN_DATA, {VALUE})
		"""
		return code.format(VALUE=self.algOptions["ColOrder"])

	def generateCodeLibraries(self):
		f = open("Join_class.py")
		JoinOperationLibrary = f.read()
		libraries = { "JoinOperation":JoinOperationLibrary };
		return libraries
