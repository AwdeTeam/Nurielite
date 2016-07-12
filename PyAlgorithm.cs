using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;


namespace Nurielite
{
	public class PyAlgorithm
	{
		// member variables
		private dynamic m_pyFile;
		private dynamic m_pyClass;
        private string m_sAlgName;
        private string m_sAlgPath;
        private PythonDictionary m_pHeldOptions;
        private List<int> m_pDependancies;
			
		// construction
		public PyAlgorithm(dynamic pyFile)
		{ 
			m_pyFile = pyFile; 
			m_pyClass = m_pyFile.PyAlgorithmInterface(); // (this will throw RuntimeBinderException if that class not found. This exception is handled in PythonGenerator)
            m_pDependancies = new List<int>();
            // TODO: check that the file has all the needed functions
		}
		private PyAlgorithm() // NOTE: this is just so that things don't crash if use loads invalid py file or class isn't found (program should then just pass this back instead and display an error message)
		{
			m_pyFile = null;
			m_pyClass = null;
			// TODO: manually set meta data to something that's obviously unloaded so visible to user aside from an error message
		}

        //properties
        public string AlgorithmName { get { return m_sAlgName; } set { m_sAlgName = value; } }
        public string AlgorithmPath { get { return m_sAlgPath; } set { m_sAlgPath = value; } }

        //functions
		public Dictionary<string, dynamic> getOptions()
		{
			Dictionary<string, dynamic> pOptions = new Dictionary<string, dynamic>();
			if (m_pyClass == null) { return pOptions; }
			
			// call getOptions from IronPython interface
			dynamic pPyOptions = m_pyClass.getOptions();

			PythonDictionary pPyDictOptions = (PythonDictionary)pPyOptions; //....is this legal??? Will I be arrested for this?

			foreach (dynamic pOption in pPyDictOptions.Keys) { pOptions.Add((string)pOption, pPyDictOptions.get(pOption)); }

			return pOptions;
		}

		public void setOptions(Dictionary<string, dynamic> pOptions)
		{
			if (m_pyClass == null) { return; }
			
			// convert options to python dictionary
			PythonDictionary pPyOptions = new PythonDictionary();

			foreach (string sKey in pOptions.Keys) { pPyOptions.Add(sKey, pOptions[sKey]); }
			m_pyClass.setOptions(pPyOptions);
		}

		public Dictionary<string, string> getMetaData()
		{
			Dictionary<string, string> pMetaData = new Dictionary<string, string>();
			if (m_pyClass == null) { return pMetaData; }

			// call getMetaData from IronPython interface
			dynamic pPyMetaData = m_pyClass.getMetaData();

			PythonDictionary pPyDictMetaData = (PythonDictionary)pPyMetaData;

			foreach (dynamic pData in pPyDictMetaData.Keys) { pMetaData.Add((string)pData, (string)pPyDictMetaData.get(pData)); }

			return pMetaData;
		}

		public string generateRunnableCode()
		{
			// in generated python incoming data should be labeled as IN_DATA, and the PythonGenerator will take care of changing it as needed
			// returned data should be labeled as a variable OUT_DATA
			if (m_pyClass == null) { return "NULL ALGORITHM"; }
            dynamic pCode = null;
            try { pCode = m_pyClass.generateRunnableCode(); }
            catch (IronPython.Runtime.Exceptions.TypeErrorException e) { }
			return (string)pCode;
		}

		public Dictionary<string, string> generateCodeLibraries()
		{
			Dictionary<string, string> pLibraries = new Dictionary<string, string>();
			if (m_pyClass == null) { return pLibraries; }

			// call generateCodeLibraries on ironPython interface 
			dynamic pPyLibraries = m_pyClass.generateCodeLibraries();
			PythonDictionary pPyDictLibraries = (PythonDictionary)pPyLibraries;			

			// TODO: don't forget try catches in case programmer forgot to return stuff from function! (not that that happened or anything, this is hypothetical, of course)
			foreach (dynamic pData in pPyDictLibraries.Keys) { pLibraries.Add((string)pData, (string)pPyDictLibraries.get(pData)); }
			
			return pLibraries;
		}

		public static PyAlgorithm getUnloadedAlgorithm() { return new PyAlgorithm(); }

        public PyAlgorithm setDependancies(List<int> pDependancies)
        {
            m_pDependancies = pDependancies;
            return this;
        }

        public List<int> getDependancies() { return m_pDependancies; }
    }
}
