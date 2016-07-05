class ScalarAdder():
	
	def add(self, inputArray, scalarValue):
		newArray = []
		for i in range(len(inputArray)):
			tmpArray = []
			for j in range(len(inputArray[i])):
				tmpArray.append(int(int(inputArray[i][j]) + int(scalarValue)))
			newArray.append(tmpArray)

		return newArray
