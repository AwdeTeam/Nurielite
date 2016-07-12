import FileOutputOutput
import ScalarAddOperation
import FileInputInput


importer = FileInputInput.FileInput()
stage0OutputData = importer.loadCSV(C\)
		
print('\nStage 0 out:' + str(stage0OutputData))


stage1InputData = stage0OutputData
adder = ScalarAddOperation.ScalarAdder()
stage1OutputData = adder.add(stage1InputData, 0)
		
print('\nStage 1 out:' + str(stage1OutputData))


stage2InputData = stage1OutputData
exporter = FileOutputOutput.FileOutput()
exporter.write(stage2InputData, C\)
		