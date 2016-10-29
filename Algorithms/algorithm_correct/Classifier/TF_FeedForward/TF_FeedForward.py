class PyAlgorithmInterface():

	algOptions = {
		"name" : "'unnamed''",
		"type" : "0",
		"XML" : "<options>" +
		"<option pythonkey='name' guitype='string_box' label='Name of the Output' description='The name of the output for this NN layer' />" +
		"<option pythonkey='type' guitype='combo_box' label='Type' description='The activation function type used' default='0' list='" +
		"Rectified Linear," +
		"Rectified Linear 6," +
		"crelu," +
		"elu," +
		"Softplus," +
		"Softsign," +
		"dropout," +
		"bias_add," +
		"Sigmoid," +
		"Hyperboli Tangent" +
		"' />" +
		"</options>"
		}

	metaData = {
		"Name" : "TF Feed Forward",
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
OUT_DATA = makeNet({TYPE}, IN_DATA, {NAME})
		"""
		return code.format(TYPE=self.algOptions["type"], NAME=self.algOptions["name"])

	def generateCodeLibraries(self):
		f = open("TF_FeedForward_class.py")
		TF_FeedForwardLib = f.read()
		libraries = { "TF_FeedForward":TF_FeedForwardLib };
		return libraries
