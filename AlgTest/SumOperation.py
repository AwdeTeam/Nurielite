# SUMMATION OPERATION
class PyAlgorithmInterface():

	algOptions = { "Sum Amount": 0 }

	metaData = {
		"Name": "Sum Operation",
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
beast = SumOperation.SummationBeast()
OUT_DATA = beast.addThings(IN_DATA, {AMOUNT})
		"""
		return code.format(AMOUNT=self.algOptions["Sum Amount"])
		
	def generateCodeLibraries(self):
		f = open("SumOperation_class.py")
		SumOperationLib = f.read()
		libraries = { "SumOperation":SumOperationLib };
		return libraries
	
