using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
	/// <summary>
	/// Temporary graph of internodes (<see cref="InterNode"/>) that is constructed and topologically sorted for the creation of the final python code.
	/// </summary>
    public class InterGraph
    {
        private List<InterNode> m_inodes;

		/// <summary>
		/// Initializes a new graph of internodes.
		/// </summary>
        public InterGraph()
        {
            m_inodes = new List<InterNode>();
        }

		/*  Thank you Wikipedia!
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

		/// <summary>
		/// Sorts the graph of python algorithms into a linear list.
		/// </summary>
		public List<PyAlgorithm> topoSort() //This might break if there are cycles, 
											//so we need to check for them further up the chain //TODO we still have to do this
		{
			Master.log("Running topological sort...");

			if (m_inodes == null)
				return new List<PyAlgorithm>();

			List<InterNode> L = new List<InterNode>();
			List<InterNode> S = new List<InterNode>();

			foreach (InterNode n in m_inodes)
				if (n.inDegree() == 0)
					S.Add(n);

			List<PyAlgorithm> algs = new List<PyAlgorithm>();

			while (S.Count > 0)
			{
				InterNode n = S.ElementAt(0);
				S.Remove(n);

				L.Add(n);
				foreach (InterNode m in n.getOutgoing())
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

        /// <summary>
        /// Finds the internode in the intergraph corresponding to the passed block.
        /// </summary>
        /// <param name="pBlock">The block to serve as the search key.</param>
        /// <returns>If the block is in the graph, returns an internode based around that block. Otherwise returns null.</returns>
        public InterNode get(Block pBlock)
        {
            foreach (InterNode pInterNode in m_inodes)
                if (pInterNode.getCore().Equals(pBlock))
                    return pInterNode;
            return null;
        }

        /// <summary>
        /// Checks to see is the passed internode is in the graph.
        /// </summary>
        /// <param name="pInterNode">The internode to perform the test.</param>
        /// <returns>True if the internode is in the graph, false otherwise.</returns>
        public Boolean contains(InterNode pInterNode) { return m_inodes.Contains(pInterNode); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBlock"></param>
        /// <returns></returns>
        public Boolean contains(Block pBlock)
        {
            foreach (InterNode i in m_inodes)
                if (i.getCore().ID == pBlock.ID)
                    return true;

            return false;
        }

        public Boolean contains(int id)
        {
            foreach (InterNode i in m_inodes)
                if (i.getCore().ID == id)
                    return true;

            return false;
        }

        public void append(InterNode n)
        {
            m_inodes.Add(n);
            if (Master.VerboseMode) Master.log("Appending InterNode with ID " + n.getCore().ID + " to graph");
        }

		private List<int> getSortedDependancies(InterNode n, List<InterNode> L)
		{
			List<int> sorted = new List<int>();
			foreach (int x in n.Dependancies)
			{
				for (int i = 0; i < L.Count; i++)
					if (L[i].getCore().ID == x)
						sorted.Add(i);
			}
			return sorted;
		}
	}
}
