import TrainerOutputOutput
import NaiveBayesClassifier
import MaskOperation
import FileInputInput
import numpy


#importer = FileInputInput.FileInput()
stage0OutputData = numpy.genfromtxt(fname='C:\\', comments='#', delimiter=',', skip_header=0, skip_footer=0, converters=None, missing_values=None, filling_values=None, usecols=None, excludelist=None, deletechars=None, replace_space='_', autostrip=False, case_sensitive=False, defaultfmt='f%i', unpack=False, usemask=False, loose=True, invalid_raise=True)
		

stage1InputData = stage0OutputData
masker = MaskOperation.Masker()
stage1OutputData = masker.mask(stage1InputData, [])
		

stage2InputData = stage0OutputData
masker = MaskOperation.Masker()
stage2OutputData = masker.mask(stage2InputData, [])
		

stage3InputData = stage1OutputData
gaussnb = NaiveBayesClassifier.NaiveBayes()
stage3OutputData = gaussnb.passToTrain(stage3InputData)
			

stage4InputData = stage2OutputData
masker = MaskOperation.Masker()
stage4OutputData = masker.mask(stage4InputData, [])
		

stage5InputData = [stage3OutputData,stage4OutputData]
trainer = TrainerOutputOutput.TrainerOutput()
trainer.trainAndDump(stage5InputData[0][0], stage5InputData[0][1], "C\\", stage5InputData[1], stage5InputData[0][2])
		