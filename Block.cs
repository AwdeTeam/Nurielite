using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows;

namespace Nurielite
{
    /// <summary>
    /// Represents a single operation or algorithm and the input and output values related to it.
    /// </summary>
    /// <seealso cref="BlockGraphic"/>
    /// <seealso cref="InterNode"/>
    /// <seealso cref="PyAlgorithm"/>
	public class Block
	{
		// member variables
		private int m_iID = 0;
        private List<Nodule> m_lNodules = new List<Nodule>();

        private List<string> m_lInputNames;
        private string m_sOutputName;
        private int m_iOutputNum;

        private string m_sName = "unnamed algorithm";
        private string m_sVersion = "##.## XXX";
		private AlgorithmType m_eFamily = AlgorithmType.Undefined;

 		private BlockGraphic m_pGraphic;

		private PyAlgorithm m_pPyAlgorithm;

		// construction
		/// <summary>
		/// Creates a block object.
		/// </summary>
		/// <param name="lInputNames">List of labels for input nodules.</param>
		/// <param name="sOutputName">Label for output nodule.</param>
		/// <param name="sName">Name of algorithm associated with block.</param>
		/// <param name="eFamily">Type of algorithm.</param>
		/// <param name="pColor">Color of the block.</param>
        public Block(List<string> lInputNames, string sOutputName, string sName, AlgorithmType eFamily, Color pColor)
        {
            if (Master.VerboseMode) Master.log("----Creating block----");
            m_iID = Master.getNextRepID();
            if (Master.VerboseMode) Master.log("ID: " + m_iID, Colors.GreenYellow);

            m_sName = sName;
            m_eFamily = eFamily;
            m_lInputNames = lInputNames;
            m_sOutputName = sOutputName;
            m_iOutputNum = (m_sOutputName == "") ? 0 : 1; // if outputname was blank, no output nodules
            m_pGraphic = new BlockGraphic(this, m_lInputNames.Count, m_iOutputNum, pColor);
			Master.getGraphicContainer().addBlockGraphic(m_pGraphic);

			// create nodes
            for (int i = 0; i < m_lInputNames.Count; i++) { m_lNodules.Add(new Nodule(this, true, i, m_lInputNames[i])); }
            for (int i = 0; i < m_iOutputNum; i++) { m_lNodules.Add(new Nodule(this, false, i, m_sOutputName)); }
        }

		// properties 
		/// <summary>
		/// Unique identifier for block. 
		/// </summary>
		public int ID { get { return m_iID; } set { m_iID = value; } }
		/// <summary>
		/// Name of the block, but not necessarily the name of the algorithm. 
		/// </summary>
		public string Name { get { return m_sName; } set { m_sName = value; } }
		/// <summary>
		/// Version number of the python algorithm associated with this block.
		/// </summary>
		/// <remarks>
		/// NOTE: This needs to be hooked up!!!!
		/// </remarks>
		public string Version { get { return m_sVersion; } set { m_sVersion = value; } } 
		/// <summary>
		/// Type of the algorithm associated with this block.
		/// </summary>
		public AlgorithmType Family { get { return m_eFamily; } set { m_eFamily = value; } }
		/// <summary>
		/// GUI/Visual controller (<see cref="BlockGraphic"/>) associated with this block. 
		/// </summary>
		public BlockGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		/// <summary>
		/// List of all nodule objects (<see cref="Nodule"/>) attached to this block.
		/// </summary>
		public List<Nodule> Nodules { get { return m_lNodules; } set { m_lNodules = value; } }
		/// <summary>
		/// Reference to the python algorithm (<see cref="PyAlgorithm"/>) loaded into this block.
		/// </summary>
		public PyAlgorithm PyAlgorithm { get { return m_pPyAlgorithm; } set { m_pPyAlgorithm = value; } }

		// -- FUNCTIONS --

		/// <summary>
		/// Programmatically create a connection between the current block and the one specified.  
		/// </summary>
		/// <param name="pTarget">Block to connect to</param>
		/// <param name="iNoduleNum">The input nodule index of the nodule on the other block to connect to.</param>
        public void connectTo(Block pTarget, int iNoduleNum)
        {
            if (m_iOutputNum == 0 || pTarget.m_lInputNames.Count() == 0)
                return;

            foreach(Nodule pNodule in m_lNodules)
            {
                if(!pNodule.IsInput)
                {
					Nodule pNoduleTarget = pTarget.m_lNodules[iNoduleNum];
					if (pNoduleTarget.IsInput && pNoduleTarget.NumConnections == 0)
					{
						Connection pConnection = new Connection(pNodule);
						pNodule.addConnection(pConnection);
						pNoduleTarget.addConnection(pConnection);
						pConnection.InputNodule = pNoduleTarget;
						pConnection.OutputNodule = pNodule;
						pConnection.completeConnection(pNoduleTarget);
						Master.getGraphicContainer().setDraggingConnection(false, null);
					}
                }
            }
        }

		/// <summary>
		/// Safely delete this block and everything associated with it (nodules, connections, etc) 
		/// </summary>
		/// <remarks>
		/// This calls the delete function on this blocks nodules and graphic controller. 
		/// </remarks>
		public void deleteBlock()
		{
			for (int i = 0; i < m_lNodules.Count; i++) { m_lNodules[i].deleteNodule(); }
			m_lNodules = null;
			m_pGraphic.deleteGraphic();
		}

		/// <summary>
		/// Get a list of all the blocks that this block connects to.
		/// </summary>
        public List<Block> getOutgoing()
        {
            List<Block> pOutgoingBlocks = new List<Block>();

            foreach (Nodule pNodule in m_lNodules)
            {
                if (!pNodule.IsInput)
                {
                    foreach(Connection pConnection in pNodule.Connections)
                    {
                        pOutgoingBlocks.Add(pConnection.InputNodule.Parent);
                    }
                }
            }

            return pOutgoingBlocks;
        }
    }
}
