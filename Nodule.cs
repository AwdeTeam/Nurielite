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

		private List<Connection> m_lConnections = new List<Connection>();

		private bool m_bIsInput = true; // false means this is output node
		private int m_iGroupNum = 0; // semi-id system, which node of input/output is it? (ex: 2nd input node etc) NOTE: 0-based
        private string m_sName = "unnamed datatype";
        
		// construction
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
		public Block Parent { get { return m_pParent; } set { m_pParent = value; } }
		public NoduleGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		public bool IsInput { get { return m_bIsInput; } set { m_bIsInput = value; } }
		public int GroupNum { get { return m_iGroupNum; } set { m_iGroupNum = value; } }
		public List<Connection> Connections { get { return m_lConnections; } set { m_lConnections = value; } }
		public int NumConnections { get { return m_lConnections.Count; } }
        public string Name { get { return m_sName; } set { m_sName = value; } }
		
		
		// -- FUNCTIONS --
		public void addConnection(Connection pConnection) { m_lConnections.Add(pConnection); }
		public void removeConnection(Connection pConnection) { m_lConnections.Remove(pConnection); }

		// finishes out the connection
		public void connect(Connection pConnection)
		{
			if (!pConnection.completeConnection(this)) { return; } // need actual deletion code for connection stuff?
			//maybe this function should return true if connection successful, false if not?

			// add connection to both nodes' collection
			m_lConnections.Add(pConnection);
			pConnection.Origin.addConnection(pConnection);
		}

		public void deleteNodule()
		{
			for (int i = m_lConnections.Count - 1; i >= 0; i--) { m_lConnections[i].removeConnection(); }
			m_lConnections = null;
			m_pGraphic.deleteGraphic();
		}
	}
}
