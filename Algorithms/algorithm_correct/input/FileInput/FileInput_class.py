import fileinput

class FileInput():
	
	def loadCSV(self, path, delim=",", type="float"):
		file = fileinput.input(path)
		
		data = []
		
		if type == "string":
			for line in file:
				data.append(line.split(delim))
		elif type == "float":
			for line in file:
				ray = []
				for value in line.split(delim):
					ray.append(float(value))
				data.append(ray)
		elif type == "int":
			for line in file:
				ray = []
				for value in line.split(delim):
					ray.append(int(float(value)))
				data.append(ray)
		
		return data