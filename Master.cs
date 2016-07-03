using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nurielite
{
	// Use this for necessary interactions between classes and window
	// (so the window doesn't need to be passed into every class in order to have necessary functionality)
    /// <summary>
    /// Testing Summary
    /// </summary>
	class Master
	{
		// private:
		private static MainWindow win;
		private static int RepID = -1; // incrementing counter for assigning block ids
		private static List<Block> m_pBlocks = new List<Block>();
		
		// public:
        public const string PATH_TO_THETHING = "../../Algorithms/algorithm_correct";
		public static Datatype TEST_DATATYPE = Datatype.defineDatatype("Scalar Integer", 1, null);

		// properties
		public static List<Block> Blocks { get { return m_pBlocks; } set { m_pBlocks = value; } }

		// NOTE: this should only be called once in main window constructor
		public static void assignWindow(MainWindow window) { win = window; }

		public static void log(string message) { win.log(message); }
		public static void log(string message, Color color) { win.log(message, color); }

		public static GraphicContainer getGraphicContainer() { return win.getGraphicContainer(); }
		public static Canvas getCanvas() { return win.MainCanvas; } // I know the name for this now!! Delegation!
		public static void setDraggingBlock(bool dragging, BlockGraphic dragBlk) { win.getGraphicContainer().setDraggingBlock(dragging, dragBlk); }
		public static void setDraggingConnection(bool dragging, ConnectionGraphic con) { win.getGraphicContainer().setDraggingConnection(dragging, con); }
		public static ConnectionGraphic getDraggingConnection() { return win.getGraphicContainer().getDraggingConnection(); }
		public static void setCommandPrompt(string text) { win.setCommandPrompt(text); }

		public static int getNextRepID() { RepID++; return RepID; }

        public static string getAlgorithmTypeName(AlgorithmType eType)
        {
            switch (eType)
            {
                case AlgorithmType.Classifier: { return "Classifier"; }
                case AlgorithmType.Clustering: { return "Clustering"; }
                case AlgorithmType.DimensionReduction: { return "DimensionReduction"; }
                case AlgorithmType.Operation: { return "Operation"; }
                case AlgorithmType.Input: { return "Input"; }
                case AlgorithmType.Output: { return "Output"; }
                case AlgorithmType.Undefined: { return "Undefined"; }
            }
            return "Undefined and also something broke.";
        }

        public static string getAlgorithmTypeNameLowerCase(AlgorithmType eType)
        {
            switch (eType)
            {
                case AlgorithmType.Classifier: { return "classifier"; }
                case AlgorithmType.Clustering: { return "clustering"; }
                case AlgorithmType.DimensionReduction: { return "dimension_reduction"; }
                case AlgorithmType.Operation: { return "operation"; }
                case AlgorithmType.Input: { return "input"; }
                case AlgorithmType.Output: { return "output"; }
                case AlgorithmType.Undefined: { return "undefined"; }
            }
            return "undefined and also something broke.";
        }
	}

	public enum AlgorithmType
	{
		Undefined = -1,
		Operation = 0,
		Classifier = 1,
		Clustering = 2,
        DimensionReduction = 3,
		Input = 4,
		Output = 5
	}
}
