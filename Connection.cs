using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlgGui
{
	public class Connection
	{
		// member variables
		private Node m_inNode, m_outNode; // same as below, but referenced for different reasons

		private Node m_origin;
		private Node m_end;

		private bool m_completed = false; // if connection has been created/assigned to two nodes

		private ConnectionGraphic m_graphic;

		// construction
		public Connection(Node start)
		{
			Master.log("Connection initialized");
			m_origin = start;
			if (start.isInput()) 
            {
                m_inNode = start;
                if(start.getNumConnections() > 0)
                {
                    Connection x = start.getConnection(0);
                    x.removeConnection();
                }
            }
			else { m_outNode = start; }
			m_graphic = GraphicFactory.createConnectionGraphic(this);
		}

		// properties
		public Node getOrigin() { return m_origin; }
		public Node getEnd() { return m_end; }

		public bool isComplete() { return m_completed; }

		public Node getInputNode() { return m_inNode; }
		public Node getOutputNode() { return m_outNode; }

		public ConnectionGraphic getGraphic() { return m_graphic; }

		// -- FUNCTIONS --

		// finishes creating connection/adds connection to both involved nodes
		// returns true on success, false on failure
		public bool completeConnection(Node other)
		{
			m_end = other;
			if (m_outNode == null) { m_outNode = other; }
			else { m_inNode = other; }

			// make sure the nodes aren't the same and are not both inputs or outputs
			if (m_origin.Equals(m_end) || 
				m_origin.isInput() == m_end.isInput() || 
                m_origin.getParent() == m_end.getParent() || 
                !m_origin.getDatatype().fits(m_end.getDatatype()))
			{
				m_graphic.removeGraphic();
				return false;
			}

            if(!m_origin.getDatatype().equals(m_end.getDatatype()))
            {
				m_graphic.setStrokeColor(Colors.Red);
            }

			// set end point to end node center
			m_graphic.finishVisualConnection(m_end);

			m_completed = true;

			int inputRepID = m_inNode.getParent().getID();
			int inputNodeID = m_inNode.getGroupNum();
			int outputRepID = m_outNode.getParent().getID();
			int outputNodeID = m_outNode.getGroupNum();
			Master.log("Connection created - OutputID: " + outputRepID + " (out-node " + outputNodeID + ") InputID: " + inputRepID + " (in-node " + inputNodeID + ")");

			return true;
		}

		// connection must already have been created
        public void removeConnection()
        {
            // remove all the things! (effectively delete connection)
            m_origin.removeConnection(this);
            m_end.removeConnection(this);
			m_graphic.removeGraphic();

            Master.log("Connection destroyed");
        }
	}
}
