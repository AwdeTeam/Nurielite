using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AlgGui
{
	public class GraphicContainer
	{
		/*private bool m_isDraggingRepresentation;
		private RepresentationGraphic m_draggingRepresentation;*/

		// this class is "central hub" for all event handlers. Takes any events from mainwindow and distributes as necessary.
		// (handles dragging controls, etc)
		// ONLY GOAL OF EVENT HANDLERS HERE ARE TO CALL/ROUTE THE APPROPRIATE EVENT HANDLERS ON COMPONENTS (unless global thing like panning)

		// graphical data constants
		// NOTE: apparently const implicitly make these "static"
		public const int NODE_SIZE = 10;

		public const int REP_MINIMUM_WIDTH = 85;
		public const int REP_MINIMUM_HEIGHT = 80;
		public const int REP_BOARD_PADDING_TOP = 24;
		public const int REP_BOARD_PADDING_LEFT = 6;

		public const int REP_Z_LEVEL = 10;
		public const int NODE_Z_LEVEL = 10;
		public const int CONNECTION_Z_LEVEL = 9;

		// member variables
		private bool m_isDraggingScreen = false;
		private bool m_isDraggingConnection = false;
		private bool m_isDraggingRepresentation = false; 
		

		private RepresentationGraphic m_draggingRepresentation;
		private ConnectionGraphic m_draggingConnection;

		private Dictionary<int, RepresentationGraphic> m_repGraphics = new Dictionary<int, RepresentationGraphic>();

		public GraphicContainer() { }

		// PROPERTIES
		public RepresentationGraphic getRepresentationGraphic(int id) { return m_repGraphics[id]; }
		public void addRepresentationGraphic(RepresentationGraphic rg) { m_repGraphics.Add(rg.getParent().getID(), rg); }

		public void setDraggingRepresentation(bool dragging, RepresentationGraphic rep)
		{
			m_isDraggingRepresentation = dragging;
			m_draggingRepresentation = rep;
		}

		public void setDraggingConnection(bool dragging, ConnectionGraphic cg)
		{
			m_isDraggingConnection = dragging;
			m_draggingConnection = cg;
		}
		public ConnectionGraphic getDraggingConnection() { return m_draggingConnection; }

		// EVENT HANDLERS
		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_isDraggingScreen)
			{
				foreach (RepresentationGraphic rg in m_repGraphics.Values)
				{
					Point p = e.GetPosition(Master.getCanvas());
					double x = p.X - rg.getRelativeX();
					double y = p.Y - rg.getRelativeY();
					rg.move(x, y);
				}
			}
			else if (m_isDraggingRepresentation) { m_draggingRepresentation.evt_MouseMove(sender, e); }
			else if (m_isDraggingConnection) { m_draggingConnection.evt_MouseMove(sender, e); }
		}

		public void evt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// NOTE: individual left-click mousedown is handled completely in most of the individual graphic classes
			// (in other words, NOT routed through this class)
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				m_isDraggingScreen = true;
				foreach (RepresentationGraphic rg in m_repGraphics.Values)
				{
					Point p = e.GetPosition(Master.getCanvas());
					rg.setRelativeX(p.X - rg.getCurrentX());
					rg.setRelativeY(p.Y - rg.getCurrentY());
				}
			}
		}

		public void evt_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (m_isDraggingScreen)
			{
				m_isDraggingScreen = false;
				foreach (RepresentationGraphic rg in m_repGraphics.Values)
				{
					rg.setRelativeX(0);
					rg.setRelativeY(0);
				}
			}
			else if (m_isDraggingRepresentation) { m_draggingRepresentation.evt_MouseUp(sender, e); }
			else if (m_isDraggingConnection)
			{
				if (m_draggingConnection != null && !m_draggingConnection.getParent().isComplete()) { m_draggingConnection.removeGraphic(); }
				m_draggingConnection = null;
				m_isDraggingConnection = false;
			}
		}
	}
}
