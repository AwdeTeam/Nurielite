from sklearn.naive_bayes import GaussianNB

class NaiveBayes():

	model = None
	
	def predict(self, trained_path, input):
		model = joblib.load(trained_path)
		return model.predict(input)
	
	def train(self, input, training, *args):
		model = GaussianNB()
		model.fit(input, training)
	
	def passToTrain(self, input, *args):
		return [ self, input, args ]