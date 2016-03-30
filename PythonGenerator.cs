using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

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
