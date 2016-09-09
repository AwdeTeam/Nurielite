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
    /// Singleton that serves as a communicator between classes.
    /// </summary>
    /// <seealso cref="MainWindow"/>
	class Master
	{
		// private:
		private static MainWindow s_pWin;
		private static int s_iRepID = -1; // incrementing counter for assigning block ids
		private static List<Block> s_lBlocks = new List<Block>();
		
		// public:

        /// <summary>
        /// Path to the list of algorithms.  Also currently used for compiled output.  We should probably change that.
        /// </summary>
        /// <seealso cref="PythonGenerator.cs"/>
        public const string ALGORITHM_PATH = "../../Algorithms/algorithm_correct";

        /// <summary>
        /// Flag that either permits of denies verbose logging to the console for debug purposes.
        /// </summary>
        public static bool VerboseMode = false;

		// properties:

        /// <summary>
        /// Master list of all <see cref="Block.cs"/> Blocks in the current context.
        /// </summary>
		public static List<Block> Blocks { get { return s_lBlocks; } set { s_lBlocks = value; } }

        /// <summary>
        /// Creates a reference to the passed <see cref="MainWindow"/> MainWindow in the Master to allow for communication from arbitrary classes to the MainWindow.
        /// </summary>
        /// <param name="pWindow">A MainWindow reference.</param>
        /// <remarks>This should only be called once in main window constructor.</remarks>
		public static void assignWindow(MainWindow pWindow) { s_pWin = pWindow; }

        /// <summary>
        /// Prints a String message to the Nurielite console.
        /// </summary>
        /// <param name="sMessage">Message to print.</param>
		public static void log(string sMessage) { s_pWin.log(sMessage); }

        /// <summary>
        /// Prints a colored String message to the Nurielite console.
        /// </summary>
        /// <param name="sMessage">Message to print.</param>
        /// <param name="pColor">Color of the message.</param>
		public static void log(string sMessage, Color pColor) { s_pWin.log(sMessage, pColor); }

        /// <summary>
        /// Gets the <see cref="GraphicContainer"/> Graphic Handler to handle graphical changes and events.
        /// </summary>
        /// <returns>The Graphic Container.</returns>
		public static GraphicContainer getGraphicContainer() { return s_pWin.getGraphicContainer(); }

        /// <summary>
        /// Gets the Canvas for direct drawing.
        /// </summary>
        /// <returns>The main Canvas.</returns>
        /// <seealso cref="MainWindow"/>
		public static Canvas getCanvas() { return s_pWin.MainCanvas; } // I know the name for this now!! Delegation!

        /// <summary>
        /// Toggles whether the passed <see cref="BlockGraphic"/> block graphic is being dragged.
        /// </summary>
        /// <param name="bDragging">.</param>
        /// <param name="pDraggingBlock">Block Graphic in question.</param>
		public static void setDraggingBlock(bool bDragging, BlockGraphic pDraggingBlock) { s_pWin.getGraphicContainer().setDraggingBlock(bDragging, pDraggingBlock); }

        /// <summary>
        /// Toggles whether the passed <see cref="ConnectionGraphic"/> connection graphic is being dragged.
        /// </summary>
        /// <param name="bDragging">Boolean dragging switch.</param>
        /// <param name="pConnectionGraphic">Connection Graphic in question.</param>
		public static void setDraggingConnection(bool bDragging, ConnectionGraphic pConnectionGraphic) { s_pWin.getGraphicContainer().setDraggingConnection(bDragging, pConnectionGraphic); }

        /// <summary>
        /// Gets the currently dragging connection (if any).
        /// </summary>
        /// <returns>A <see cref="ConnectionGraphic"/> Connection Graphic</returns>
		public static ConnectionGraphic getDraggingConnection() { return s_pWin.getGraphicContainer().getDraggingConnection(); }

        //TODO: Nathan, I have no idea what this does.
        /// <summary>
        /// ???  No idea ???
        /// </summary>
        /// <param name="sText"></param>
		public static void setCommandPrompt(string sText) { s_pWin.setCommandPrompt(sText); }

        /// <summary>
        /// Gets the next valid unused id for a <see cref="Block"/>block.
        /// </summary>
        /// <returns>Integer ID</returns>
		public static int getNextRepID() { s_iRepID++; return s_iRepID; }

        /// <summary>
        /// Removes the block with the given ID from the system.
        /// </summary>
        /// <param name="iID"></param>
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
	}

    /// <summary>
    /// The general class of the <see cref="PyAlgorithm"/> algorithm.
    /// </summary>
	public enum AlgorithmType
	{
        /// <summary>
        /// A generic type to handle undefined or unspecified algorithms.
        /// </summary>
		Undefined = -1,

        /// <summary>
        /// An algorithm that is static (cannot be trained or cloned) and is generally designed to help with data flow management.
        /// </summary>
		Operation = 0,

        /// <summary>
        /// An algorithm that sorts a given point in n-space into k categories.
        /// </summary>
		Classifier = 1,

        /// <summary>
        /// An algorithm that finds groups of clusters of points in n-space.
        /// </summary>
		Clustering = 2,

        /// <summary>
        /// An algorithm that finds simplifications of data to move points from k-space to j-space where j << k.
        /// </summary>
        DimensionReduction = 3,

        /// <summary>
        /// General input.
        /// </summary>
        /// <remarks>Merge with Outputs for I/O type?</remarks>
		Input = 4,

        /// <summary>
        /// General output.
        /// </summary>
        /// <remarks>Merge with Inputs for I/O type?</remarks>
		Output = 5
	}
}
