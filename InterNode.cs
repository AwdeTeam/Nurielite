using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
	/// <summary>
	/// Vertex representation in an <see cref="Intergraph"/>intergraph of a block and its algorithm.
	/// </summary>
    public class InterNode
    {
        private LinkedList<InterNode> m_pInNodes;
        private LinkedList<InterNode> m_pOutNodes;
        private List<int> m_pDependancies;
        private Block m_pCore;
        private InterGraph m_pMaster;

		/// <summary>
		/// Initializes an internode inside a graph.
		/// </summary>
		/// <param name="pCore">The block which will serve as the logical base for the internode.</param>
		/// <param name="pMaster">The graph into which the internode will be placed.</param>
        public InterNode(Block pCore, InterGraph pMaster)
        {
            if (!pMaster.contains(pCore.ID))
            {
                m_pInNodes = new LinkedList<InterNode>();
                m_pOutNodes = new LinkedList<InterNode>();
                m_pDependancies = new List<int>();
                m_pCore = pCore;
                m_pMaster = pMaster;

                pMaster.append(this);

                updateLinks();
            }
        }

        /// <summary>
        /// Creates a link from the current internode to the passed internode.
        /// </summary>
        /// <param name="pNode">The internode that will be the endpoint of the new link between the two internodes.</param>
        public void connectTo(InterNode pNode)
        {
            pNode.m_pInNodes.AddLast(this);
            pNode.m_pDependancies.Add(this.m_pCore.ID);
            m_pOutNodes.AddLast(pNode);
        }

        /// <summary>
        /// Removes any from the current internode to the passed internode.
        /// </summary>
        /// <param name="pNode">The internode to remove the link towards.</param>
        public void disconnect(InterNode pNode)
        {
			if (this.m_pInNodes.Contains(pNode)) { m_pInNodes.Remove(pNode); }
			if (this.m_pOutNodes.Contains(pNode)) { m_pOutNodes.Remove(pNode); }
        }

        /// <summary>
        /// Counts the number of incoming links.
        /// </summary>
        /// <returns>Integer number of incoming links.</returns>
        public int inDegree() { return m_pInNodes.Count; }

        /// <summary>
        /// Counts the number of outgoing links.
        /// </summary>
        /// <returns>Integer number of outgoing links.</returns>
        public int outDegree() { return m_pOutNodes.Count; }

        /// <summary>
        /// Collects all incoming links.
        /// </summary>
        /// <returns>List of incoming links.</returns>
        public List<InterNode> getIncoming() { return m_pInNodes.ToList(); }

        /// <summary>
        /// Collects all outgoing links.
        /// </summary>
        /// <returns>List of outgoing links.</returns>
        public List<InterNode> getOutgoing() { return m_pOutNodes.ToList(); }

        /// <summary>
        /// Returns the block associated with the internode.
        /// </summary>
        /// <returns>Block associated with the internode.</returns>
        /// <seealso cref="Block"/>
        public Block getCore() { return m_pCore; }

        /// <summary>
        /// Checks to see if the internodes have the same core.
        /// </summary>
        /// <param name="pNode"></param>
        /// <returns></returns>
        public Boolean Equals(InterNode pNode) { return pNode.getCore().Equals(m_pCore); }

        /// <summary>
        /// Gets a list of dependent indexes.
        /// </summary>
        public List<int> Dependancies { get { return m_pDependancies; } }

        /// <summary>
        /// Finds the associated PyAlgorithm.
        /// </summary>
        /// <returns></returns>
        public PyAlgorithm getAlgorithm()
        {
			//return py;
			return m_pCore.PyAlgorithm; //TODO: CHANGE!!@!!
        }

        private void updateLinks()
        {
            List<Block> lrep = m_pCore.getOutgoing();

            foreach (Block r in lrep)
            {
                if (m_pMaster.contains(r.ID))
                {
                    if (Master.VerboseMode) Master.log("InterNode with rep id " + m_pCore.ID + " is connecting to already extant InterNode " + r.ID);
                    connectTo(m_pMaster.get(r));
                }
                else
                {
                    InterNode inode = new InterNode(r, m_pMaster);
                    if (Master.VerboseMode) Master.log("InterNode with rep id " + m_pCore.ID + " is connecting to new InterNode " + r.ID);
                    connectTo(inode);
                }
            }

        }
    }
}
