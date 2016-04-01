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
	public class Representation
	{
        public static String[] ALGORITHM_TYPES = { "operation", "classifier", "clustering", "dimension_reduction", "input", "output"};

		// member variables
		private int m_id = 0;
        private List<Node> m_nodes = new List<Node>();

		// TODO: make lists
        private Datatype[] m_inputs;
        private Datatype[] m_outputs;

        private string m_name = "unnamed algorithm";
        private string m_version = "##.## XXX";
        private string m_algorithm = "No-Op"; //TODO Merge with Python Algorithm IDs
		private String m_family = "undefined";

 		private RepresentationGraphic m_graphic;

		// construction
        public Representation(Datatype[] inputs, Datatype[] outputs, String name, String family, Color color)
        {
            Master.log("----Creating representation----");
            m_id = Master.getNextRepID();
            Master.log("ID: " + m_id, Colors.GreenYellow);

            m_name = name;
            m_family = family;
          
            this.m_inputs = inputs;
            this.m_outputs = outputs;
          
			m_graphic = GraphicFactory.createRepresentationGraphic(this, inputs.Length, outputs.Length, color);

			// create nodes
			for (int i = 0; i < m_inputs.Length; i++) { m_nodes.Add(new Node(this, true, i, m_inputs[i])); }
			for (int i = 0; i < m_outputs.Length; i++) { m_nodes.Add(new Node(this, false, i, m_outputs[i])); }
        }

        public static int FindType(String type)
        {
            int i = -1;
            
            for(i = 0; i < ALGORITHM_TYPES.Length; i++)
                if(ALGORITHM_TYPES[i].Equals(type))
                    return i;
            return i;
        }

		// properties
		public int getID() { return m_id; }
		public string getName() { return m_name; }
		public void setName(string name) { m_name = name; m_graphic.setName(m_name); }
		public string getVersion() { return m_version; }
		public string getAlgorithm() { return m_algorithm; }
        public String getFamily() { return m_family; }
        public void setFamily(String family) { m_family = family; }

		public RepresentationGraphic getGraphic() { return m_graphic; }
		public List<Node> getNodes() { return m_nodes; }


		// -- FUNCTIONS --
	}
}
