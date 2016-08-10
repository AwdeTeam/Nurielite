import FileOutputOutput
import NaiveBayesClassifier
import MaskOperation
import FileInputInput
import numpy


#importer = FileInputInput.FileInput()
stage0OutputData = numpy.genfromtxt(fname="D:/Workbench/Projectum/Nurielite/Datasets/xor.txt", comments="#", delimiter=",", skip_header=0, skip_footer=0, converters=None, missing_values=None, filling_values=None, usecols=None, excludelist=None, deletechars=None, replace_space="_", autostrip=False, case_sensitive=False, defaultfmt=None, unpack=False, usemask=False, loose=False, invalid_raise=False)
		

stage1InputData = stage0OutputData
masker = MaskOperation.Masker()
stage1OutputData = masker.mask(stage1InputData, [0])
		

stage2InputData = stage1OutputData
gaussnb = NaiveBayesClassifier.NaiveBayes()
stage2OutputData = gaussnb.predict("D:/Workbench/Projectum/Nurielite/Algorithms/algorithm_correct/COMPILED/OUTPUT/trained_model", stage2InputData)
			

stage3InputData = stage2OutputData
exporter = FileOutputOutput.FileOutput()
exporter.write(stage3InputData, "D:/Workbench/Projectum/Nurielite/Algorithms/algorithm_correct/COMPILED/OUTPUT/predictions.txt")
		