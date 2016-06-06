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
	class PyAlgorithm
	{
		// member variables
		private dynamic m_pyFile;
		private dynamic m_pyClass;
			
		// construction
		public PyAlgorithm(dynamic pyFile)
		{ 
			m_pyFile = pyFile; 
			m_pyClass = m_pyFile.PyAlgorithmInterface(); // (this will throw RuntimeBinderException if that class not found. This exception is handled in PythonGenerator)
			// TODO: check that the file has all the needed functions
		}
		private PyAlgorithm() // NOTE: this is just so that things don't crash if use loads invalid py file or class isn't found (program should then just pass this back instead and display an error message)
		{
			m_pyFile = null;
			m_pyClass = null;
			// TODO: manually set meta data to something that's obviously unloaded so visible to user aside from an error message
		}

		public Dictionary<string, dynamic> getOptions()
		{
			Dictionary<string, dynamic> options = new Dictionary<string, dynamic>();
			if (m_pyClass == null) { return options; }
			
			// call getOptions from IronPython interface
			dynamic pyOptions = m_pyClass.getOptions();

			PythonDictionary dictPyOptions = (PythonDictionary)pyOptions; //....is this legal??? Will I be arrested for this?

			foreach (dynamic option in dictPyOptions.Keys) { options.Add((string)option, dictPyOptions.get(option)); }

			return options;
		}

		public void setOptions(Dictionary<string, dynamic> options)
		{
			if (m_pyClass == null) { return; }
			
			// convert options to python dictionary
			PythonDictionary pyOptions = new PythonDictionary();

			foreach (string key in options.Keys) { pyOptions.Add(key, options[key]); }
			m_pyClass.setOptions(pyOptions);
		}

		public Dictionary<string, string> getMetaData()
		{
			Dictionary<string, string> metaData = new Dictionary<string, string>();
			if (m_pyClass == null) { return metaData; }

			// call getMetaData from IronPython interface
			dynamic pyMetaData = m_pyClass.getMetaData();

			PythonDictionary dictPyMetaData = (PythonDictionary)pyMetaData;

			foreach (dynamic data in dictPyMetaData.Keys) { metaData.Add((string)data, (string)dictPyMetaData.get(data)); }

			return metaData;
		}

		public string generateRunnableCode()
		{
			// in generated python incoming data should be labeled as IN_DATA, and the PythonGenerator will take care of changing it as needed
			// returned data should be labeled as a variable OUT_DATA
			if (m_pyClass == null) { return "NULL ALGORITHM"; }
			dynamic code = m_pyClass.generateRunnableCode();
			return (string)code;
		}

		public Dictionary<string, string> generateCodeLibraries()
		{
			Dictionary<string, string> libraries = new Dictionary<string, string>();
			if (m_pyClass == null) { return libraries; }

			// call generateCodeLibraries on ironPython interface 
			dynamic pyLibraries = m_pyClass.generateCodeLibraries();
			PythonDictionary dictPyLibraries = (PythonDictionary)pyLibraries;			

			// TODO: don't forget try catches in case programmer forgot to return stuff from function! (not that that happened or anything, this is hypothetical, of course)
			foreach (dynamic data in dictPyLibraries.Keys) { libraries.Add((string)data, (string)dictPyLibraries.get(data)); }
			
			return libraries;
		}

		public static PyAlgorithm getUnloadedAlgorithm() { return new PyAlgorithm(); }
	}
}
