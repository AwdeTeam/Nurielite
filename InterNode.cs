﻿using System;
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
        private Block m_core;
        private InterGraph m_master;

        public InterNode(Block core, InterGraph master)
        {
            if (!master.contains(core.getID()))
            {
                m_inNodes = new LinkedList<InterNode>();
                m_outNodes = new LinkedList<InterNode>();
                m_core = core;
                m_master = master;

                master.append(this);

                updateLinks();
            }
        }

        private void updateLinks()
        {
            List<Block> lrep = m_core.getOutgoing();

            foreach(Block r in lrep)
            {
                if (m_master.contains(r.getID()))
                {
                    Master.log("InterNode with rep id " + m_core.getID() + " is connecting to already extant InterNode " + r.getID());
                    connectTo(m_master.get(r));
                }
                else
                {
                    InterNode inode = new InterNode(r, m_master);
                    Master.log("InterNode with rep id " + m_core.getID() + " is connecting to new InterNode " + r.getID());
                    connectTo(inode);
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
        }

        public int inDegree() { return m_inNodes.Count; }
        public int outDegree() { return m_outNodes.Count; }

        public List<InterNode> getOutgoing() { return m_outNodes.ToList(); }
        public List<InterNode> getIncoming() { return m_inNodes.ToList(); }

        public Block getCore() { return m_core; }

        public bool Equals(InterNode n) { return n.getCore().Equals(m_core); }

        public PyAlgorithm getAlgorithm()
        {
			PythonGenerator pgen = new PythonGenerator();
			PyAlgorithm py = pgen.loadPythonAlgorithm("../../AlgTest/FileOutput.py");
			Dictionary<string, dynamic> options = new Dictionary<string, dynamic>();
			options.Add("thing", m_core);
			py.setOptions(options);

			return py;
        }
    }
}
