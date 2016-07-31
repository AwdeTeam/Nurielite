from sklearn.naive_bayes import GaussianNB

class NaiveBayes():

	model = null
	
	def predict(self, trained_path, input):
		model = joblib.load(trained_path)
		return model.predict(input)
	
	def train(self, trainingData, *args):
		model = GaussianNB()
		model.fit(trainingData, args[0]) #args[0] is the column of classification labels
	
	def passToTrain(self, trainingData, *args):
		return [ self, trainingData, args ]
