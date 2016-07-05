import FileOutputOutput
import SimpleJoinOperation
import ScalarAddOperation
import FileInputInput


importer = FileInputInput.FileInput()
stage0OutputData = importer.loadCSV("D:\Workbench\Projectum\Nurielite\AlgTest\COMPILED\TestData.dat")
		
print('\nStage 0 out:' + str(stage0OutputData))


stage1InputData = stage0OutputData
adder = ScalarAddOperation.ScalarAdder()
stage1OutputData = adder.add(stage1InputData, 2)
		
print('\nStage 1 out:' + str(stage1OutputData))


stage2InputData = stage0OutputData
adder = ScalarAddOperation.ScalarAdder()
stage2OutputData = adder.add(stage2InputData, -1)
		
print('\nStage 2 out:' + str(stage2OutputData))


stage3InputData = [stage1OutputData,stage2OutputData,]
joiner = SimpleJoinOperation.Joiner()
stage3OutputData = joiner.join(stage3InputData)
		
print('\nStage 3 out:' + str(stage3OutputData))


stage4InputData = stage3OutputData
exporter = FileOutputOutput.FileOutput()
exporter.write(stage4InputData, "D:\Workbench\Projectum\Nurielite\AlgTest\COMPILED\OUTPUT.dat")
		