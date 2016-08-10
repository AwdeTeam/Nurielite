from sklearn.externals import joblib
import numpy as np

class TrainerOutput():
	
	def trainAndDump(self, model, trainingData, path, *args):
		print("self: " + str(self))
		print("model: " + str(model))
		print("trainingData: " + str(trainingData))
		print("path: " + str(path))
		print("args: "+ str(args))
		
		T = args[0]
		Y = T.flatten()
		
		print("array: " + str(Y))
		
		model.train(trainingData, *args)
		joblib.dump(model, path)
