class FileInput():

	data = [];

	def loadFile(self, fileName):
		self.data = []; # clear data (in case of same variable name?)
		f = open(fileName, 'r')

		for line in f:
			line = line.rstrip()
			self.data.append(line)

	def getData(self):
		return self.data
