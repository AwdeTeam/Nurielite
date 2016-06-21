class Joiner():

	def join(self, inputArrayArray, indexOrder):
		newArray = []
		
		# obtain the length of each input array
		arrayLengths = []
		for i in range(len(inputArrayArray)):
			arrayLengths.append(len(inputArrayArray[i]))

		for i in range(len(indexOrder)):
			index = indexOrder[i]
			
			inputNum = -1 # represents which input array
			currentArray = 0 # the current array to check
			currentLengthSum = 0 # sum of lengths of arrays so far
			while inputNum == -1:
				currentLenghtSum += arrayLengths[currentArray]
				if index < currentLengthSum:
					inputNum = currentArray + 1
					if inputNum > len(arrayLengths):
						# TODO TODO TODO error!!!!
					currentLengthSum -= arrayLengths[currentArray]
					break
				currentArray += 1
				if currentArray > len(arrayLengths):
					# TODO TODO TODO error!!!!

			newArray.append(inputArrayArray[inputNum][index - currentLenghtSum])

		return newArray
