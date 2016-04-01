import SumOperation
import sys
import FileInput


fileInput = FileInput.FileInputNode()
fileInput.loadFile("TestData.dat")
stage1OutputData = fileInput.getData()
		
print('\nStage 1 out:' + str(stage1OutputData))


stage1InputData = stage1OutputData

beast = SumOperation.SummationBeast()
stage2OutputData = beast.addThings(stage1InputData, 5)
		
print('\nStage 2 out:' + str(stage2OutputData))


stage2InputData = stage2OutputData

beast = SumOperation.SummationBeast()
stage3OutputData = beast.addThings(stage2InputData, 1)
		
print('\nStage 3 out:' + str(stage3OutputData))
