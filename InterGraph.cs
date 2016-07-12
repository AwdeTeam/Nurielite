using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
    class InterGraph
    {
        private List<InterNode> m_inodes;

        public InterGraph()
        {
            m_inodes = new List<InterNode>();
        }

        /*  Thank you wikipedia!
         * 
         *	L ← Empty list that will contain the sorted elements
         *	S ← Set of all nodes with no incoming edges
         *	while S is non-empty do
         *	    remove a node n from S
         *	    add n to tail of L
         *	    for each node m with an edge e from n to m do
         *	        remove edge e from the graph
         *	        if m has no other incoming edges then
         *	            insert m into S
         *	if graph has edges then
         *	    return error (graph has at least one cycle)
         *	else 
         *	    return L (a topologically sorted order)
         */

        public List<PyAlgorithm> topoSort() //This might break if there are cycles, 
                                            //so we need to check for them further up the chain
        {
            if (m_inodes == null)
                return new List<PyAlgorithm>();

            List<InterNode> L = new List<InterNode>();
            List<InterNode> S = new List<InterNode>();

            foreach (InterNode n in m_inodes)
                if (n.inDegree() == 0)
                    S.Add(n);

            List<PyAlgorithm> algs = new List<PyAlgorithm>();

            while(S.Count > 0)
            {
                InterNode n = S.ElementAt(0);
                S.Remove(n);

                L.Add(n);
                foreach(InterNode m in n.getOutgoing())
                {
                    m.disconnect(n);
                    if (m.inDegree() == 0)
                        S.Add(m);
                }
            }

            

            foreach (InterNode n in L)
                algs.Add(n.getAlgorithm().setDependancies(getSortedDependancies(n, L)));

            return algs;
        }

        private List<int> getSortedDependancies(InterNode n, List<InterNode> L)
        {
            List<int> sorted = new List<int>();
            foreach(int x in n.Dependancies)
            {
                for (int i = 0; i < L.Count; i++)
                    if (L[i].getCore().ID == x)
                        sorted.Add(i);
            }
            return sorted;
        }

        public InterNode get(Block r) 
        {
            foreach (InterNode n in m_inodes)
                if (n.getCore().Equals(r))
                    return n;
            return null;
        }

        public Boolean contains(InterNode n) { return m_inodes.Contains(n); }
        public void append(InterNode n) { 
            m_inodes.Add(n);
            Master.log("Appending InterNode with ID " + n.getCore().ID + " to graph");
        }

        internal bool contains(Block r)
        {
            foreach (InterNode i in m_inodes)
                if (i.getCore().ID == r.ID)
                    return true;

            return false;
        }

        internal bool contains(int id)
        {
            foreach (InterNode i in m_inodes)
                if (i.getCore().ID == id)
                    return true;

            return false;
        }
    }
}
