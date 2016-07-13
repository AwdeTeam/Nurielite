from sklearn.externals import joblib

class TrainerOutput():
	
	def trainAndDump(self, model, inputData, trainingData, path, *args):
		model.train(inputData, trainingData, *args)
		joblib.dump(model, path)