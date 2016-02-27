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

namespace AlgGui
{
	public class Representation
	{
        public enum AlgorithmFamily { Classifier, Clustering, DimensionReduction, Operation };


		// member variables
		private int m_id = 0;
        private List<Node> m_nodes = new List<Node>();

		// TODO: make lists
        private Datatype[] m_inputs;
        private Datatype[] m_outputs;

        private string m_name = "unnamed algorithm";
        private string m_version = "##.## XXX";
        private string m_algorithm = "No-Op"; //TODO Merge with Python Algorithm IDs
		private AlgorithmFamily m_family = AlgorithmFamily.Operation;

 		private RepresentationGraphic m_graphic;

		// construction
        public Representation(Datatype[] inputs, Datatype[] outputs)
        {
            Master.log("----Creating representation----");
            m_id = Master.getNextRepID();
            Master.log("ID: " + m_id, Colors.GreenYellow);
          
            this.m_inputs = inputs;
            this.m_outputs = outputs;
          
			m_graphic = GraphicFactory.createRepresentationGraphic(this, inputs.Length, outputs.Length);

			// create nodes
			for (int i = 0; i < m_inputs.Length; i++) { m_nodes.Add(new Node(this, true, i, m_inputs[i])); }
			for (int i = 0; i < m_outputs.Length; i++) { m_nodes.Add(new Node(this, false, i, m_outputs[i])); }
        }

		// properties
		public int getID() { return m_id; }
		public string getName() { return m_name; }
		public void setName(string name) { m_name = name; m_graphic.setName(m_name); }
		public string getVersion() { return m_version; }
		public string getAlgorithm() { return m_algorithm; }

		public RepresentationGraphic getGraphic() { return m_graphic; }
		public List<Node> getNodes() { return m_nodes; }


		// -- FUNCTIONS --
	}
}
