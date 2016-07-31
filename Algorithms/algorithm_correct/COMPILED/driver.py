import TrainerOutputOutput
import NaiveBayesClassifier
import MaskOperation
import FileInputInput
import numpy


stage0OutputData = numpy.genfromtxt("D:\Workbench\Projectum\datasets\student\student-mat.csv", None, "#", ",", 0, 0, None, None, None, None, ['names'], None, None, "_", True, False, None, False, False, True, False, False)
		
print('\nStage 0 out:' + str(stage0OutputData))


stage1InputData = stage0OutputData
masker = MaskOperation.Masker()
stage1OutputData = masker.mask(stage1InputData, [1])
		
print('\nStage 1 out:' + str(stage1OutputData))


stage2InputData = stage0OutputData
masker = MaskOperation.Masker()
stage2OutputData = masker.mask(stage2InputData, [2,3,4,5,6,7,8,9,10,11,12])
		
print('\nStage 2 out:' + str(stage2OutputData))


stage3InputData = stage2OutputData
gaussnb = NaiveBayesClassifier.NaiveBayes()
stage3OutputData = gaussnb.passToTrain(stage3InputData)
			
print('\nStage 3 out:' + str(stage3OutputData))


stage4InputData = [stage1OutputData,stage3OutputData]
trainer = TrainerOutputOutput.TrainerOutput()
trainer.loadAndDump(stage4InputData[0][0], stage4InputData[0][1], stage4InputData[1], "C\\", stage4InputData[0][2] )
		