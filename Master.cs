﻿using System;
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
		
		// public:

		// NOTE: this should only be called once in main window constructor
		public static void assignWindow(MainWindow window) { win = window; }

		public static void log(string message) { win.log(message); }
		public static void log(string message, Color color) { win.log(message, color); }

		public static GraphicContainer getGraphicContainer() { return win.getGraphicContainer(); }
		public static Canvas getCanvas() { return win.getMainCanvas(); } // I know the name for this now!! Delegation!
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
	}

	public enum AlgorithmType
	{
		Undefined = 0,
		Operation = 1,
		Classifier = 2,
		Clustering = 3,
        DimensionReduction = 4,
		Input = 5,
		Output = 6
	}
}
