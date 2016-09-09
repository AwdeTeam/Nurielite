using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

// (connect format should be: "connect -[outputNodeRepresetnationID (from)],[nodeNum] -[inputNodeRepresentationID (to)],[nodeNum]

namespace Nurielite
{
    /// <summary>
    /// The logical component that controls the interface between <see cref="Connection"/> Connections and <see cref="Block"/> Blocks.
    /// </summary>
    /// <seealso cref="NoduleGraphic"/>
	public class Nodule
	{
		// TODO: add labels on hover (right click allows you to change?) [for naming groups?]

		// member variables
		private Block m_pParent;
		private NoduleGraphic m_pGraphic;

		private List<Connection> m_lConnections = new List<Connection>();

		private bool m_bIsInput = true; // false means this is output node
		private int m_iGroupNum = 0; // semi-id system, which node of input/output is it? (ex: 2nd input node etc) NOTE: 0-based
        private string m_sName = "unnamed datatype";
        
		// construction

        /// <summary>
        /// Builds a new Nodule.
        /// </summary>
        /// <param name="pParent">The <see cref="Block"/> Block to which the Nodule will serve as an attachment point.</param>
        /// <param name="bIsInput">A switch that controls whether the Nodule serves as incoming data or sends outgoing data to/from a <see cref="Connection"/> Connection.</param>
        /// <param name="iGroupNum">The number of Nodules in the grouping of Nodules on the Block.</param>
        /// <param name="sName">The name or label applied to Connections from this Nodule.</param>
		public Nodule(Block pParent, bool bIsInput, int iGroupNum, string sName) // PASS IN X AND Y OF BLOCK
		{
			m_pParent = pParent;
			m_bIsInput = bIsInput;
			m_iGroupNum = iGroupNum;
            m_sName = sName;

			m_pGraphic = new NoduleGraphic(this);
		}

		// properties
		// TODO: some of these don't need setters?

        /// <summary>
        /// The parent block to which this nodule is attached.
        /// </summary>
		public Block Parent { get { return m_pParent; } set { m_pParent = value; } }

        /// <summary>
        /// The <see cref="NoduleGraphic"/> responsible for handling the graphical representation of the nodule.
        /// </summary>
		public NoduleGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }

        /// <summary>
        /// A switch that describes whether the nodule serves as incoming data or sends outgoing data to/from a <see cref="Connection"/>.
        /// </summary>
		public bool IsInput { get { return m_bIsInput; }}

        /// <summary>
        /// The number of Nodules in the grouping of nodules on the block.
        /// </summary>
		public int GroupNum { get { return m_iGroupNum; } set { m_iGroupNum = value; } }

        /// <summary>
        /// A list of all <see cref="Connection"/>s connected to the nodule.
        /// </summary>
		public List<Connection> Connections { get { return m_lConnections; } set { m_lConnections = value; } }

        /// <summary>
        /// The number of <see cref="Connection"/>s connected to the nodule.
        /// </summary>
		public int NumConnections { get { return m_lConnections.Count; } }

        /// <summary>
        /// The name or label applied to connections from this nodule.
        /// </summary>
        public string Name { get { return m_sName; } set { m_sName = value; } }
		
		
		// -- FUNCTIONS --

        /// <summary>
        /// Appends a new <see cref="Connection"/> to the nodule.
        /// </summary>
        /// <param name="pConnection">Connection to append.</param>
		public void addConnection(Connection pConnection) { m_lConnections.Add(pConnection); }

        /// <summary>
        /// Removes the passed <see cref="Connection"/> from the nodule/
        /// </summary>
        /// <param name="pConnection">Connection to remove.</param>
		public void removeConnection(Connection pConnection) { m_lConnections.Remove(pConnection); }

		// finishes out the connection

        /// <summary>
        /// Checks if the <see cref="Connection"/> is valid an finalizes its creation.
        /// </summary>
        /// <param name="pConnection"></param>
		public void connect(Connection pConnection)
		{
			if (!pConnection.completeConnection(this)) { return; } // need actual deletion code for connection stuff?
			//maybe this function should return true if connection successful, false if not?

			// add connection to both nodes' collection
			m_lConnections.Add(pConnection);
			pConnection.Origin.addConnection(pConnection);
		}

        /// <summary>
        /// Removes this nodule from its <see cref="Block"/> along with any of this nodule's <see cref="Connection"/>s.
        /// </summary>
		public void deleteNodule()
		{
			for (int i = m_lConnections.Count - 1; i >= 0; i--) { m_lConnections[i].removeConnection(); }
			m_lConnections = null;
			m_pGraphic.deleteGraphic();
		}
	}
}
