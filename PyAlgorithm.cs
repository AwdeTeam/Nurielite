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
    /// <summary>
    /// A direct C# representation of a python PyAlgorithmInterface class. This gives access to all the normal functions available in the python interface.
    /// </summary>
	public class PyAlgorithm
	{
		// member variables
		private dynamic m_pyFile;
		private dynamic m_pyClass;
        private string m_sAlgName;
        private string m_sAlgPath;
        private PythonDictionary m_pHeldOptions;
        private List<int> m_lDependencies;
			
		// construction
		/// <summary>
		/// Constructs a PyAlgorithm instance from the passed python code file (which should contain the PyAlgorithmInterface class)
		/// </summary>
		/// <param name="pyFile">PyAlgorithmInterface class python file.</param>
		public PyAlgorithm(dynamic pyFile)
		{ 
			m_pyFile = pyFile; 
			m_pyClass = m_pyFile.PyAlgorithmInterface(); // (this will throw RuntimeBinderException if that class not found. This exception is handled in PythonGenerator)
            m_lDependencies = new List<int>();
            // TODO: check that the file has all the needed functions
		}
		private PyAlgorithm() // NOTE: this is just so that things don't crash if use loads invalid py file or class isn't found (program should then just pass this back instead and display an error message)
		{
			m_pyFile = null;
			m_pyClass = null;
			// TODO: manually set meta data to something that's obviously unloaded so visible to user aside from an error message
		}

        //properties
		/// <summary>
		/// The name of the algorithm.
		/// </summary>
        public string AlgorithmName { get { return m_sAlgName; } set { m_sAlgName = value; } }
		/// <summary>
		/// The relative filepath (based on algorithm family name and algorith name) to the PyAlgorithmInterface file
		/// </summary>
        public string AlgorithmPath { get { return m_sAlgPath; } set { m_sAlgPath = value; } }

        //functions
		/// <summary>
		/// Gets the dictionary of algorithm options from the PyAlgorithmInterface.
		/// </summary>
		/// <returns>The dictionary of algorithm options from the PyAlgorithmInterface</returns>
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

		/// <summary>
		/// Updates the algorithm options in the interface.
		/// </summary>
		/// <param name="dOptions">Updated dictionary of algorithm options.</param>
		public void setOptions(Dictionary<string, dynamic> dOptions)
		{
			if (m_pyClass == null) { return; }
			
			// convert options to python dictionary
			PythonDictionary pPyOptions = new PythonDictionary();

			foreach (string sKey in dOptions.Keys) { pPyOptions.Add(sKey, dOptions[sKey]); }
			m_pyClass.setOptions(pPyOptions);
		}

		/// <summary>
		/// Gets the dictionary of meta data (version, author, etc.) from the PyAlgorithmInterface.
		/// </summary>
		/// <returns>Dictionary of meta data.</returns>
		public Dictionary<string, string> getMetaData()
		{
			Dictionary<string, string> dMetaData = new Dictionary<string, string>();
			if (m_pyClass == null) { return dMetaData; }

			// call getMetaData from IronPython interface
			dynamic pPyMetaData = m_pyClass.getMetaData();

			PythonDictionary pPyDictMetaData = (PythonDictionary)pPyMetaData;

			foreach (dynamic pData in pPyDictMetaData.Keys) { dMetaData.Add((string)pData, (string)pPyDictMetaData.get(pData)); }

			return dMetaData;
		}

		/// <summary>
		/// Runs the ironpython PyAlgorithmInterface method for generating the code that runs the represented algorithm.
		/// </summary>
		/// <returns>Python code that runs the algorithm.</returns>
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

		/// <summary>
		/// Calls the ironpython PyAlgorithmInterface method for getting all import references and the code associated with them.
		/// </summary>
		/// <remarks>If the value for a key is a blank string, it is a system/external library, and the code should NOT be copied to a local file.</remarks>
		/// <returns>Returns the dictionary with imports as the keys and associated library code as values.</returns>
		public Dictionary<string, string> generateCodeLibraries()
		{
			Dictionary<string, string> dLibraries = new Dictionary<string, string>();
			if (m_pyClass == null) { return dLibraries; }

			// call generateCodeLibraries on ironPython interface 
			dynamic pPyLibraries = m_pyClass.generateCodeLibraries();
			PythonDictionary pPyDictLibraries = (PythonDictionary)pPyLibraries;			

			// TODO: don't forget try catches in case programmer forgot to return stuff from function! (not that that happened or anything, this is hypothetical, of course)
			foreach (dynamic pData in pPyDictLibraries.Keys) { dLibraries.Add((string)pData, (string)pPyDictLibraries.get(pData)); }
			
			return dLibraries;
		}

		/// <summary>
		/// Gets a null algorithm.
		/// </summary>
		/// <returns>Null PyAlgorithm.</returns>
		public static PyAlgorithm getUnloadedAlgorithm() { return new PyAlgorithm(); }

		/// <summary>
		/// Establishes which algorithms this pyalgorithm depends on for input. NOTE: This is used in the toposort
		/// </summary>
		/// <param name="lDependencies">We assume it's the IDs of the algorithms...</param>
		/// <returns>Returns an instance of this pyalgorithm.</returns>
        public PyAlgorithm setDependancies(List<int> lDependencies)
        {
            m_lDependencies = lDependencies;
            return this;
        }

		/// <summary>
		/// Gets the list of algorithms this pyalgorithm depends on for input. NOTE: This is used in the toposort
		/// </summary>
		/// <returns>Returns the list of algorithm ids this pyalgorithm depends on for input.</returns>
        public List<int> getDependancies() { return m_lDependencies; }
    }
}
