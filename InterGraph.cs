﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
    class InterGraph
    {
        private List<InterNode> inodes;

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
            List<InterNode> L = new List<InterNode>();
            List<InterNode> S = new List<InterNode>();

            foreach (InterNode n in inodes)
                if (n.inDegree() == 0)
                    S.Add(n);
            
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

            List<PyAlgorithm> algs = new List<PyAlgorithm>();

            foreach (InterNode n in L)
                algs.Add(n.getAlgorithm());

            return algs;
        }

        public InterNode get(Representation r) 
        {
            foreach (InterNode n in inodes)
                if (n.getCore().Equals(r))
                    return n;
            return null;
        }

        public Boolean contains(InterNode n) { return inodes.Contains(n); }
        public void append(InterNode n) { inodes.Add(n); }
    }
}
