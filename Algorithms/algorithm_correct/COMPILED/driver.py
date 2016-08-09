import TrainerOutputOutput
import NaiveBayesClassifier
import FileInputInput
import numpy


#importer = FileInputInput.FileInput()
stage0OutputData = numpy.genfromtxt(fname='C:\\', comments='#', delimiter=',', skip_header=0, skip_footer=0, converters=None, missing_values=None, filling_values=None, usecols=None, excludelist=None, deletechars=None, replace_space='_', autostrip=False, case_sensitive=False, defaultfmt='f%i', unpack=False, usemask=False, loose=True, invalid_raise=True)
		

stage1InputData = stage0OutputData
gaussnb = NaiveBayesClassifier.NaiveBayes()
stage1OutputData = gaussnb.passToTrain(stage1InputData)
			

stage2InputData = [stage1OutputData,stage0OutputData]
trainer = TrainerOutputOutput.TrainerOutput()
trainer.trainAndDump(stage2InputData[0][0], stage2InputData[0][1], stage2InputData[1], "C\\", stage2InputData[0][2] )
		