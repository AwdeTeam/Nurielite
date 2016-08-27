using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nurielite
{
	/// <summary>
	/// Represents a pathway of data, flowing out of one python algorithm and into another.
	/// </summary>
	public class Connection
	{
		// member variables
		private Nodule m_pInNodule, m_pOutNodule; // same as below, but referenced for different reasons

		private Nodule m_pOrigin;
		private Nodule m_pEnd;

		private bool m_bCompleted = false; // if connection has been created/assigned to two nodes

		private ConnectionGraphic m_pGraphic;

		// construction
		/// <summary>
		/// Initializes a new connection originating from specified nodule.
		/// </summary>
		/// <param name="pStart">The nodule to start the connection from.</param>
		public Connection(Nodule pStart)
		{
            if (Master.VerboseMode) Master.log("Connection initialized");
			m_pOrigin = pStart;
			if (pStart.IsInput) 
            {
                m_pInNodule = pStart;
                if(pStart.Connections.Count > 0)
                {
                    Connection x = pStart.Connections[0];
                    x.removeConnection();
                }
            }
			else { m_pOutNodule = pStart; }
			m_pGraphic = new ConnectionGraphic(this);
		}

		// properties
		/// <summary>
		/// The first nodule referred to during the creation of this connection. This property should only be used during the creation of the connection.(<see cref="Nodule"/>.)
		/// </summary>
		public Nodule Origin { get { return m_pOrigin; } set { m_pOrigin = value; } }
		/// <summary>
		/// The second nodule referred to during the creation of this connection. This property should only be used during the creation of the connection.
		/// </summary>
		public Nodule End { get { return m_pEnd; } set { m_pEnd = value; } }
		/// <summary>
		/// The terminal nodule for the data connection. (Data flows through connection TO this nodule.)
		/// </summary>
		public Nodule InputNodule { get { return m_pInNodule; } set { m_pInNodule = value; } }
		/// <summary>
		/// The introductory nodule for the data connection. (Data flows through connection FROM this nodule.)
		/// </summary>
		public Nodule OutputNodule { get { return m_pOutNodule; } set { m_pOutNodule = value; } }
		/// <summary>
		/// Represents whether the connection has been created and finalized between the nodules or not.
		/// </summary>
		public bool IsComplete { get { return m_bCompleted; } set { m_bCompleted = value; } }
		/// <summary>
		/// GUI/Visual controller associated with this connection. (<see cref="ConnectionGraphic"/>)
		/// </summary>
		public ConnectionGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		
		// -- FUNCTIONS --

		/// <summary>
		/// Finalizes the creation process of a connection between nodules, and adds a connection reference inside the involved nodules. Returns true on success, false on failure.
		/// </summary>
		/// <param name="pOther">The end nodule (<see cref="Connection.End"/>) to connect to the origin nodule.</param>
		public bool completeConnection(Nodule pOther)
		{
			m_pEnd = pOther;
			if (m_pOutNodule == null) { m_pOutNodule = pOther; }
			else { m_pInNodule = pOther; }

			// make sure the nodes aren't the same and are not both inputs or outputs
			if (m_pOrigin.Equals(m_pEnd) || 
				m_pOrigin.IsInput == m_pEnd.IsInput || 
                m_pOrigin.Parent == m_pEnd.Parent)
			{
				m_pGraphic.removeGraphic();
				return false;
			}

			// set end point to end node center
			m_pGraphic.finishVisualConnection(m_pEnd);

			m_bCompleted = true;

			int iInputBlockID = m_pInNodule.Parent.ID;
			int iInputNodeID = m_pInNodule.GroupNum;
			int iOutputBlockID = m_pOutNodule.Parent.ID;
			int iOutputNodeID = m_pOutNodule.GroupNum;
			if(Master.VerboseMode) Master.log("Connection created - OutputID: " + iOutputBlockID + " (out-node " + iOutputNodeID + ") InputID: " + iInputBlockID + " (in-node " + iInputNodeID + ")");

			return true;
		}

		/// <summary>
		/// Safely remove the connection from all involved nodules and deletes the visual representation.
		/// </summary>
		/// <remarks>
		/// Connection must have already been created and finalized.
		/// </remarks>
        public void removeConnection()
        {
            // remove all the things! (effectively delete connection)
            m_pOrigin.removeConnection(this);
            m_pEnd.removeConnection(this);
			m_pGraphic.removeGraphic();

            if (Master.VerboseMode) Master.log("Connection destroyed");
        }
	}
}
