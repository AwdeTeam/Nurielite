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
	public class Connection
	{
		// member variables
		private Nodule m_pInNodule, m_pOutNodule; // same as below, but referenced for different reasons

		private Nodule m_pOrigin;
		private Nodule m_pEnd;

		private bool m_bCompleted = false; // if connection has been created/assigned to two nodes

		private ConnectionGraphic m_pGraphic;

		// construction
		public Connection(Nodule pStart)
		{
			Master.log("Connection initialized");
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
		public Nodule Origin { get { return m_pOrigin; } set { m_pOrigin = value; } }
		public Nodule End { get { return m_pEnd; } set { m_pEnd = value; } }
		public Nodule InputNodule { get { return m_pInNodule; } set { m_pInNodule = value; } }
		public Nodule OutputNodule { get { return m_pOutNodule; } set { m_pOutNodule = value; } }
		public bool IsComplete { get { return m_bCompleted; } set { m_bCompleted = value; } }
		public ConnectionGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		
		// -- FUNCTIONS --

		// finishes creating connection/adds connection to both involved nodes
		// returns true on success, false on failure
		public bool completeConnection(Nodule pOther)
		{
			m_pEnd = pOther;
			if (m_pOutNodule == null) { m_pOutNodule = pOther; }
			else { m_pInNodule = pOther; }

			// make sure the nodes aren't the same and are not both inputs or outputs
			if (m_pOrigin.Equals(m_pEnd) || 
				m_pOrigin.IsInput == m_pEnd.IsInput || 
                m_pOrigin.Parent == m_pEnd.Parent || 
                !m_pOrigin.Datatype.fits(m_pEnd.Datatype))
			{
				m_pGraphic.removeGraphic();
				return false;
			}

            if(!m_pOrigin.Datatype.equals(m_pEnd.Datatype))
            {
				m_pGraphic.setStrokeColor(Colors.Red);
            }

			// set end point to end node center
			m_pGraphic.finishVisualConnection(m_pEnd);

			m_bCompleted = true;

			int iInputBlockID = m_pInNodule.Parent.ID;
			int iInputNodeID = m_pInNodule.GroupNum;
			int iOutputBlockID = m_pOutNodule.Parent.ID;
			int iOutputNodeID = m_pOutNodule.GroupNum;
			Master.log("Connection created - OutputID: " + iOutputBlockID + " (out-node " + iOutputNodeID + ") InputID: " + iInputBlockID + " (in-node " + iInputNodeID + ")");

			return true;
		}

		// connection must already have been created
        public void removeConnection()
        {
            // remove all the things! (effectively delete connection)
            m_pOrigin.removeConnection(this);
            m_pEnd.removeConnection(this);
			m_pGraphic.removeGraphic();

            Master.log("Connection destroyed");
        }
	}
}
