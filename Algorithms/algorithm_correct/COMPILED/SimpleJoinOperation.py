class Joiner():

	def join(self, inputArrayArray):
		newArray = []
		
		for i in range(len(inputArrayArray)):
			tmpArray = []
			for j in range(len(inputArrayArray[i])):
				tmpArray.append(inputArrayArray[i][j])
			newArray.append(tmpArray)

		return newArray
