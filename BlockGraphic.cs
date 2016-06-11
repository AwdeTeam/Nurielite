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
    /// Block Graphic contains the graphical code for the Block class.  
    /// It is responsible for rendering the Block as well as the associated Nodules.
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

        /// <summary>
        /// Used for panning
        /// </summary>
		private double m_dRelativeX = 0;
        /// <summary>
        /// Used for panning
        /// </summary>
		private double m_dRelativeY = 0;
		
		// gui elements
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
		public BlockGraphic(Block parent, int numIn, int numOut, Color color)
		{
			m_pBlkParent = parent;

			m_iID = parent.getID();
			m_sName = parent.getName();
			m_sVersion = parent.getVersion();
			m_eAlgorithm = parent.getFamily();
            m_pColorBase = color;

			createDrawing(100, 100, numIn, numOut, m_pColorBase);

			// get starting offset points for nodes
			m_iInputNoduleOffset = (int)((m_pRecBody.Width - (numIn * GraphicContainer.NODE_SIZE)) / 2);
			m_iOutputNoduleOffset = (int)((m_pRecBody.Width - (numOut * GraphicContainer.NODE_SIZE)) / 2);
		}

		// -- PROPERTIES --
        public double CurrentX { get { return Canvas.GetLeft(m_pRecBody); } }
        public double CurrentY { get { return Canvas.GetTop(m_pRecBody); } }
        public double RelativeX { get { return m_dRelativeX; } set { m_dRelativeX = value; } }
        public double RelativeY { get { return m_dRelativeY; } set { m_dRelativeY = value; } }
        public string Name { get { return m_sName; } set { m_sName = value; m_pLblName.Content = value; } }
        
        public Color BaseColor { get { return m_pColorBase; } set { m_pColorBase = value; setBrushes(); applyBrushes(); } }
        public Block Parent { get { return m_pBlkParent; } }
       
        public void setDragging(bool dragging) { m_bIsDragging = dragging; }

		// -- FUNCTIONS --

		// finds what offset should be for given node
		public int getNodeOffsetX(bool isInput, int groupNum)
		{
			if (isInput) { return m_iInputNoduleOffset + groupNum * GraphicContainer.NODE_SIZE; }
			else { return m_iOutputNoduleOffset + groupNum * GraphicContainer.NODE_SIZE; }
		}
		public int getNodeOffsetY(bool isInput)
		{
			if (isInput) { return -(GraphicContainer.NODE_SIZE); }
			else { return (int)m_pRecBody.Height; }
		}

		private void createDrawing(int x, int y, int numIn, int numOut, Color initialColor)
		{
			int width = calcOptimalWidth(numIn, numOut);

			m_pColorBase = initialColor;
			setBrushes();
			applyBrushes();

			// create body
			m_pRecBody.Height = GraphicContainer.REP_MINIMUM_HEIGHT;
			m_pRecBody.Width = width;
			m_pRecBody.RadiusX = 5;
			m_pRecBody.RadiusY = 5;
			m_pRecBody.Stroke = m_pBrushBorderStandard;
			m_pRecBody.StrokeThickness = 2;
			Canvas.SetZIndex(m_pRecBody, GraphicContainer.REP_Z_LEVEL);

			// board (inner part of body)
			m_pRecBoard.Height = GraphicContainer.REP_MINIMUM_HEIGHT - 30;
			m_pRecBoard.Width = width - 12;
			m_pRecBoard.RadiusX = 3;
			m_pRecBoard.RadiusY = 3;
			m_pRecBoard.IsHitTestVisible = false;
			Canvas.SetZIndex(m_pRecBoard, GraphicContainer.REP_Z_LEVEL);

			// labels
			m_pLblID.Margin = m_pThicknessZero;
			m_pLblID.Content = m_iID + " " + m_sVersion;
			m_pLblID.Foreground = m_pBrushForeground;
			m_pLblID.IsHitTestVisible = false;
			Canvas.SetZIndex(m_pLblID, GraphicContainer.REP_Z_LEVEL);

			m_pLblName.Margin = m_pThicknessZero;
			m_pLblName.Content = m_sName;
			m_pLblName.Foreground = m_pBrushForeground;
			m_pLblName.Padding = m_pThicknessZero;
			m_pLblName.Margin = m_pThicknessZero;
			m_pLblName.Height = 20;
			Canvas.SetZIndex(m_pLblName, GraphicContainer.REP_Z_LEVEL);

			m_pLblContent.Foreground = m_pBrushForeground;
			m_pLblContent.Content = Master.getAlgorithmTypeName(m_eAlgorithm) + "\n" + "Accuracy";  //Oops
			m_pLblContent.IsHitTestVisible = false;
			m_pLblContent.Width = m_pRecBoard.Width;
			Canvas.SetZIndex(m_pLblContent, GraphicContainer.REP_Z_LEVEL);

			move(x, y);

			// TODO: mousedown events will be handled in here?

			// ADD ALL THE THINGS!!
			Canvas cnvs = Master.getCanvas();
			cnvs.Children.Add(m_pRecBody);
			cnvs.Children.Add(m_pRecBoard);
			cnvs.Children.Add(m_pLblID);
			cnvs.Children.Add(m_pLblContent);
			cnvs.Children.Add(m_pLblName);

			m_pRecBody.MouseDown += new MouseButtonEventHandler(evt_MouseDown);
			m_pLblName.MouseDown += new MouseButtonEventHandler(name_MouseDown);
		}

		// moves entire block to passed x and y (based on upper left corner)
		public void move(double x, double y) 
		{
			Canvas.SetLeft(m_pRecBody, x);
			Canvas.SetTop(m_pRecBody, y);

			Canvas.SetLeft(m_pRecBoard, x + GraphicContainer.REP_BOARD_PADDING_LEFT);
			Canvas.SetTop(m_pRecBoard, y + GraphicContainer.REP_BOARD_PADDING_TOP);

			Canvas.SetLeft(m_pLblID, x);
			Canvas.SetTop(m_pLblID, y);

			Canvas.SetLeft(m_pLblName, x + m_pRecBody.Width + 2);
			Canvas.SetTop(m_pLblName, y + (m_pRecBody.Height / 2) - (m_pLblName.Height / 2));

			Canvas.SetLeft(m_pLblContent, x + GraphicContainer.REP_BOARD_PADDING_LEFT);
			Canvas.SetTop(m_pLblContent, y + GraphicContainer.REP_BOARD_PADDING_TOP);

			// move nodes
			foreach (Nodule n in m_pBlkParent.getNodes()) { n.getGraphic().move(x, y); }
		}

		// find least amount of space to fit all nodes
		private int calcOptimalWidth(int numIn, int numOut)
		{
			int totalXIn = numIn * GraphicContainer.NODE_SIZE;
			int totalXOut = numOut * GraphicContainer.NODE_SIZE;

			int widest = totalXIn;
			if (totalXOut > widest) { widest = totalXOut; }

			if (widest < GraphicContainer.REP_MINIMUM_WIDTH) { widest = GraphicContainer.REP_MINIMUM_WIDTH; } // make it at least 25 pixels wide
			return widest;
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

		private Color lightenColor(Color color, float p)
		{
			float red = (255 - color.R) * p + color.R;
			float green = (255 - color.G) * p + color.G;
			float blue = (255 - color.B) * p + color.B;
			return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
		}

		// -- EVENT HANDLERS --

		public void evt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Master.log("Block ID " + m_iID + " clicked", Colors.Salmon);
				m_bIsDragging = true;
				m_pRecBody.Stroke = m_pBrushBorderSelected;

				// get relative coordinates
				Point p = e.GetPosition(Master.getCanvas());
				m_dRelativeX = p.X - Canvas.GetLeft(m_pRecBody);
				m_dRelativeY = p.Y - Canvas.GetTop(m_pRecBody);

				Master.setDraggingBlock(true, this);
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
				Master.setCommandPrompt("edit rep -" + m_iID + " -");
			}
		}

		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bIsDragging)
			{
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X - m_dRelativeX;
				double y = p.Y - m_dRelativeY;
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

		public void name_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Master.setCommandPrompt("edit rep -" + m_iID + " -lbl -\"");
		}
	}
}
