using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using IronPython.Hosting;
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
			m_pyClass = m_pyFile.PyAlgorithmInterface(); // (this will throw runtimebinderexception if that class not found. This exception is handled in PythonGenerator)
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
			
			dynamic pyOptions = m_pyClass.getOptions();

			IronPython.Runtime.PythonDictionary dictPyOptions = (IronPython.Runtime.PythonDictionary)pyOptions; //....is this legal??? Will I be arrested for this?

			foreach (dynamic thing in dictPyOptions.Keys) { options.Add((string)thing, dictPyOptions.get(thing)); }

			return options;
		}

		public static PyAlgorithm getUnloadedAlgorithm() { return new PyAlgorithm(); }
	}
}
