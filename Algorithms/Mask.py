# MASK
class PyAlgorithmInterface():

	algOptions = {
		"ColIndicies" : [],
		"XML" : "<options><option pythonkey='ColIndicies' guitype='txtbox' label='Masked Cols' description='List indicies of columns (comma delimited) you want masked (not included) in the output' /></options>"
		}

		
	metaData = {
		"Name" : "Mask",
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
masker = MaskOperation.MaskOperation()
OUT_DATA = masker.mask(IN_DATA, {VALUE})
		"""
		return code.format(VALUE=self.algOptions["ColIndicies"])

	def generateCodeLibraries(self):
		f = open("Mask_class.py")
		MaskOperationLibrary = f.read()
		libraries = { "MaskOperation":MaskOperationLibrary };
		return libraries
