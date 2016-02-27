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

namespace AlgGui
{
	public class Node
	{
		// TODO: add labels on hover (right click allows you to change?) [for naming groups?]

		// member variables
		private Representation m_parent;
		private NodeGraphic m_graphic;

		private List<Connection> m_connections = new List<Connection>();

		private bool m_isInput = true; // false means this is output node
		private int m_groupNum = 0; // semi-id system, which node of input/output is it? (ex: 2nd input node etc) NOTE: 0-based

        private Datatype m_datatype = null;
        
		// construction
		public Node(Representation parent, bool isInput, int groupNum, Datatype datatype) // PASS IN X AND Y OF REPRESENTATION
		{
			m_parent = parent;
			m_isInput = isInput;
			m_groupNum = groupNum;
			m_datatype = datatype;

			m_graphic = GraphicFactory.createNodeGraphic(this);
		}

		// properties
		public Representation getParent() { return m_parent; }
		public NodeGraphic getGraphic() { return m_graphic; }

		public bool isInput() { return m_isInput; }
		public int getGroupNum() { return m_groupNum; }

		public List<Connection> getConnections() { return m_connections; }
		public void addConnection(Connection c) { m_connections.Add(c); }
		public void removeConnection(Connection c) { m_connections.Remove(c); }
        public int getNumConnections() { return m_connections.Count;  }
        public Connection getConnection(int i) { return m_connections[i]; }

		public Datatype getDatatype() { return m_datatype; }
		
		// -- FUNCTIONS --

		// finishes out the connection
		public void connect(Connection con)
		{
			if (!con.completeConnection(this)) { return; } // need actual deletion code for connection stuff?
			//maybe this function should return true if connection successful, false if not?

			// add connection to both nodes' collection
			m_connections.Add(con);
			con.getOrigin().addConnection(con);
		}
	}
}
