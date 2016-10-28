# JOIN
class PyAlgorithmInterface():

	algOptions = {
		"name" : "'unnamed'",
		"XML" : "<options><option pythonkey='ColOrder' guitype='string_box' label='Name' description='Tensorflow internal name' /></options>"
		}


	metaData = {
		"Name" : "TF Add",
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
OUT_DATA = tf.add(IN_DATA[0], IN_DATA[1], {NAME})
		"""
		return code.format(NAME=self.algOptions["name"])

	def generateCodeLibraries(self):
		f = open("TF_Add_class.py")
		TF_Add_OperationLibrary = f.read()
		libraries = { "TF_Add_Operation":TF_Add_OperationLibrary };
		return libraries
