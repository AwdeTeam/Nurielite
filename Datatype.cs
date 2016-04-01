using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
    public class Datatype
    {
        private string m_name = "";
        private int m_integers = 0;
        private int m_reals = 0;
        private Datalabel m_labels;

        public static Datatype[] Directory = null; //This is a VERY temporary solution; will need a more effecient structure later

        private Datatype(){}

        private Datatype(int ints, int reals, string name, Datalabel label)
        {
            m_integers = ints;
            m_reals = reals;
            m_name = name;
            m_labels = label;
        }

        private static Boolean appendType(Datatype dt)
        {
            if (Directory == null)
            {
                Directory = new Datatype[] { dt };
                return true;
            }
            
            if (Directory.Contains(dt))
                return false;

            Datatype[] n = new Datatype[Directory.Length + 1];
            for (int i = 0; i < Directory.Length; i++)
                n[i] = Directory[i];
            n[Directory.Length] = dt;
            Directory = n;
            return true;
        }

        public static Datatype defineDatatype(String name, int integer, int real, Datalabel label)
        {
            Datatype r = new Datatype();
            r.m_name = name;
            r.m_integers = integer;
            r.m_reals = real;
            r.m_labels = label;
            appendType(r);
            return r;
        }

        public static Datatype join(Datatype a, Datatype b) //Must Preserve Order
        {
            Datatype first = order(a, b, true);
            Datatype second = order(a, b, false);
            return new Datatype(first.getIntegers() + second.getIntegers(), 
                first.getReals() + second.getReals(), 
                first.getName() + " x " + second.getName(),
                Datalabel.join(first, second));
        }

        public Datatype join(Datatype b)
        {
            return Datatype.join(this, b);
        }

        public Boolean equals(Datatype compare) //Datatype names SHOULD be unique!
        {
            return compare.m_name.Equals(m_name) && fits(compare);
        }

        public Boolean fits(Datatype compare)
        {
            return compare.m_integers == m_integers && compare.m_reals == m_reals;
        }

        
        public string stringRep()
        {
            return getName() + " (" + getIntegers() + " int, " + getReals() + " real)";
        }

        public String getName() { return m_name; }
        public int getReals() { return m_reals; }
        public int getIntegers() { return m_integers; }

        public static void testingTypes()
        {
            defineDatatype("32x32 Grayscale", 32 * 32, 0, 
                new Datalabel(new string[] {"Grayscale Image"}, new string[] {}, new int[] {32*32}, new int[] {0}));

            defineDatatype("Flower Properties", 15, 0,
                new Datalabel(new string[] {"Petal Length", "Number of petals", "Petal color", "Somethingelse"}, new string[] {},
                    new int[] {1, 1, 3, 10}, new int[] {0}));

            defineDatatype("Chartered Accountants", 2, 1,
                new Datalabel(new string[] {"Hats", "Anteaters"}, new string[] {"Lion Ferocity"}, new int[] {1,1}, new int[] {1}));
        }

        public static void genericTypes()
        {
            defineDatatype("Null", 0, 0, null);
            defineDatatype("Scalar Integer", 1, 0, null);
            defineDatatype("Scalar Real", 0, 1, null);

            defineDatatype("32x32 Grayscale", 32 * 32, 0, null);
            defineDatatype("256x256 Grayscale", 256 * 256, 0, null);
            defineDatatype("1024x1024 Grayscale", 1024 * 1024, 0, null);
            defineDatatype("1024x1024 Color", 1024 * 1024 * 3, 0, null);
            defineDatatype("1024x1024 Alpha", 1024 * 1024 * 4, 0, null);

            defineDatatype("2D Point", 0, 2, null);
            defineDatatype("3D Point", 0, 3, null);
            defineDatatype("2D Triangle", 0, 2 * 3, null);
            defineDatatype("3D Triangle", 0, 3 * 3, null);

            defineDatatype("2D Point Mass", 0, 2 * 3 + 1, null);
            defineDatatype("3D Point Mass", 0, 3 * 3 + 1, null);
        }

        public static int numberOfTypes()
        {
            return Directory.Count();
        }

        public static Datatype findType(String name)
        {
			foreach (Datatype d in Directory)
			{
				if (d.m_name.Equals(name)) { return d; }
			}
            return null;
        }

        public static Datatype getType(int dex) 
        {
            try { return Directory[dex]; }
			catch(Exception e) { return null; }
        }

        public static Datatype getType(String type)
        {
            for(int i = 0; i < Directory.Length; i++)
            {
                if (Directory[i].getName().Equals(type))
                    return Directory[i];
            }
            return null;
        }

		public override string ToString()
		{
			return this.m_name;
		}

        private static Datatype order(Datatype a, Datatype b, Boolean inv)
        {
            if(a.getIntegers() < b.getIntegers())
            {
                return (inv) ? a : b;
            }
            else if(b.getIntegers() < a.getIntegers())
            {
                return (inv) ? b : a;
            }

            if (a.getReals() < b.getReals())
            {
                return (inv) ? a : b;
            }
            else if (b.getReals() < a.getReals())
            {
                return (inv) ? b : a;
            }

            if (a.getName().CompareTo(b.getName()) < 0)
            {
                return (inv) ? a : b;
            }
            else if (a.getName().CompareTo(b.getName()) > 0)
            {
                return (inv) ? b : a;
            }

            return (inv) ? a : b;
        }

        internal Datalabel getDatalabel()
        {
            return m_labels;
        }

        internal static System.Collections.IEnumerable getTypeList()
        {
            List<String> r = new List<String>();
            for (int i = 0; i < Directory.Length; i++)
                r.Add(Directory[i].getName());
            return r;
        }
    }
}
