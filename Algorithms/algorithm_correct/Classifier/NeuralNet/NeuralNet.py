class PyAlgorithmInterface():

	algOptions = {
		"path" : "\"C\\\\\"",
		"train" : "true",
		"rate" : "1.0"
		"XML" : "<option pythonkey='rate' guitype='text_box' label='Learning Rate' description='The the learning rate of the backpropagation algorithm.' default='0' /><options><option pythonkey='train' guitype='check_box' label='Train this algorithm' description='Whether or not this algorithm will be trained (requires an output)' /><option pythonkey='path' guitype='file_chooser' label='Path to File' description='The path to the file' default='C\\' /></options>"
		}

	metaData = {
		"Name" : "Neural Net",
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
		code = ""
		if(self.algOptions["train"] == "true"):
			code = """
gaussnb = NaiveBayesClassifier.NaiveBayes()
OUT_DATA = gaussnb.passToTrain(IN_DATA)
			"""
		else:
			code = """
gaussnb = NaiveBayesClassifier.NaiveBayes()
OUT_DATA = gaussnb.predict({VALUE}, IN_DATA)
			"""
		return code.format(VALUE=self.algOptions["path"])

	def generateCodeLibraries(self):
		f = open("NeuralNet_class.py")
		FileInputLibrary = f.read()
		libraries = { "NeuralNetClassifier":FileInputLibrary };
		return libraries