import TrainerOutputOutput
import NaiveBayesClassifier
import MaskOperation
import FileInputInput
import numpy


#stage0OutputData = numpy.genfromtxt(fname="C:\dwl\lab\AwdeMachineLearning\Nurielite\Datasets\student-mat.csv", dtype=None, comments="#", delimiter=",", skip_header=0, skip_footer=0, converters=None, missing_values=None, filling_values=None, usecols=None, names=['names'], excludelist=None, deletechars=None, replace_space="_", autostrip=True, case_sensitive=False, defaultfmt=None, unpack=False, usemask=False, loose=True, invalid_raise=False, max_rows=None)
stage0OutputData = numpy.genfromtxt(fname="C:/dwl/lab/AwdeMachineLearning/Nurielite/Datasets/xor.txt", delimiter=",", skip_header=1, skip_footer=0, converters=None, missing_values=None, filling_values=None, usecols=None, excludelist=None, deletechars=None, replace_space="_", autostrip=True, case_sensitive=False, defaultfmt=None, unpack=False, usemask=False, loose=True, invalid_raise=False)
		
print('\nStage 0 out:' + str(stage0OutputData))

# training data
stage1InputData = stage0OutputData
masker = MaskOperation.Masker()
stage1OutputData = masker.mask(stage1InputData, [0])
		
print('\nStage 1 out:' + str(stage1OutputData))


# target
stage2InputData = stage0OutputData
masker = MaskOperation.Masker()
#stage2OutputData = masker.mask(stage2InputData, [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33])
stage2OutputData = masker.mask(stage2InputData, [1,2])
		
print('\nStage 2 out:' + str(stage2OutputData))


stage3InputData = stage1OutputData
gaussnb = NaiveBayesClassifier.NaiveBayes()
stage3OutputData = gaussnb.passToTrain(stage3InputData)
			
print('\nStage 3 out:' + str(stage3OutputData))


stage4InputData = [stage3OutputData,stage1OutputData]
trainer = TrainerOutputOutput.TrainerOutput()
trainer.trainAndDump(stage4InputData[0][0], stage4InputData[0][1], "C\\", stage4InputData[1])
		
