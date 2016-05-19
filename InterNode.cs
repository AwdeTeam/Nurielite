using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nurielite
{
    class InterNode
    {
        private LinkedList<InterNode> m_inNodes;
        private LinkedList<InterNode> m_outNodes;
        private Representation m_core;
        private InterGraph m_master;

        public InterNode(Representation core, InterGraph master)
        {
            if (!master.contains(this))
            {
                m_inNodes = new LinkedList<InterNode>();
                m_outNodes = new LinkedList<InterNode>();
                m_core = core;
                m_master = master;

                master.append(this);

                foreach (Node n in m_core.getNodes()) //Caution! Might be unstable!
                {
                    if (n.isInput())
                    {
                        foreach (Connection c in n.getConnections())
                        {
                            Representation rep = c.getOutputNode().getParent();
                            InterNode inode = m_master.get(rep);

                            if (inode != null)
                            {
                                m_inNodes.AddLast(inode);
                            }
                            else
                            {
                                inode = new InterNode(rep, m_master);
                                m_inNodes.AddLast(inode);
                                m_master.append(inode);
                            }
                        }
                    }
                    else
                    {
                        foreach (Connection c in n.getConnections())
                        {
                            Representation rep = c.getInputNode().getParent();
                            InterNode inode = m_master.get(rep);

                            if (inode != null)
                            {
                                m_outNodes.AddLast(inode);
                            }
                            else
                            {
                                inode = new InterNode(rep, m_master);
                                m_outNodes.AddLast(inode);
                                m_master.append(inode);
                            }
                        }
                    }
                }

            }
        }

        public void connectTo(InterNode node)
        {
            node.m_inNodes.AddLast(this);
            m_outNodes.AddLast(node);
        }

        public void disconnect(InterNode node)
        {
			if (this.m_inNodes.Contains(node)) { m_inNodes.Remove(node); }
			if (this.m_outNodes.Contains(node)) { m_outNodes.Remove(node); }
			
            /*foreach(InterNode n in m_inNodes)
            {
                if(node.Equals(n))
                {
                    foreach(InterNode m in node.m_outNodes)
                    {
                        if(m.Equals(this))
                        {
                            m_inNodes.Remove(n);
                            node.m_outNodes.Remove(m);
                        }
                    }
                }
            }
			

            foreach (InterNode n in m_outNodes)
            {
                if (node.Equals(n))
                {
                    foreach (InterNode m in node.m_inNodes)
                    {
                        if (m.Equals(this))
                        {
                            m_outNodes.Remove(n);
                            node.m_inNodes.Remove(m);
                        }
                    }
                }
            }
			*/
        }

        public int inDegree() { return m_inNodes.Count; }
        public int outDegree() { return m_outNodes.Count; }

        public List<InterNode> getOutgoing() { return m_outNodes.ToList(); }
        public List<InterNode> getIncoming() { return m_inNodes.ToList(); }

        public Representation getCore() { return m_core; }

        public bool Equals(InterNode n) { return n.getCore().Equals(m_core); }

        public PyAlgorithm getAlgorithm()
        {
			PythonGenerator pgen = new PythonGenerator();
			PyAlgorithm py = pgen.loadPythonAlgorithm("C:/dwl/lab/AwdeMachineLearning/Nurielite/AlgTest/FileOutput.py");
			Dictionary<string, dynamic> options = new Dictionary<string, dynamic>();
			options.Add("thing", m_core);
			py.setOptions(options);

			return py;
        }
    }
}
