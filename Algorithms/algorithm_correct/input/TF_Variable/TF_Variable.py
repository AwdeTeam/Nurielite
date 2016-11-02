class PyAlgorithmInterface():

	algOptions = {
		"name" : "'unnamed tensor'",
		"value" : "[]",
		"type" : "None",
		"shape" : "None",
		"XML" : "<options>"+
		"<option pythonkey='name' guitype='string_box' label='Name' description='Name of the Tensor Constant' default='unnamed tensor' />" +
		"<option pythonkey='value' guitype='text_box' label='Value' description='Tensor Value' default='[]' />" +
		"<option pythonkey='shape' guitype='text_box' label='Shape' description='Shape of the Tensor' default='None' />" +
		"<option pythonkey='type' guitype='text_box' label='Data Type' description='Type of data in Tensor' default='None' />" +
		"</options>"
		}

	metaData = {
		"Name" : "TF Constant",
		"Creator" : "Benjamin Croisdale",
		"Version" : "1.0.0",
		"Default" : "<default><inputs></inputs><output>tensor</output></default>"
		}

	def getMetaData(self):
		return self.metaData

	def getOptions(self):
		return self.algOptions

	def setOptions(self, options):
		self.algOptions = options

	def generateRunnableCode(self):
		code = """
OUT_DATA = tf.constant({VALUE}, dtype={TYPE}, shape={SHAPE}, name={NAME})
		"""
		return code.format(NAME=self.algOptions["name"],VALUE=self.algOptions["value"],TYPE=self.algOptions["type"],SHAPE=self.algOptions["shape"])

	def generateCodeLibraries(self):
		f = open("TF_Constant_class.py")
		FileInputLibrary = f.read()
		libraries = { "TF_ConstantInput":FileInputLibrary, "numpy":"" };
		return libraries
