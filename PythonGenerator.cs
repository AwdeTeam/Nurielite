using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Nurielite
{
    class PythonGenerator
    {
		public PythonGenerator() { }

		public void testme()
		{
			var ipy = Python.CreateRuntime();
			dynamic test = ipy.UseFile("IPyTest.py");
			test.simpleFunction();
		}
    }
}
