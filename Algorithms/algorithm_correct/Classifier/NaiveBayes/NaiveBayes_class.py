from sklearn.naive_bayes import GaussianNB
from sklearn.externals import joblib
import numpy as np

class NaiveBayes():

	model = None
	
	def predict(self, trained_path, input):
		print(trained_path)
		model = joblib.load(trained_path)
		X = np.asarray(input)
		print("X: " + str(X))
		return model.predict(X, X.shape)
	
	def train(self, trainingData, *args):
		model = GaussianNB()
		model.fit(trainingData, args[0]) #args[0] is the column of classification labels
	
	def passToTrain(self, trainingData, *args):
		return [ self, trainingData, args ]
