import TrainerOutputOutput
import NaiveBayesClassifier
import FileInputInput


importer = FileInputInput.FileInput()
stage0OutputData = importer.loadCSV(C\)
		
print('\nStage 0 out:' + str(stage0OutputData))


stage1InputData = stage0OutputData
gaussnb = NaiveBayesClassifier.NaiveBayes()
stage1OutputData = gaussnb.passToTrain(stage1InputData)
			
print('\nStage 1 out:' + str(stage1OutputData))


stage2InputData = [stage1OutputData,stage0OutputData]
trainer = TrainerOutputOutput.TrainerOutput()
trainer.loadAndDump(stage2InputData[0][0], stage2InputData[0][1], stage2InputData[1], "C\\", stage2InputData[0][2] )
		