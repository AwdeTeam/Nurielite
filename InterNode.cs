using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
    class InterNode
    {
        private LinkedList<InterNode> m_pInNodes;
        private LinkedList<InterNode> m_pOutNodes;
        private List<int> m_pDependancies;
        private Block m_pCore;
        private InterGraph m_pMaster;

        public InterNode(Block core, InterGraph master)
        {
            if (!master.contains(core.ID))
            {
                m_pInNodes = new LinkedList<InterNode>();
                m_pOutNodes = new LinkedList<InterNode>();
                m_pDependancies = new List<int>();
                m_pCore = core;
                m_pMaster = master;

                master.append(this);

                updateLinks();
            }
        }

        private void updateLinks()
        {
            List<Block> lrep = m_pCore.getOutgoing();

            foreach(Block r in lrep)
            {
                if (m_pMaster.contains(r.ID))
                {
                    Master.log("InterNode with rep id " + m_pCore.ID + " is connecting to already extant InterNode " + r.ID);
                    connectTo(m_pMaster.get(r));
                }
                else
                {
                    InterNode inode = new InterNode(r, m_pMaster);
                    Master.log("InterNode with rep id " + m_pCore.ID + " is connecting to new InterNode " + r.ID);
                    connectTo(inode);
                }
            }

        }

        public void connectTo(InterNode node)
        {
            node.m_pInNodes.AddLast(this);
            node.m_pDependancies.Add(this.m_pCore.ID);
            m_pOutNodes.AddLast(node);
        }

        public void disconnect(InterNode node)
        {
			if (this.m_pInNodes.Contains(node)) { m_pInNodes.Remove(node); }
			if (this.m_pOutNodes.Contains(node)) { m_pOutNodes.Remove(node); }
        }

        public int inDegree() { return m_pInNodes.Count; }
        public int outDegree() { return m_pOutNodes.Count; }

        public List<InterNode> getOutgoing() { return m_pOutNodes.ToList(); }
        public List<InterNode> getIncoming() { return m_pInNodes.ToList(); }

        public Block getCore() { return m_pCore; }

        public bool Equals(InterNode n) { return n.getCore().Equals(m_pCore); }

        public List<int> Dependancies { get { return m_pDependancies; } }

        public PyAlgorithm getAlgorithm()
        {
			/*PythonGenerator pgen = new PythonGenerator();
			PyAlgorithm py = pgen.loadPythonAlgorithm(m_core.AlgorithmPath, m_core.AlgorithmName + ".py");
			Dictionary<string, dynamic> options = new Dictionary<string, dynamic>();
			options.Add("thing", m_core);
			py.setOptions(options);*/

			//return py;
			return m_pCore.PyAlgorithm; //TODO: CHANGE!!@!!
        }
    }
}
