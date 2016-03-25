using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nurielite
{
    public class Datalabel
    {
        private List<string> m_realLabels;
        private List<string> m_intLabels;
        private List<int> m_realLength; //Sum should equal num reals
        private List<int> m_intLength; //Sum should equal num ints

        public Datalabel(List<string> intLabels, List<string> realLabels, List<int> intLength, List<int> realLength)
        {
            // TODO: Complete member initialization
            this.m_intLabels = intLabels;
            this.m_realLabels = realLabels;
            this.m_intLength = intLength;
            this.m_realLength = realLength;
        }

        public Datalabel(ArrayList intLabels, ArrayList realLabels, ArrayList intLength, ArrayList realLength) :
            this(intLabels.Cast<string>().ToList(), realLabels.Cast<string>().ToList(), intLength.Cast<int>().ToList(), realLength.Cast<int>().ToList()) { }

        public Datalabel(string[] intLabels, string[] realLabels, int[] intLength, int[] realLength) :
            this(new ArrayList(intLabels), new ArrayList(realLabels), new ArrayList(intLength), new ArrayList(realLength)) { }

        public string intLabel(int dex)
        {
            return m_intLabels[cascade(dex, true)];
        }

        public string realLabel(int dex)
        {
            return m_realLabels[cascade(dex, false)];
        }

        private int cascade(int init, Boolean r)
        {
            return cascade(init, 0, r);
        }

        private int cascade(int init, int dex, Boolean r)
        {
            if(!r)
            {
                if (init < m_realLength[dex])
                    return dex;
                return cascade(init - m_realLength[dex], dex + 1, r);
            }
            else
            {
                if (init < m_intLength[dex])
                    return dex;
                return cascade(init - m_intLength[dex], dex + 1, r);
            }
        }

        internal static Datalabel join(Datatype first, Datatype second)
        {
            return first.getDatalabel().append(second.getDatalabel());
        }

        private Datalabel append(Datalabel second)
        {
            List<string> p1 = new List<string>();
            List<string> p2 = new List<string>();
            List<int> p3 = new List<int>();
            List<int> p4 = new List<int>();

            p1.AddRange(m_intLabels);
            p1.AddRange(second.m_intLabels);
            p2.AddRange(m_realLabels);
            p2.AddRange(second.m_realLabels);
            p3.AddRange(m_intLength);
            p3.AddRange(second.m_intLength);
            p4.AddRange(m_realLength);
            p4.AddRange(second.m_realLength);
            return new Datalabel(p1, p2, p3, p4);
        }
    }
}
