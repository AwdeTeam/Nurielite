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
		public static void loadAlgorithmBlock(string sName, AlgorithmType eFamily, int iNumInputs, int iNumOutputs)
		{
			// load the python algorithm 
			PythonGenerator pPyGen = new PythonGenerator();
			PyAlgorithm pPyAlgorithm = pPyGen.loadPythonAlgorithm(eFamily.ToString() + "/" + sName, sName + ".py");

			// get algorithm meta data for default nodule labels (should be under key "default")
            Dictionary<string, string> dAlgMetaData = pPyAlgorithm.getMetaData();
            List<string> lInputNames = new List<string>();
            string sOutputNames = "";
            if(dAlgMetaData.ContainsKey("Default")) //TODO add error handling
            {
                string sRawLabelsXml = dAlgMetaData["Default"];
                XElement pLabelsXml = XElement.Parse(sRawLabelsXml);
                XElement pInputRootXml = pLabelsXml.Element("inputs");
                foreach (XElement pElement in pInputRootXml.Elements("input"))
                    lInputNames.Add(pElement.Value);
                sOutputNames = pLabelsXml.Element("output").Value;
            }
            else
            {
				// if no labels provided, set default "unnamed datatype"
                sOutputNames = (iNumOutputs == 1) ? "unnamed datatype" : "";
                for (int i = 0; i < iNumInputs; i++) 
                    lInputNames.Add("unnamed datatype");
            }

			Block pBlock = AlgorithmLoader.generateBlock(sName, eFamily, iNumInputs, iNumOutputs, lInputNames, sOutputNames);
			pBlock.PyAlgorithm = pPyAlgorithm;
			Master.Blocks.Add(pBlock);
		}
	
        private static Block generateBlock(string sName, AlgorithmType eFamily, int iNumInputs, int iNumOutputs, List<string> lInputNames, string sOutputName)
        {
			// ensure proper label count
            if(iNumInputs > lInputNames.Count)
            {
                for (int i = lInputNames.Count; i < iNumInputs; i++)
                    lInputNames.Add("unnamed datatype");
            }
            else if(iNumInputs < lInputNames.Count)
            {
                lInputNames.RemoveRange(iNumInputs, lInputNames.Count - iNumInputs);
            }

			// set the block color by algorithm family
            Color pColor = Colors.Gray;
            switch(eFamily)
            {
                case AlgorithmType.Classifier:
                    pColor = Colors.SeaGreen;
                    break;
                case AlgorithmType.Clustering:
                    pColor = Colors.Turquoise;
                    break;
                case AlgorithmType.DimensionReduction:
                    pColor = Colors.Thistle;
                    break;
                case AlgorithmType.Operation:
                    pColor = Colors.SkyBlue;
                    break;
                case AlgorithmType.Input:
                    pColor = Colors.Violet;
                    break;
                case AlgorithmType.Output:
                    pColor = Colors.Violet;
                    break;
            }
            return new Block(lInputNames, sOutputName, sName, eFamily, pColor);
        }
    }
}
