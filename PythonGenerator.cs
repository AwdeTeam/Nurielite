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
		// member variables
		private ScriptRuntime m_runtime;
		private ScriptScope m_scope;
		private ScriptEngine m_engine;
		
		private MemoryStream m_outputStream;
		private MemoryStream m_errorStream;

		// construction
		public PythonGenerator() 
		{
			// set up python script engine stuff
			m_runtime = Python.CreateRuntime();
			m_engine = m_runtime.GetEngine("Python");
			m_scope = m_engine.CreateScope();

			m_outputStream = new MemoryStream();
			m_errorStream = new MemoryStream();

			// redirect python output to the memory stream
			m_runtime.IO.SetOutput(m_outputStream, new StreamWriter(m_outputStream));
			m_runtime.IO.SetErrorOutput(m_errorStream, new StreamWriter(m_errorStream));
		}
		
		// properties
		
		// returns everything that python scripts have output since last clearRuntimeOutput()
		public string getRuntimeOutput() { return readFromStream(m_outputStream); }
		public string getRuntimeErrorOutput() { return readFromStream(m_errorStream); }
		
		// functions
		public void clearRuntimeOutput() { m_outputStream.SetLength(0); }

		// returns index of dynamic instance (other classes can get that particular instance)
		public PyAlgorithm loadPythonAlgorithm(string fileName)
		{
			dynamic algorithm = m_runtime.UseFile(fileName);

			try
			{
				PyAlgorithm alg = new PyAlgorithm(algorithm);
				return alg;
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
		public void generatePythonCode(List<PyAlgorithm> algorithms, string inputPath, string outputPath)
		{
			Directory.SetCurrentDirectory(inputPath);
			Master.log("Generating python...");
			// take care of classes and imports, obviously ignore duplicates 

			// if a library dictionary from a pyalgorithm has a value of blank, just add that import (means to external library)

			List<string> imports = new List<string>();

			string runnableCode = "";

			// tempoary stage variables, change this cause it won't work with branching diagrams
			int inStage = 0;
			int outStage = 0;

			foreach (PyAlgorithm alg in algorithms)
			{
				// handle libraries first
				Dictionary<string, string> libraries = alg.generateCodeLibraries();

				foreach (string libName in libraries.Keys)
				{
					if (imports.Contains(libName)) { continue; } // don't do anything if we've already done something with the libraries needed for this algorithm 
					
					// TODO: check if it starts with 'from'?
					runnableCode = "import " + libName + "\n" + runnableCode;
					imports.Add(libName);
					
					if (libraries[libName] != "") 
					{
						File.WriteAllText(outputPath + "\\" + libName + ".py", libraries[libName]); // copy library to compile folder
					}
				}

				// parse template code for data connections
				// TODO: this will eventually have to be based on representation connections 
				string verbatimCode = alg.generateRunnableCode();

				if (verbatimCode.Contains("IN_DATA"))
				{
					inStage++;

					verbatimCode = "\nstage" + inStage + "InputData = stage" + outStage + "OutputData\n" + verbatimCode;
					verbatimCode = verbatimCode.Replace("IN_DATA", "stage" + inStage + "InputData");
				}
				if (verbatimCode.Contains("OUT_DATA"))
				{
					outStage++;
					verbatimCode = verbatimCode.Replace("OUT_DATA", "stage" + outStage + "OutputData");

					verbatimCode += "\nprint('\\nStage " + outStage + " out:' + str(stage" + outStage + "OutputData))\n";
				}

				/*verbatimCode = verbatimCode.Replace("OUT_DATA", "stageOneInputData");
				verbatimCode += "\nprint(stageOneInputData)";*/

				runnableCode += "\n" + verbatimCode;
			}

			// write runnable code to output
			/*StreamWriter runnableSW = new StreamWriter(outputPath + "\\driver.py");
			runnableSW.Write(runnableCode);*/
			File.WriteAllText(outputPath + "\\driver.py", runnableCode);

			Master.log("Python generated!");
		}
		
		
		
		public void testme()
		{
			//engine.Execute("print 'hello from inside c#'");
			dynamic test = m_runtime.UseFile("../../IPyTest.py");
			test.simpleFunction();
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
