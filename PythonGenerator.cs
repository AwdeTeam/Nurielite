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
		public PythonGenerator() { }

		public string testme()
		{
			ScriptRuntime ipy = Python.CreateRuntime();
			ScriptEngine engine = ipy.GetEngine("Python");
			ScriptScope scope = engine.CreateScope();
			//var ipy = Python.CreateRuntime();

			// setup output stuff
			//MemoryStream outputStream = new MemoryStream();
			//MemoryStream outputStream = new MemoryStream(1024);
			//StreamReader sr = new StreamReader(outputStream);
			
			
			MemoryStream outputStream = new MemoryStream();
			//StreamWriter outputStreamWriter = new StreamWriter(outputStream);
			//ipy.IO.SetOutput(outputStream, outputStreamWriter);
			ipy.IO.SetOutput(outputStream, new StreamWriter(outputStream));

			engine.Execute("print 'hello from inside c#'");
			dynamic test = ipy.UseFile("../../IPyTest.py");
			test.simpleFunction();

			string str = readFromStream(outputStream);
			return str;

			//return sr.ReadToEnd();

			//return sr.ReadLine();
		}

		// https://blogs.msdn.microsoft.com/seshadripv/2008/07/08/how-to-redirect-output-from-python-using-the-dlr-hosting-api/
		static string readFromStream(MemoryStream ms)
		{
			int length = (int)ms.Length;
			Byte[] bytes = new Byte[length];

			ms.Seek(0, SeekOrigin.Begin);
			ms.Read(bytes, 0, (int)ms.Length);

			return Encoding.GetEncoding("utf-8").GetString(bytes, 0, (int)ms.Length);
		}
    }
}
