using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Nurielite
{
	public class ConnectionGraphic
	{
		private const int Z_LEVEL = 9;

		// member variables
		private Line m_lnBody = new Line();
		private Label m_lblTypeName = new Label();
		private SolidColorBrush m_brushStroke = new SolidColorBrush(Colors.Black);

		private Connection m_pParent;

		private bool m_bIsDragging = false;

		// construction
		public ConnectionGraphic(Connection parent)
		{
			m_pParent = parent;
			createDrawing();
		}

		// properties
		public Connection Parent { get { return m_pParent; } set { m_pParent = value; } }
		
		public void setStrokeColor(Color color)
		{
			m_brushStroke = new SolidColorBrush(color);
			m_lnBody.Stroke = m_brushStroke;
		}

		// -- FUNCTIONS -- 
		private void createDrawing()
		{
			m_lnBody.Stroke = m_brushStroke;
			m_lnBody.StrokeThickness = 2;
			Nodule origin = m_pParent.Origin;
			m_lnBody.X1 = origin.Graphic.CurrentX + GraphicContainer.NODE_SIZE / 2;
			m_lnBody.Y1 = origin.Graphic.CurrentY + GraphicContainer.NODE_SIZE / 2;
			m_lnBody.X2 = origin.Graphic.CurrentX;
			m_lnBody.Y2 = origin.Graphic.CurrentY;
			m_lnBody.IsHitTestVisible = false; // make click-throughable
			Canvas.SetZIndex(m_lnBody, Z_LEVEL);

			// TODO: null check necessary?
			m_lblTypeName.Content = origin.Name;
			Canvas.SetLeft(m_lblTypeName, m_lnBody.X2);
			Canvas.SetTop(m_lblTypeName, m_lnBody.Y2);

			Master.getCanvas().Children.Add(m_lnBody);
			Master.getCanvas().Children.Add(m_lblTypeName);

			Master.setDraggingConnection(true, this);
			m_bIsDragging = true;

			m_lnBody.MouseDown += new MouseButtonEventHandler(evt_MouseDown);
			m_lnBody.MouseMove += new MouseEventHandler(evt_MouseDown);
		}

		// visually attaches connection to passed node
		public void finishVisualConnection(Nodule n)
		{
			adjustSecondPoint((int)(n.Graphic.CurrentX+ GraphicContainer.NODE_SIZE / 2), (int)(n.Graphic.CurrentY + GraphicContainer.NODE_SIZE / 2));
			m_lnBody.IsHitTestVisible = true; // make clickable
			Master.getCanvas().Children.Remove(m_lblTypeName);
		}

		// moves the end of the line attached to passed node
		public void adjustRelatedPoint(Nodule node)
		{
			Nodule origin = m_pParent.Origin;
			Nodule end = m_pParent.End;
			if (node.Equals(origin)) { adjustFirstPoint((int)(origin.Graphic.CurrentX + GraphicContainer.NODE_SIZE / 2), (int)(origin.Graphic.CurrentY + GraphicContainer.NODE_SIZE / 2)); }
			else if (node.Equals(end)) { adjustSecondPoint((int)(end.Graphic.CurrentX + GraphicContainer.NODE_SIZE / 2), (int)(end.Graphic.CurrentY + GraphicContainer.NODE_SIZE / 2)); }
		}

		// adjusts "origin" connected point
		public void adjustFirstPoint(int iX, int iY)
		{
			m_lnBody.X1 = iX;
			m_lnBody.Y1 = iY;
		}

		// adjusts "end" connected point
		public void adjustSecondPoint(int iX, int iY)
		{
			m_lnBody.X2 = iX;
			m_lnBody.Y2 = iY;
			if (m_bIsDragging == true)
			{
				Canvas.SetLeft(m_lblTypeName, m_lnBody.X2);
				Canvas.SetTop(m_lblTypeName, m_lnBody.Y2);
			}
		}

		public void removeGraphic()
		{
			Master.getCanvas().Children.Remove(m_lnBody);
			Master.getCanvas().Children.Remove(m_lblTypeName);
		}


		// -- EVENT HANDLERS --

		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bIsDragging)
			{
				Point p = e.GetPosition(Master.getCanvas());
				int x = (int)p.X;
				int y = (int)p.Y;
				adjustSecondPoint(x, y);
			}
		}

		public void evt_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.RightButton == MouseButtonState.Pressed) { m_pParent.removeConnection(); }
		}
	}
}
