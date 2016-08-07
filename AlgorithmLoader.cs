using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Nurielite
{
    /// <summary>
    /// Builds a Block from a set of constraints
    /// </summary>
    /// <remarks>
    /// So... this class might be pretty much pointless as it's basically just a constructor for Block
    /// </remarks>
    class AlgorithmLoader
    {

		//NOTE: python file path can be reconstructed by eFamily/sName/sName.py
		//NOTE: THIS IS ENTRYPOINT FOR NEW BLOCK
		public static void loadAlgorithmBlock(string sName, AlgorithmType eFamily, int numInputs, int numOutputs)
		{
			PythonGenerator pPyGen = new PythonGenerator();
			//PyAlgorithm pPyAlgorithm = pPyGen.loadPythonAlgorithm(Master.PATH_TO_THETHING + "/" + eFamily.ToString() + "/" + sName, sName + ".py");
			PyAlgorithm pPyAlgorithm = pPyGen.loadPythonAlgorithm(eFamily.ToString() + "/" + sName, sName + ".py");

            Dictionary<string, string> d = pPyAlgorithm.getMetaData();
            List<string> inputNames = new List<string>();
            string outputName = "";
            if(d.ContainsKey("Default")) //TODO add error handling
            {
                string rawXML = d["Default"];
                XElement xml = XElement.Parse(rawXML);
                XElement inputs = xml.Element("inputs");
                foreach (XElement element in inputs.Elements("input"))
                    inputNames.Add(element.Value);
                outputName = xml.Element("output").Value;
            }
            else
            {
                outputName = (numOutputs == 1) ? "unnamed datatype" : "";
                for (int i = 0; i < numInputs; i++)
                    inputNames.Add("unnamed datatype");
            }

			Block pBlock = AlgorithmLoader.generateBlock(sName, eFamily, numInputs, numOutputs, inputNames, outputName);
			pBlock.PyAlgorithm = pPyAlgorithm;
			Master.Blocks.Add(pBlock);
		}
	
        private static Block generateBlock(string sName, AlgorithmType eFamily, int numInputs, int numOutputs, List<string> inputNames, string outputName)
        {
            if(numInputs > inputNames.Count)
            {
                for (int i = inputNames.Count; i < numInputs; i++)
                    inputNames.Add("unnamed datatype");
            }
            else if(numInputs < inputNames.Count)
            {
                inputNames.RemoveRange(numInputs, inputNames.Count - numInputs);
            }

            Color color = Colors.Gray;
            switch(eFamily)
            {
                case AlgorithmType.Classifier:
                {
                    color = Colors.SeaGreen;
                    break;
                }

                case AlgorithmType.Clustering:
                {
                    color = Colors.Turquoise;
                    break;
                }

                case AlgorithmType.DimensionReduction:
                {
                    color = Colors.Thistle;
                    break;
                }

                case AlgorithmType.Operation:
                {
                    color = Colors.SkyBlue;
                    break;
                }

                case AlgorithmType.Input:
                {
                    color = Colors.Violet;
                    break;
                }

                case AlgorithmType.Output:
                {
                    color = Colors.Violet;
                    break;
                }
            }
            return new Block(inputNames, outputName, sName, eFamily, color);
        }

        /*public static Block generateBlock(string name, string path, int family, Datatype[] inputs, Datatype[] outputs)
        {
            return generateBlock(name, path, (AlgorithmType) family, inputs, outputs);
        }

        public static Block load(string path)
        {
            string filePath = path;
            StreamReader sr = new StreamReader(filePath);
            string[] data = sr.ReadLine().Split(',');
            return generateBlock(data[0], path, AlgorithmType.Input, null, null);
        }*/
    }
}
