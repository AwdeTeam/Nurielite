class Joiner():

	def join(self, inputArrayArray):
		newArray = []
		
		for i in range(len(inputArrayArray)):
			for j in range(len(inputArrayArray[i])):
				newArray.append(inputArrayArray[i][j])

		return newArray
