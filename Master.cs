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
		private static MainWindow s_pWin;
		private static int s_iRepID = -1; // incrementing counter for assigning block ids
		private static List<Block> s_lBlocks = new List<Block>();
		
		// public:
        public const string PATH_TO_THETHING = "../../Algorithms/algorithm_correct";
        public static bool VerboseMode = false;

		// properties
		public static List<Block> Blocks { get { return s_lBlocks; } set { s_lBlocks = value; } }

		// NOTE: this should only be called once in main window constructor
		public static void assignWindow(MainWindow pWindow) { s_pWin = pWindow; }

		public static void log(string sMessage) { s_pWin.log(sMessage); }
		public static void log(string sMessage, Color pColor) { s_pWin.log(sMessage, pColor); }

		public static GraphicContainer getGraphicContainer() { return s_pWin.getGraphicContainer(); }
		public static Canvas getCanvas() { return s_pWin.MainCanvas; } // I know the name for this now!! Delegation!
		public static void setDraggingBlock(bool bDragging, BlockGraphic pDraggingBlock) { s_pWin.getGraphicContainer().setDraggingBlock(bDragging, pDraggingBlock); }
		public static void setDraggingConnection(bool bDragging, ConnectionGraphic pConnectionGraphic) { s_pWin.getGraphicContainer().setDraggingConnection(bDragging, pConnectionGraphic); }
		public static ConnectionGraphic getDraggingConnection() { return s_pWin.getGraphicContainer().getDraggingConnection(); }
		public static void setCommandPrompt(string sText) { s_pWin.setCommandPrompt(sText); }

		public static int getNextRepID() { s_iRepID++; return s_iRepID; }

		public static void removeBlock(int iID) 
		{
			int iIndexToRemove = -1;
			for (int i = 0; i < s_lBlocks.Count; i++)
			{
				if (s_lBlocks[i].ID == iID) { iIndexToRemove = i; break; }
			}

			if (iIndexToRemove == -1) { throw new Exception("Could not find block with ID " + iID); }
			s_lBlocks[iIndexToRemove].deleteBlock();
			s_lBlocks.RemoveAt(iIndexToRemove);
		}

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
