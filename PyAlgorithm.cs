using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			// TODO: NEEDS TO GO IN TRY CATCH 
			m_pyClass = m_pyFile.PyAlgorithmInterface();
		}

		//public List<>
		public Dictionary<string, dynamic> getOptions()
		{
			Dictionary<string, dynamic> options = new Dictionary<string, dynamic>();
			dynamic pyOptions = m_pyClass.getOptions();

			IronPython.Runtime.PythonDictionary dictPyOptions = (IronPython.Runtime.PythonDictionary)pyOptions; //....is this legal??? Will I be arrested for this?

			/*foreach (dynamic thing in pyOptions)
			{
				Master.log("Thing: " + thing + ":" + thing.getKey());
			}*/
			foreach (dynamic thing in dictPyOptions.Keys)
			{
				//Master.log("Thing:" + thing + ": " + dictPyOptions.get(thing));
				options.Add((string)thing, dictPyOptions.get(thing));
			}

			return options;
		}
	}
}
