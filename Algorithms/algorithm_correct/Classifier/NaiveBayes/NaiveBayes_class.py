from sklearn.naive_bayes import GaussianNB

class NaiveBayes():

	model = null
	
	def predict(self, trained-path, input):
		model = joblib.load(trained-path)
		return model.predict(input)
	
	def train(self, input, training, *args):
		model = GaussianNB()
		model.fit(input, training)
	
	def passToTrain(self, input, *args):
		return [ self, input, *args ]