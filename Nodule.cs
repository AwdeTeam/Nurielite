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
	public class Nodule
	{
		// TODO: add labels on hover (right click allows you to change?) [for naming groups?]

		// member variables
		private Block m_pParent;
		private NoduleGraphic m_pGraphic;

		private List<Connection> m_pConnections = new List<Connection>();

		private bool m_bIsInput = true; // false means this is output node
		private int m_iGroupNum = 0; // semi-id system, which node of input/output is it? (ex: 2nd input node etc) NOTE: 0-based

        private Datatype m_pDataType = null;
        
		// construction
		public Nodule(Block pParent, bool bIsInput, int iGroupNum, Datatype pDatatype) // PASS IN X AND Y OF BLOCK
		{
			m_pParent = pParent;
			m_bIsInput = bIsInput;
			m_iGroupNum = iGroupNum;
			m_pDataType = pDatatype;

			m_pGraphic = new NoduleGraphic(this);
		}

		// properties
		// TODO: find references and update. Also do these really need setters?
		public Block Parent { get { return m_pParent; } set { m_pParent = value; } }
		public NoduleGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		public bool IsInput { get { return m_bIsInput; } set { m_bIsInput = value; } }
		public int GroupNum { get { return m_iGroupNum; } set { m_iGroupNum = value; } }
		public List<Connection> Connections { get { return m_pConnections; } set { m_pConnections = value; } }
		public int NumConnections { get { return m_pConnections.Count; } }
		public Datatype Datatype { get { return m_pDataType; } set { m_pDataType = value; } }
		
		
		
		public Block getParent() { return m_pParent; } 
		public NoduleGraphic getGraphic() { return m_pGraphic; }

		public bool isInput() { return m_bIsInput; }
		public int getGroupNum() { return m_iGroupNum; }

		public List<Connection> getConnections() { return m_pConnections; }
		public void addConnection(Connection c) { m_pConnections.Add(c); }
		public void removeConnection(Connection c) { m_pConnections.Remove(c); }
        public int getNumConnections() { return m_pConnections.Count;  }
        public Connection getConnection(int i) { return m_pConnections[i]; }

		public Datatype getDatatype() { return m_pDataType; }
		
		// -- FUNCTIONS --

		// finishes out the connection
		public void connect(Connection pConnection)
		{
			if (!pConnection.completeConnection(this)) { return; } // need actual deletion code for connection stuff?
			//maybe this function should return true if connection successful, false if not?

			// add connection to both nodes' collection
			m_pConnections.Add(pConnection);
			pConnection.getOrigin().addConnection(pConnection);
		}
	}
}
