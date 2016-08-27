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
    /// <summary>
    /// Contains the graphical code for the block class. It is responsible for rendering the Block as well as the associated nodules.
    /// </summary>
	public class BlockGraphic
	{
		// info variables
		private int m_iID;
		private string m_sName;
		private string m_sVersion;
		private AlgorithmType m_eAlgorithm;

		private int m_iInputNoduleOffset = 0;
		private int m_iOutputNoduleOffset = 0;

        // Used for panning
		private double m_dRelativeX = 0;
		private double m_dRelativeY = 0;

		// gui elements
		private Canvas m_pCanvasParent = new Canvas();
		private Rectangle m_pRecBody = new Rectangle();
		private Rectangle m_pRecBoard = new Rectangle();
		private Label m_pLblName = new Label();
		private Label m_pLblID = new Label();
		private Label m_pLblContent = new Label();
		private Color m_pColorBase = Colors.SeaGreen;
		private SolidColorBrush m_pBrushBorderStandard = new SolidColorBrush(Colors.Black); // prob better way to do this?
		private SolidColorBrush m_pBrushBorderSelected = new SolidColorBrush(Colors.Orange);
		private SolidColorBrush m_pBrushForeground = new SolidColorBrush(Colors.Black);
		private SolidColorBrush m_pBrushBackground;
		private SolidColorBrush m_pBrushBackgroundLight;
		private Thickness m_pThicknessZero = new Thickness(0); //Is this always zero? Should it be static constant?

		private Block m_pBlkParent;

		private bool m_bIsDragging = false;

		// construction
		// TODO: overloaded part, make this a function that takes a lot more, than constructors just call different ones with some default parameters instead
		/// <summary>
		/// Creates a visual representation of the block.
		/// </summary>
		/// <param name="parent">The block object that this graphic visually represents.</param>
		/// <param name="numIn">Number of input nodules to render.</param>
		/// <param name="numOut">Number of output nodules to render.</param>
		/// <param name="color">Color to render the block as.</param>
		public BlockGraphic(Block parent, int numIn, int numOut, Color color)
		{
			m_pBlkParent = parent;

			m_iID = parent.ID;
			m_sName = parent.Name;
			m_sVersion = parent.Version;
            m_eAlgorithm = parent.Family;
            m_pColorBase = color;

			createDrawing(100, 100, numIn, numOut, m_pColorBase);

			// get starting offset points for nodes
			m_iInputNoduleOffset = (int)((m_pRecBody.Width - (numIn * GraphicContainer.NODULE_SIZE)) / 2);
			m_iOutputNoduleOffset = (int)((m_pRecBody.Width - (numOut * GraphicContainer.NODULE_SIZE)) / 2);
		}

		// -- PROPERTIES --
		/// <summary>
		/// The X coordinate of the top left corner of the block in canvas display.
		/// </summary>
        public double CurrentX { get { return Canvas.GetLeft(m_pCanvasParent); } }
		/// <summary>
		/// The Y coordinate of the top left corner of the block in canvas display.
		/// </summary>
        public double CurrentY { get { return Canvas.GetTop(m_pCanvasParent); } }
		/// <summary>
		/// The X distance form the left side of the block to the mouse cursor. Used during dragging/clicking a block.
		/// </summary>
        public double RelativeX { get { return m_dRelativeX; } set { m_dRelativeX = value; } }
		/// <summary>
		/// The Y distance from the top of the block and the mouse cursor. Used during dragging/clicking a block.
		/// </summary>
        public double RelativeY { get { return m_dRelativeY; } set { m_dRelativeY = value; } }
		/// <summary>
		/// The name that is displayed next to the visual block.
		/// </summary>
        public string Name { get { return m_sName; } set { m_sName = value; m_pLblName.Content = value; } }
        
		/// <summary>
		/// The color that is used as a base for the rendered block.
		/// </summary>
        public Color BaseColor { get { return m_pColorBase; } set { m_pColorBase = value; setBrushes(); applyBrushes(); } }
		/// <summary>
		/// The Block object (<see cref="Block"/>) that this graphic is associated with.
		/// </summary>
        public Block Parent { get { return m_pBlkParent; } }
       

		// -- FUNCTIONS --

		// finds what offset should be for given node
		/// <summary>
		/// Determines what the X offset for the left side of the nodule from the left side of the block should be, based on the nodule index.
		/// </summary>
		/// <param name="isInput">Determines if the nodule is an input to the python algorithm or not.</param>
		/// <param name="iGroupNum">The index of the nodule in the Block's list of nodules.</param>
		public int getNoduleOffsetX(bool isInput, int iGroupNum)
		{
			if (isInput) { return m_iInputNoduleOffset + iGroupNum * GraphicContainer.NODULE_SIZE; }
			else { return m_iOutputNoduleOffset + iGroupNum * GraphicContainer.NODULE_SIZE; }
		}
		/// <summary>
		/// Determines what the Y offset for the top of the nodule from the top of the block should be, based on whether the nodule represents an input or an output.
		/// </summary>
		/// <param name="bIsInput">Determines if the nodule is an input to the python algorithm or not.</param>
		public int getNoduleOffsetY(bool bIsInput)
		{
			if (bIsInput) { return -(GraphicContainer.NODULE_SIZE); }
			else { return (int)m_pRecBody.Height; }
		}

		private void createDrawing(int iX, int iY, int iNumberInputs, int iNumberOutputs, Color pInitialColor)
		{
			int iWidth = calcOptimalWidth(iNumberInputs, iNumberOutputs);

			m_pColorBase = pInitialColor;
			setBrushes();
			applyBrushes();

			// set up parent canvas 
			Canvas.SetZIndex(m_pCanvasParent, GraphicContainer.BLOCK_Z_LEVEL);

			// create body
			m_pRecBody.Height = GraphicContainer.BLOCK_MINIMUM_HEIGHT;
			m_pRecBody.Width = iWidth;
			m_pRecBody.RadiusX = 5;
			m_pRecBody.RadiusY = 5;
			m_pRecBody.Stroke = m_pBrushBorderStandard;
			m_pRecBody.StrokeThickness = 2;
			Canvas.SetZIndex(m_pRecBody, GraphicContainer.BLOCK_Z_LEVEL);

			// board (inner part of body)
			m_pRecBoard.Height = GraphicContainer.BLOCK_MINIMUM_HEIGHT - 30;
			m_pRecBoard.Width = iWidth - 12;
			m_pRecBoard.RadiusX = 3;
			m_pRecBoard.RadiusY = 3;
			m_pRecBoard.IsHitTestVisible = false;
			Canvas.SetZIndex(m_pRecBoard, GraphicContainer.BLOCK_Z_LEVEL);
			Canvas.SetLeft(m_pRecBoard, GraphicContainer.BLOCK_BOARD_PADDING_LEFT);
			Canvas.SetTop(m_pRecBoard, GraphicContainer.BLOCK_BOARD_PADDING_TOP);

			// labels
			m_pLblID.Margin = m_pThicknessZero;
			m_pLblID.Content = m_iID + " " + m_sVersion;
			m_pLblID.Foreground = m_pBrushForeground;
			m_pLblID.IsHitTestVisible = false;
			Canvas.SetZIndex(m_pLblID, GraphicContainer.BLOCK_Z_LEVEL);

			m_pLblName.Margin = m_pThicknessZero;
			m_pLblName.Content = m_sName;
			m_pLblName.Foreground = m_pBrushForeground;
			m_pLblName.Padding = m_pThicknessZero;
			m_pLblName.Margin = m_pThicknessZero;
			m_pLblName.Height = 20;
			Canvas.SetZIndex(m_pLblName, GraphicContainer.BLOCK_Z_LEVEL);
			Canvas.SetLeft(m_pLblName, m_pRecBody.Width + 2);
			Canvas.SetTop(m_pLblName, (m_pRecBody.Height / 2) - (m_pLblName.Height / 2));

			m_pLblContent.Foreground = m_pBrushForeground;
			m_pLblContent.Content = Name + "\n" + "Accuracy";  //Oops
			m_pLblContent.IsHitTestVisible = false;
			m_pLblContent.Width = m_pRecBoard.Width;
			Canvas.SetZIndex(m_pLblContent, GraphicContainer.BLOCK_Z_LEVEL);
			Canvas.SetLeft(m_pLblContent, GraphicContainer.BLOCK_BOARD_PADDING_LEFT);
			Canvas.SetTop(m_pLblContent, GraphicContainer.BLOCK_BOARD_PADDING_TOP);

			// add all the things!
			m_pCanvasParent.Children.Add(m_pRecBody);
			m_pCanvasParent.Children.Add(m_pRecBoard);
			m_pCanvasParent.Children.Add(m_pLblID);
			m_pCanvasParent.Children.Add(m_pLblContent);
			m_pCanvasParent.Children.Add(m_pLblName);

			Master.getCanvas().Children.Add(m_pCanvasParent);
			move(iX, iY);

			m_pRecBody.MouseDown += new MouseButtonEventHandler(evt_MouseDownBody);
			m_pLblName.MouseDown += new MouseButtonEventHandler(evt_MouseDownLabel);
		}

		/// <summary>
		/// Safely removes this graphic from the display canvas.
		/// </summary>
		public void deleteGraphic() { Master.getCanvas().Children.Remove(m_pCanvasParent); }

		/// <summary>
		/// Visually moves block to the specified x and y coordinates, based on the upper left corner of block.
		/// </summary>
		/// <param name="dX">X coordinate.</param>
		/// <param name="dY">Y coordinate.</param>
		public void move(double dX, double dY) 
		{
			Canvas.SetLeft(m_pCanvasParent, dX);
			Canvas.SetTop(m_pCanvasParent, dY);
			
			// move nodes
			foreach (Nodule n in m_pBlkParent.Nodules) { n.Graphic.move(dX, dY); }
		}

		// find least amount of space to fit all nodes
		private int calcOptimalWidth(int iNumberInputs, int iNumberOutputs)
		{
			int iInputsTotalPixelWidth = iNumberInputs * GraphicContainer.NODULE_SIZE;
			int iOutputsTotalPixelWidth = iNumberOutputs * GraphicContainer.NODULE_SIZE;

			int iWidest = iInputsTotalPixelWidth;
			if (iOutputsTotalPixelWidth > iWidest) { iWidest = iOutputsTotalPixelWidth; }

			if (iWidest < GraphicContainer.BLOCK_MINIMUM_WIDTH) { iWidest = GraphicContainer.BLOCK_MINIMUM_WIDTH; } // make it at least 25 pixels wide
			return iWidest;
		}

		// assumes m_baseColor has been assigned appropriately
		// (Use this to avoid unnecessary recalling of functions over and over)
		private void setBrushes()
		{
			m_pBrushBackground = new SolidColorBrush(m_pColorBase);
			m_pBrushBackgroundLight = new SolidColorBrush(lightenColor(m_pColorBase, 0.6f));
		}

		private void applyBrushes()
		{
			m_pRecBody.Fill = m_pBrushBackground;
			m_pRecBoard.Fill = m_pBrushBackgroundLight;
		}

		private Color lightenColor(Color pColor, float fFactor)
		{
			float fRed = (255 - pColor.R) * fFactor + pColor.R;
			float fGreen = (255 - pColor.G) * fFactor + pColor.G;
			float fBlue = (255 - pColor.B) * fFactor + pColor.B;
			return Color.FromArgb(pColor.A, (byte)fRed, (byte)fGreen, (byte)fBlue);
		}

        private void editBlock(Block pBlock)
        {
            BlockEditorWin pPopup = new BlockEditorWin(pBlock);
            pPopup.Show();
        }

		// -- EVENT HANDLERS --

		public void evt_MouseDownBody(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				//Master.log("Block ID " + m_iID + " clicked", Colors.Salmon); Probably don't need this anymore
				m_bIsDragging = true;
				m_pRecBody.Stroke = m_pBrushBorderSelected;

				// get relative coordinates
				Point pMousePoint = e.GetPosition(Master.getCanvas());
				m_dRelativeX = pMousePoint.X - Canvas.GetLeft(m_pCanvasParent);
				m_dRelativeY = pMousePoint.Y - Canvas.GetTop(m_pCanvasParent);

				Master.setDraggingBlock(true, this);
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
                editBlock(m_pBlkParent);
			}
		}

		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bIsDragging)
			{
				Point pMousePoint = e.GetPosition(Master.getCanvas());
				double x = pMousePoint.X - m_dRelativeX;
				double y = pMousePoint.Y - m_dRelativeY;
				move(x, y);
			}
		}

		public void evt_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_bIsDragging && e.LeftButton == MouseButtonState.Released)
			{
				m_bIsDragging = false;
				Master.setDraggingBlock(false, null);

				m_pRecBody.Stroke = m_pBrushBorderStandard;
				m_dRelativeX = 0;
				m_dRelativeY = 0;
			}
		}

		public void evt_MouseDownLabel(object sender, MouseButtonEventArgs e)
		{
			Master.setCommandPrompt("edit rep -" + m_iID + " -lbl -\"");
		}
	}
}
