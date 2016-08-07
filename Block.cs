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
        private List<Nodule> m_pNodules = new List<Nodule>();

        private List<string> m_pInputNames;
        private string m_sOutputName;
        private int m_iOutputNum;

        private string m_sName = "unnamed algorithm";
        private string m_sVersion = "##.## XXX";
        //private string m_sAlgorithmName = "No-Op"; //TODO Merge with Python Algorithm IDs
        //private string m_sAlgorithmPath;
		private AlgorithmType m_eFamily = AlgorithmType.Undefined;

 		private BlockGraphic m_pGraphic;

		private PyAlgorithm m_pPyAlgorithm;

		// construction
        public Block(List<string> inputNames, string outputName, string sName,/* string path,*/ AlgorithmType eFamily, Color pColor)
        {
            Master.log("----Creating block----");
            m_iID = Master.getNextRepID();
            Master.log("ID: " + m_iID, Colors.GreenYellow);

            m_sName = sName;
            m_eFamily = eFamily;
            m_pInputNames = inputNames;
            m_sOutputName = outputName;
            //m_sAlgorithmPath = path;
            //m_sAlgorithmName = path.Substring(path.IndexOf("alg_") + "alg_".Length);
            m_iOutputNum = (m_sOutputName == "") ? 0 : 1;
            m_pGraphic = new BlockGraphic(this, m_pInputNames.Count, m_iOutputNum, pColor);
			Master.getGraphicContainer().addBlockGraphic(m_pGraphic);

			// create nodes
            for (int i = 0; i < m_pInputNames.Count; i++) { m_pNodules.Add(new Nodule(this, true, i, m_pInputNames[i])); }
            for (int i = 0; i < m_iOutputNum; i++) { m_pNodules.Add(new Nodule(this, false, i, m_sOutputName)); }
        }

		// properties 
		public int ID { get { return m_iID; } set { m_iID = value; } }
		public string Name { get { return m_sName; } set { m_sName = value; } }
		public string Version { get { return m_sVersion; } set { m_sVersion = value; } } 
		//public string AlgorithmName { get { return m_sAlgorithmName; } set { m_sAlgorithmName = value; } }
        //public string AlgorithmPath { get { return m_sAlgorithmPath; } }
		public AlgorithmType Family { get { return m_eFamily; } set { m_eFamily = value; } }
		public BlockGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		public List<Nodule> Nodules { get { return m_pNodules; } set { m_pNodules = value; } }
		public PyAlgorithm PyAlgorithm { get { return m_pPyAlgorithm; } set { m_pPyAlgorithm = value; } }

		// -- FUNCTIONS --

        public void connectTo(Block pTarget, int iNoduleNum)
        {
            if (m_iOutputNum == 0 || pTarget.m_pInputNames.Count() == 0)
                return;

            foreach(Nodule n in m_pNodules)
            {
                if(!n.IsInput)
                {
					Nodule pNoduleTarget = pTarget.m_pNodules[iNoduleNum];
					if (pNoduleTarget.IsInput && pNoduleTarget.NumConnections == 0)
					{
						Connection c = new Connection(n);
						n.addConnection(c);
						pNoduleTarget.addConnection(c);
						c.InputNodule = pNoduleTarget;
						c.OutputNodule = n;
						c.completeConnection(pNoduleTarget);
						Master.getGraphicContainer().setDraggingConnection(false, null);
					}
                }
            }
        }

		public void deleteBlock()
		{
			for (int i = 0; i < m_pNodules.Count; i++) { m_pNodules[i].deleteNodule(); }
			m_pNodules = null;
			m_pGraphic.deleteGraphic();
		}

        public List<Block> getOutgoing()
        {
            List<Block> pOutgoingBlocks = new List<Block>();

            foreach (Nodule pNodule in m_pNodules)
            {
                if (!pNodule.IsInput)
                {
                    foreach(Connection c in pNodule.Connections)
                    {
                        pOutgoingBlocks.Add(c.InputNodule.Parent);
                    }
                }
            }

            return pOutgoingBlocks;
        }
    }
}
