import numpy as np

class Masker():

	def mask(self, inputArray, maskIndicies):
		return np.delete(inputArray, maskIndicies, 1)
