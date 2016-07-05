import csv

class FileOutput():
	
	def write(self, data, path, form="csv"):
		
		with open(path, "wb") as f:
			writer = csv.writer(f)
			writer.writerows(data)
