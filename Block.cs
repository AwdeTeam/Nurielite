﻿using System;
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
        /// <summary>
        /// 
        /// </summary>
        public static string[] ALGORITHM_TYPES = { "operation", "classifier", "clustering", "dimension_reduction", "input", "output"};

		// member variables
		private int m_iID = 0;
        private List<Nodule> m_pNodules = new List<Nodule>();

        private Datatype[] m_aInputs;
        private Datatype[] m_aOutputs;

        private string m_sName = "unnamed algorithm";
        private string m_sVersion = "##.## XXX";
        private string m_sAlgorithmName = "No-Op"; //TODO Merge with Python Algorithm IDs
		private string m_sFamily = "undefined";
		private AlgorithmType m_eFamily = AlgorithmType.Undefined;

 		private BlockGraphic m_pGraphic;

		// construction
        public Block(Datatype[] aInputs, Datatype[] aOutputs, string sName, string sFamily, Color pColor)
        {
            Master.log("----Creating block----");
            m_iID = Master.getNextRepID();
            Master.log("ID: " + m_iID, Colors.GreenYellow);

            m_sName = sName;
            m_sFamily = sFamily;
          
            this.m_aInputs = aInputs;
            this.m_aOutputs = aOutputs;

			m_pGraphic = new BlockGraphic(this, aInputs.Length, aOutputs.Length, pColor);
			Master.getGraphicContainer().addBlockGraphic(m_pGraphic);

			// create nodes
			for (int i = 0; i < m_aInputs.Length; i++) { m_pNodules.Add(new Nodule(this, true, i, m_aInputs[i])); }
			for (int i = 0; i < m_aOutputs.Length; i++) { m_pNodules.Add(new Nodule(this, false, i, m_aOutputs[i])); }
        }

		// properties 
		// TODO: go through oririnal getters/setters and find references, and update them to new properties
		public int ID { get { return m_iID; } set { m_iID = value; } }
		public string Name { get { return m_sName; } set { m_sName = value; } }
		public string Version { get { return m_sVersion; } set { m_sVersion = value; } } 
		public string AlgorithmName { get { return m_sAlgorithmName; } set { m_sAlgorithmName = value; } }
		public AlgorithmType Family { get { return m_eFamily; } set { m_eFamily = value; } }
		public BlockGraphic Graphic { get { return m_pGraphic; } set { m_pGraphic = value; } }
		public List<Nodule> Nodules { get { return m_pNodules; } set { m_pNodules = value; } }
		
		public int getID() { return m_iID; }
		public string getName() { return m_sName; }
		public void setName(string name) { m_sName = name; m_pGraphic.Name = m_sName; }
		public string getVersion() { return m_sVersion; }
		public string getAlgorithm() { return m_sAlgorithmName; }
        public String getFamily() { return m_sFamily; }
        public void setFamily(String family) { m_sFamily = family; }

		public BlockGraphic getGraphic() { return m_pGraphic; }
		public List<Nodule> getNodes() { return m_pNodules; }


		// -- FUNCTIONS --

        public List<Block> getOutgoing()
        {
            List<Block> lout = new List<Block>();

            foreach (Nodule pNodule in m_pNodules)
            {
                if (!pNodule.isInput())
                {
                    foreach(Connection c in pNodule.getConnections())
                    {
                        lout.Add(c.getInputNode().getParent());
                    }
                }
            }

            return lout;
        }
    }
}
