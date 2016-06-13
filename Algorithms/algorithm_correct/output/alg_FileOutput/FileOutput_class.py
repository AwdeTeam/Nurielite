class FileOutput():
	
	def write(self, data, path, form="csv"):
		file = open(path)
		
		s = ""
		
		for line in data:
			for value in line:
				s += ","
			s += "\n"
		
		file.write(s)