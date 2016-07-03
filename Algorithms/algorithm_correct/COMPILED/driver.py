import FileOutputOutput
import ScalarAddOperation
import FileInputInput


importer = FileInputInput.FileInput()
stage1OutputData = importer.loadCSV({VALUE})
		
print('\nStage 1 out:' + str(stage1OutputData))


stage1InputData = stage1OutputData

adder = ScalarAddOperation.ScalarAdder()
stage2OutputData = adder.add(stage1InputData, 0)
		
print('\nStage 2 out:' + str(stage2OutputData))


exporter = FileOutputOutput.FileOutput()
exporter.write(stage3OutputData, C\)
		
print('\nStage 3 out:' + str(stage3OutputData))
