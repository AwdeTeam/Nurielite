class Masker():

	def mask(self, inputArray, maskIndicies):
		newArray = []
		for i in range(len(inputArray)):
			if not (i in maskIndicies):
				newArray.append(int(inputArray[i]))

		return newArray
			
