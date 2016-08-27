from sklearn.externals import joblib
import numpy as np

class TrainerOutput():
	
	def trainAndDump(self, model, trainingData, path, *args):
		T = args[0]
		Y = T.flatten()	
		model.train(trainingData, *args)
		joblib.dump(model, path)
