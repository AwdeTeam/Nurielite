from sklearn.externals import joblib

class TrainerOutput():
	
	def trainAndDump(self, model, trainingData, path, *args):
		model.train(trainingData, *args)
		joblib.dump(model, path)
