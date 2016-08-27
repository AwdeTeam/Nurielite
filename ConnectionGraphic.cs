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
	/// <summary>
	/// Contains the graphical code for the connection class. It is responsible rendering the connection between the two nodules.
	/// </summary>
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
		/// <summary>
		/// Creates a visual representation for the connection.
		/// </summary>
		/// <param name="pParent">The connection object that this instance visually represents.</param>
		public ConnectionGraphic(Connection pParent)
		{
			m_pParent = pParent;
			createDrawing();
		}

		// properties
		/// <summary>
		/// The connection object that this instance visually represents. (<see cref="Connection"/>)
		/// </summary>
		public Connection Parent { get { return m_pParent; } set { m_pParent = value; } }
		
		// -- FUNCTIONS -- 
		private void createDrawing()
		{
			m_lnBody.Stroke = m_brushStroke;
			m_lnBody.StrokeThickness = 2;
			Nodule origin = m_pParent.Origin;
			m_lnBody.X1 = origin.Graphic.CurrentX + GraphicContainer.NODULE_SIZE / 2;
			m_lnBody.Y1 = origin.Graphic.CurrentY + GraphicContainer.NODULE_SIZE / 2;
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

		/// <summary>
		/// Visually attaches connection to passed nodule.
		/// </summary>
		/// <param name="pNodule">Nodule to end this connection on.</param>
		public void finishVisualConnection(Nodule pNodule)
		{
			adjustSecondPoint((int)(pNodule.Graphic.CurrentX+ GraphicContainer.NODULE_SIZE / 2), (int)(pNodule.Graphic.CurrentY + GraphicContainer.NODULE_SIZE / 2));
			m_lnBody.IsHitTestVisible = true; // make clickable
			Master.getCanvas().Children.Remove(m_lblTypeName);
		}

		// moves the end of the line attached to passed node
		/// <summary>
		/// Updates/corrects the position of the end of the connection line based on the passed nodule.
		/// </summary>
		/// <param name="pNode">The nodule that has moved/needs the connected line point updated.</param>
		public void adjustRelatedPoint(Nodule pNode)
		{
			Nodule pOrigin = m_pParent.Origin;
			Nodule pEnd = m_pParent.End;
			if (pNode.Equals(pOrigin)) { adjustFirstPoint((int)(pOrigin.Graphic.CurrentX + GraphicContainer.NODULE_SIZE / 2), (int)(pOrigin.Graphic.CurrentY + GraphicContainer.NODULE_SIZE / 2)); }
			else if (pNode.Equals(pEnd)) { adjustSecondPoint((int)(pEnd.Graphic.CurrentX + GraphicContainer.NODULE_SIZE / 2), (int)(pEnd.Graphic.CurrentY + GraphicContainer.NODULE_SIZE / 2)); }
		}

		/// <summary>
		/// Moves the origin point (<see cref="Connection.Origin"/>) of the connection to the the passed coordinates.
		/// </summary>
		/// <param name="iX">X coordinate to move the origin point to.</param>
		/// <param name="iY">Y coordinate to move the origin point to.</param>
		public void adjustFirstPoint(int iX, int iY)
		{
			m_lnBody.X1 = iX;
			m_lnBody.Y1 = iY;
		}

		/// <summary>
		/// Moves the end point (<see cref="Connection.End"/>) of the connection to the passed coordinates.
		/// </summary>
		/// <param name="iX">X coordinate to move the end point to.</param>
		/// <param name="iY">Y coordinate to move the end point to.</param>
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

		/// <summary>
		/// Safely removes the visual representation of the connection from the display canvas.
		/// </summary>
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
				Point pMousePoint = e.GetPosition(Master.getCanvas());
				int x = (int)pMousePoint.X;
				int y = (int)pMousePoint.Y;
				adjustSecondPoint(x, y);
			}
		}

		public void evt_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.RightButton == MouseButtonState.Pressed) { m_pParent.removeConnection(); }
		}
	}
}
