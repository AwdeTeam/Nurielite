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
	public class RepresentationGraphic
	{
		// info variables
		private int m_id;
		private string m_name;
		private string m_version;
		private string m_algorithm; //TODO: merge with python algorithm

		private int m_inOffStartPoint = 0;
		private int m_outOffStartPoint = 0;

		private double m_relativeX = 0; // used for panning
		private double m_relativeY = 0;
		
		// gui elements
		private Rectangle m_body = new Rectangle();
		private Rectangle m_board = new Rectangle();
		private Label m_lblName = new Label();
		private Label m_lblID = new Label();
		//private TextBox m_txtContent = new TextBox(); // TODO: why textbox? Does user need to edit?
		private Label m_lblContent = new Label();
		private Color m_baseColor = Colors.SeaGreen;
		private SolidColorBrush m_brushBorder = new SolidColorBrush(Colors.Black); // prob better way to do this?
		private SolidColorBrush m_brushBorderSelected = new SolidColorBrush(Colors.Orange);
		private SolidColorBrush m_brushForeground = new SolidColorBrush(Colors.Black);
		private SolidColorBrush m_brushBase;
		private SolidColorBrush m_brushLightenedBase;
		private Thickness m_noThickness = new Thickness(0);

		private Representation m_parent;

		private bool m_isDragging = false;

		// construction
		// TODO: overloaded part, make this a function that takes a lot more, than constructors just call different ones with some default parameters instead
		public RepresentationGraphic(Representation parent, int numIn, int numOut)
		{
			m_parent = parent;

			m_id = parent.getID();
			m_name = parent.getName();
			m_version = parent.getVersion();
			m_algorithm = parent.getAlgorithm();

			createDrawing(100, 100, numIn, numOut, m_baseColor);

			// get starting offset points for nodes
			m_inOffStartPoint = (int)((m_body.Width - (numIn * GraphicContainer.NODE_SIZE)) / 2);
			m_outOffStartPoint = (int)((m_body.Width - (numOut * GraphicContainer.NODE_SIZE)) / 2);
		}

		// -- PROPERTIES --
		public double getCurrentX() { return Canvas.GetLeft(m_body); }
		public double getCurrentY() { return Canvas.GetTop(m_body); }
		public void setDragging(bool dragging) { m_isDragging = dragging; }

		public double getRelativeX() { return m_relativeX; }
		public double getRelativeY() { return m_relativeY; }
		public void setRelativeX(double x) { m_relativeX = x; }
		public void setRelativeY(double y) { m_relativeY = y; }
		public Representation getParent() { return m_parent; }

		public void setBaseColor(Color color) 
		{ 
			m_baseColor = color;
			setBrushes();
			applyBrushes();
		}

		public void setName(string name) { m_name = name; m_lblName.Content = m_name; }

		// -- FUNCTIONS --

		// finds what offset should be for given node
		public int getNodeOffsetX(bool isInput, int groupNum)
		{
			if (isInput) { return m_inOffStartPoint + groupNum * GraphicContainer.NODE_SIZE; }
			else { return m_outOffStartPoint + groupNum * GraphicContainer.NODE_SIZE; }
		}
		public int getNodeOffsetY(bool isInput)
		{
			if (isInput) { return -(GraphicContainer.NODE_SIZE); }
			else { return (int)m_body.Height; }
		}

		private void createDrawing(int x, int y, int numIn, int numOut, Color initialColor)
		{
			int width = calcOptimalWidth(numIn, numOut);

			m_baseColor = initialColor;
			setBrushes();
			applyBrushes();

			// create body
			m_body.Height = GraphicContainer.REP_MINIMUM_HEIGHT;
			m_body.Width = width;
			m_body.RadiusX = 5;
			m_body.RadiusY = 5;
			m_body.Stroke = m_brushBorder;
			m_body.StrokeThickness = 2;
			Canvas.SetZIndex(m_body, GraphicContainer.REP_Z_LEVEL);

			// board (inner part of body)
			m_board.Height = GraphicContainer.REP_MINIMUM_HEIGHT - 30;
			m_board.Width = width - 12;
			m_board.RadiusX = 3;
			m_board.RadiusY = 3;
			m_board.IsHitTestVisible = false;
			Canvas.SetZIndex(m_board, GraphicContainer.REP_Z_LEVEL);

			// labels
			m_lblID.Margin = m_noThickness;
			m_lblID.Content = m_id + " " + m_version;
			m_lblID.Foreground = m_brushForeground;
			m_lblID.IsHitTestVisible = false;
			Canvas.SetZIndex(m_lblID, GraphicContainer.REP_Z_LEVEL);

			m_lblName.Margin = m_noThickness;
			m_lblName.Content = m_name;
			m_lblName.Foreground = m_brushForeground;
			m_lblName.Padding = m_noThickness;
			m_lblName.Margin = m_noThickness;
			m_lblName.Height = 20;
			Canvas.SetZIndex(m_lblName, GraphicContainer.REP_Z_LEVEL);

			m_lblContent.Foreground = m_brushForeground;
			m_lblContent.Content = m_algorithm + "\n" + "Accuracy stuff blah blah blah blah";
			m_lblContent.IsHitTestVisible = false;
			m_lblContent.Width = m_board.Width;
			Canvas.SetZIndex(m_lblContent, GraphicContainer.REP_Z_LEVEL);

			move(x, y);

			// TODO: mousedown events will be handled in here?

			// ADD ALL THE THINGS!!
			Canvas cnvs = Master.getCanvas();
			cnvs.Children.Add(m_body);
			cnvs.Children.Add(m_board);
			cnvs.Children.Add(m_lblID);
			cnvs.Children.Add(m_lblContent);
			cnvs.Children.Add(m_lblName);

			m_body.MouseDown += new MouseButtonEventHandler(evt_MouseDown);
			m_lblName.MouseDown += new MouseButtonEventHandler(name_MouseDown);
		}

		// moves entire representation to passed x and y (based on upper left corner)
		public void move(double x, double y) 
		{
			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);

			Canvas.SetLeft(m_board, x + GraphicContainer.REP_BOARD_PADDING_LEFT);
			Canvas.SetTop(m_board, y + GraphicContainer.REP_BOARD_PADDING_TOP);

			Canvas.SetLeft(m_lblID, x);
			Canvas.SetTop(m_lblID, y);

			Canvas.SetLeft(m_lblName, x + m_body.Width + 2);
			Canvas.SetTop(m_lblName, y + (m_body.Height / 2) - (m_lblName.Height / 2));

			Canvas.SetLeft(m_lblContent, x + GraphicContainer.REP_BOARD_PADDING_LEFT);
			Canvas.SetTop(m_lblContent, y + GraphicContainer.REP_BOARD_PADDING_TOP);

			// move nodes
			foreach (Node n in m_parent.getNodes()) { n.getGraphic().move(x, y); }
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
			m_brushBase = new SolidColorBrush(m_baseColor);
			m_brushLightenedBase = new SolidColorBrush(lightenColor(m_baseColor, 0.6f));
		}

		private void applyBrushes()
		{
			m_body.Fill = m_brushBase;
			m_board.Fill = m_brushLightenedBase;
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
				Master.log("Representation ID " + m_id + " clicked", Colors.Salmon);
				m_isDragging = true;
				m_body.Stroke = m_brushBorderSelected;

				// get relative coordinates
				Point p = e.GetPosition(Master.getCanvas());
				m_relativeX = p.X - Canvas.GetLeft(m_body);
				m_relativeY = p.Y - Canvas.GetTop(m_body);

				Master.setDraggingRepresentation(true, this);
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
				Master.setCommandPrompt("edit rep -" + m_id + " -");
			}
		}

		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_isDragging)
			{
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X - m_relativeX;
				double y = p.Y - m_relativeY;
				move(x, y);
			}
		}

		public void evt_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_isDragging && e.LeftButton == MouseButtonState.Released)
			{
				m_isDragging = false;
				Master.setDraggingRepresentation(false, null);

				m_body.Stroke = m_brushBorder;
				m_relativeX = 0;
				m_relativeY = 0;
			}
		}

		public void name_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Master.setCommandPrompt("edit rep -" + m_id + " -lbl -\"");
		}
	}
}
