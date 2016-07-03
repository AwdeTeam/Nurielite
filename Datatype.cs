using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
    public class Datatype
    {
		//member variables
        private string m_sName = "";
        private int m_iNumReals = 0;
		private List<string> m_pLabel;

		public static Dictionary<string, Datatype> Directory = null;
		
		//construction
        private Datatype(){}

		//properties
		public List<string> Label { get { return m_pLabel; } set { m_pLabel = value; } }
		public string Name { get { return m_sName; } set { m_sName = value; } }
		public int NumReals { get { return m_iNumReals; } set { m_iNumReals = value; } }
		
		//functions
        private Datatype(int reals, string name, List<string> pLabel) 
        {
            m_iNumReals = reals;
            m_sName = name;
			m_pLabel = pLabel;
        }

        private static bool appendType(string sName, Datatype pDatatype)
        {
            if (Directory == null) { Directory = new Dictionary<string, Datatype>(); }
			if (Directory.ContainsKey(sName)) { return false; }

			Directory.Add(sName, pDatatype);
            return true;
        }

        public static Datatype defineDatatype(string sName, int iNumReals, List<string> pLabel)
        {
            Datatype r = new Datatype();
            r.m_sName = sName;
            r.m_iNumReals = iNumReals;
			r.m_pLabel = pLabel;
            appendType(sName, r);
            return r;
        }

        public bool equals(Datatype pCompareTo) //Datatype names SHOULD be unique!
        {
            return pCompareTo.m_sName.Equals(m_sName) && fits(pCompareTo);
        }

        public bool fits(Datatype pCompareTo) { return pCompareTo.m_iNumReals == m_iNumReals; }

        public static void genericTypes()
        {
            defineDatatype("Scalar Real", 1, null);

            defineDatatype("32x32 Grayscale", 32 * 32, null);
            defineDatatype("256x256 Grayscale", 256 * 256, null);
            defineDatatype("1024x1024 Grayscale", 1024 * 1024, null);
            defineDatatype("1024x1024 Color", 1024 * 1024 * 3, null);
            defineDatatype("1024x1024 Alpha", 1024 * 1024 * 4, null);

            defineDatatype("2D Point", 2, null);
            defineDatatype("3D Point", 3, null);
            defineDatatype("2D Triangle", 2 * 3, null);
            defineDatatype("3D Triangle", 3 * 3, null);

            defineDatatype("2D Point Mass", 2 * 3 + 1, null);
            defineDatatype("3D Point Mass", 3 * 3 + 1, null);
        }

		public override string ToString()
		{
			return this.m_sName;
		}
    }
}
