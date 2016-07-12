using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Media;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

using Microsoft.CSharp.RuntimeBinder; // needed for RuntimeBinderException


namespace Nurielite
{
    class PythonGenerator
    {
        public static string ALGORITHM_DIRECTORY = "..\\..\\Algorithms\\algorithm_correct\\";

		// member variables
		private ScriptRuntime m_pRuntime;
		private ScriptScope m_pScope;
		private ScriptEngine m_pEngine;
		
		private MemoryStream m_pOutputStream;
		private MemoryStream m_pErrorStream;

		// construction
		public PythonGenerator() 
		{
			// set up python script engine stuff
			m_pRuntime = Python.CreateRuntime();
			m_pEngine = m_pRuntime.GetEngine("Python");
			m_pScope = m_pEngine.CreateScope();

			m_pOutputStream = new MemoryStream();
			m_pErrorStream = new MemoryStream();

			// redirect python output to the memory stream
			m_pRuntime.IO.SetOutput(m_pOutputStream, new StreamWriter(m_pOutputStream));
			m_pRuntime.IO.SetErrorOutput(m_pErrorStream, new StreamWriter(m_pErrorStream));
		}
		
		// properties
		
		// returns everything that python scripts have output since last clearRuntimeOutput()
		public string getRuntimeOutput() { return readFromStream(m_pOutputStream); }
		public string getRuntimeErrorOutput() { return readFromStream(m_pErrorStream); }
		
		// functions
		public void clearRuntimeOutput() { m_pOutputStream.SetLength(0); }

		// returns index of dynamic instance (other classes can get that particular instance)
		// TODO: don't forget to check that it contains necessary methods
		// TODO: static?
		public PyAlgorithm loadPythonAlgorithm(string sPath, string sName)
		{
			dynamic pAlgorithm = m_pRuntime.UseFile(sPath + "\\" + sName);

			try
			{
				PyAlgorithm pAlg = new PyAlgorithm(pAlgorithm);
                pAlg.AlgorithmPath = sPath;
                pAlg.AlgorithmName = sName;
				return pAlg;
			}
			catch (RuntimeBinderException e)
			{
				Master.log("Python class 'PyAlgorithmInterface' not found. the ironpython interface files MUST contain a class'PyAlgorithmInterface.'\nError text: " + e.Message, Colors.Red);
			}

			return PyAlgorithm.getUnloadedAlgorithm();
		}

		// TODO: make loadpython function work based off of a common inputpath as well, maybe make inputpath a class member variable?

		// NOTE: eventually pass in representation list, each representation should have an associated pyAlgorithm
		// also note that all functions in algorithms that are made to take data should on some level have the first param be previous layer data 
		// OUTPUT PATH SHOULD NOT INCLUDE TRAILING SLASH, input path is so local files can be found (also note that output path is in relation to input path
		public void generatePythonCode(List<PyAlgorithm> pAlgorithms, string sInputPath, string sOutputPath)
		{
			//Directory.SetCurrentDirectory(sInputPath);
			Master.log("Generating python...");
			// take care of classes and imports, obviously ignore duplicates 

			// if a library dictionary from a pyalgorithm has a value of blank, just add that import (means to external library)

			List<string> pImports = new List<string>();

			string sRunnableCode = "";

			// tempoary stage variables, change this cause it won't work with branching diagrams
			int iInStage = 0;
			int iOutStage = 0;

			foreach (PyAlgorithm pAlg in pAlgorithms)
			{
				// handle libraries first
				string sPrevLoc = Directory.GetCurrentDirectory();
				
                Directory.SetCurrentDirectory(pAlg.AlgorithmPath);
				Dictionary<string, string> pLibraries = pAlg.generateCodeLibraries();

				Directory.SetCurrentDirectory(sPrevLoc);
				foreach (string sLibName in pLibraries.Keys)
				{
					if (pImports.Contains(sLibName)) { continue; } // don't do anything if we've already done something with the libraries needed for this algorithm 
					
					// TODO: check if it starts with 'from'?
					sRunnableCode = "import " + sLibName + "\n" + sRunnableCode;
					pImports.Add(sLibName);
					
					if (pLibraries[sLibName] != "") // copy library to compile folder if not external
					{
						File.WriteAllText(sOutputPath + "\\" + sLibName + ".py", pLibraries[sLibName]); 
					}
				}
                Directory.SetCurrentDirectory(pAlg.AlgorithmPath);
				
				// parse template code for data connections
				// TODO: this will eventually have to be based on representation connections 
				string sVerbatimCode = pAlg.generateRunnableCode();
                bool bValidCodeFlag = true;
                if(sVerbatimCode == null) { bValidCodeFlag = false; }

				if (bValidCodeFlag)
				{
					List<int> pDep = pAlg.getDependancies();

					if (sVerbatimCode.Contains("IN_DATA"))
					{
						iInStage++;
						string sDepComp = "";
						if (pDep.Count < 2)
							sDepComp = "stage" + (pDep[0]) + "OutputData";
						else
						{
							sDepComp = "[";
							foreach (int i in pDep)
								sDepComp += "stage" + (i) + "OutputData,";
							sDepComp.Substring(0, sDepComp.Length - 2);
							sDepComp += "]";
						}

						// connect input of this algorithm to output of last algorithm
						sVerbatimCode = "\nstage" + iInStage + "InputData = " + sDepComp + sVerbatimCode;
						sVerbatimCode = sVerbatimCode.Replace("IN_DATA", "stage" + iInStage + "InputData");

					}
					if (sVerbatimCode.Contains("OUT_DATA"))
					{
						sVerbatimCode = sVerbatimCode.Replace("OUT_DATA", "stage" + iOutStage + "OutputData");

						sVerbatimCode += "\nprint('\\nStage " + iOutStage + " out:' + str(stage" + iOutStage + "OutputData))\n";
						iOutStage++;
					}

					sRunnableCode += "\n" + sVerbatimCode;
					Directory.SetCurrentDirectory(sPrevLoc);
				}
				else { throw new Exception("Runnable python code failed to generate!"); }
			}

			// write runnable code to output
			File.WriteAllText(sOutputPath + "\\driver.py", sRunnableCode);

			Master.log("Python generated!");
		}
		
		// thanks to https://blogs.msdn.microsoft.com/seshadripv/2008/07/08/how-to-redirect-output-from-python-using-the-dlr-hosting-api/
		private string readFromStream(MemoryStream ms) // gets everything from inside the passed memory stream
		{
			int length = (int)ms.Length;
			Byte[] bytes = new Byte[length];

			ms.Seek(0, SeekOrigin.Begin);
			ms.Read(bytes, 0, (int)ms.Length);

			return Encoding.GetEncoding("utf-8").GetString(bytes, 0, (int)ms.Length);
		}
    }
}
