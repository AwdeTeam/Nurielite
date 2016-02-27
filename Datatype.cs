using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgGui
{
    public class Datatype
    {
        public enum Type { Null, Boolean, Integer, BoundedReal, Real, Complex, String, Image, Aggregate }

        private Type m_type = Type.Null;
        private string m_name = "";
        private int m_rank = -1;
        private Datatype[] m_bundle = null;

        private static Datatype[] Directory = null; //This is a VERY temporary solution; will need a more effecient structure later

        private Datatype(){}

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

        public static Datatype aggregateTypes(String name, Datatype[] agg)
        {
            Datatype r = new Datatype();
            r.m_type = Type.Aggregate;
            r.m_name = name;
            r.m_rank = 0;
            Console.Out.WriteLine(agg.Length);
			for (int i = 0; i < agg.Length; i++) { r.m_rank += agg[i].m_rank; }
            r.m_bundle = agg;
            if (!appendType(r))
            {
             //   throw new UnauthorizedAccessException(); //Probably not the right exception to use...
            }
            return r;
        }

        public static Datatype aggregateTypes(String name, Datatype a, Datatype b)
        {
            return aggregateTypes(name, new Datatype[] { a, b });
        }

        public static Datatype aggregateTypes(String name, Datatype a, Datatype b, Datatype c)
        {
            return aggregateTypes(name, new Datatype[] { a, b, c });
        }

        public static Datatype defineDatatype(String name, Type type, int rank)
        {
            Datatype r = new Datatype();
            r.m_type = type;
            r.m_name = name;
            r.m_rank = rank;
            appendType(r);
            return r;
        }

        public Boolean equals(Datatype compare) //Datatype names SHOULD be unique!
        {
            return compare.m_name.Equals(m_name) && fits(compare);
        }

        public Boolean fits(Datatype compare)
        {
            return compare.m_rank == m_rank && compare.m_type == m_type;
        }

        public String getName() { return m_name; }
        public Type getType() { return m_type; }
        public int getRank() { return m_rank; }

        public static void testingTypes()
        {
            defineDatatype("32x32 Grayscale", Type.Image, 32 * 32);
            defineDatatype("Flower Properties", Type.Real, 15);
            defineDatatype("Chartered Accountants", Type.Integer, 1);
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

		public override string ToString()
		{
			return this.m_name;
		}
    }
}
