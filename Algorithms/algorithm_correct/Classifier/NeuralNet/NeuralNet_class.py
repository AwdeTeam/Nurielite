import math
import random
import numpy as np
#import theano.tensor as T

class NeuralNet:

	model = None
	
	def predict(self, trained_path, input):
		model = NeuralNetModel.load(trained_path)
		X = np.asarray(input)
		return model.predict(X, X.shape)
	
	def train(self, trainingData, *args):
		model = NeuralNetModel()
		model.fit(trainingData, args[0]) #args[0] is the column of classification labels
	
	def passToTrain(self, trainingData, *args):
		return [ self, trainingData, args ]


class NeuralNetModel:
	
	net = [][]
	
	s = lambda x: 1 / (1 + math.exp(-1*x)
	dsdx = lambda x: s(x) * (1 - s(x))
	
	def save(self):
		
	
	def load(self):
	
	
	def predict():
	
	
	def fit():
	

class Neuron:

	weights = []
	bias = 0
	
	def output(self, input, s):
		r = 0
		for i in range(0,len(input)):
			r += self.weights[i] * input[i]
		return s(r - self.bias)
	
	def learn(self, input, expexted, s, dsdx, k):
		bias = bias - k * bluh(bias)
		
		for i in range(0, len(weights)):
			weights[i] = weights[i] - k*bluh(weights[i])