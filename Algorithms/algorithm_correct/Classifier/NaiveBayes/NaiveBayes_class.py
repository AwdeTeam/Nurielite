from sklearn.naive_bayes import GaussianNB
from sklearn.externals import joblib

class NaiveBayes():

	model = null
	
	def predict(self, trained-path, input):
		model = joblib.load(trained-path)
		return model.predict(input)
	
	def train(self, trained-path='!NEWMODEL', input, training)
		model = GaussianNB()
		model.fit(input, training)
	
	def save(self, trained-path='!NEWMODEL', name)
		