from sklearn.naive_bayes import GaussianNB

class NaiveBayes():

	model = None
	
	def predict(self, trained_path, input):
		model = joblib.load(trained_path)
		return model.predict(input)
	
	def train(self, training, *args):
		model = GaussianNB()
		model.fit(training, args[0])
	
	def passToTrain(self, training, *args):
		return [ self, training, args ]
