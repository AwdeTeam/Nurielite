class SummationBeast():

	def addThings(self, addToArray, amount):
		newArray = [];
		for i in range(0, len(addToArray)):
			newArray.append(int(int(addToArray[i]) + int(amount)))

		return newArray
