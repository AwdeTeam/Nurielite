using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Nurielite
{
	public class GraphicContainer
	{
		/*private bool m_isDraggingRepresentation;
		private RepresentationGraphic m_draggingRepresentation;*/

		// this class is "central hub" for all event handlers. Takes any events from MainWindow and distributes as necessary.
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
		private bool m_bIsDraggingScreen = false;
		private bool m_bIsDraggingConnection = false;
		private bool m_bIsDraggingBlock = false; 
		

		private BlockGraphic m_pDraggingBlock;
		private ConnectionGraphic m_pDraggingConnection;

		private Dictionary<int, BlockGraphic> m_pBlockGraphics = new Dictionary<int, BlockGraphic>();

		public GraphicContainer() { }

		// PROPERTIES
		public BlockGraphic getBlockGraphic(int id) { return m_pBlockGraphics[id]; }
		public void addBlockGraphic(BlockGraphic rg) { m_pBlockGraphics.Add(rg.Parent.ID, rg); }

		public void setDraggingBlock(bool bDragging, BlockGraphic pBlockGraphic)
		{
			m_bIsDraggingBlock = bDragging;
			m_pDraggingBlock = pBlockGraphic;
		}

		public void setDraggingConnection(bool dragging, ConnectionGraphic cg)
		{
			m_bIsDraggingConnection = dragging;
			m_pDraggingConnection = cg;
		}
		public ConnectionGraphic getDraggingConnection() { return m_pDraggingConnection; }

		// EVENT HANDLERS
		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bIsDraggingScreen)
			{
				foreach (BlockGraphic rg in m_pBlockGraphics.Values)
				{
					Point p = e.GetPosition(Master.getCanvas());
                    double x = p.X - rg.RelativeX;
					double y = p.Y - rg.RelativeY;
					rg.move(x, y);
				}
			}
			else if (m_bIsDraggingBlock) { m_pDraggingBlock.evt_MouseMove(sender, e); }
			else if (m_bIsDraggingConnection) { m_pDraggingConnection.evt_MouseMove(sender, e); }
		}

		public void evt_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// NOTE: individual left-click mousedown is handled completely in most of the individual graphic classes
			// (in other words, NOT routed through this class)
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				m_bIsDraggingScreen = true;
				foreach (BlockGraphic rg in m_pBlockGraphics.Values)
				{
					Point p = e.GetPosition(Master.getCanvas());
					rg.RelativeX = p.X - rg.CurrentX;
					rg.RelativeY = p.Y - rg.CurrentY;
				}
			}
		}

		public void evt_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (m_bIsDraggingScreen)
			{
				m_bIsDraggingScreen = false;
				foreach (BlockGraphic rg in m_pBlockGraphics.Values)
				{
                    rg.RelativeX = 0;
                    rg.RelativeY = 0;
				}
			}
			else if (m_bIsDraggingBlock) { m_pDraggingBlock.evt_MouseUp(sender, e); }
			else if (m_bIsDraggingConnection)
			{
				if (m_pDraggingConnection != null && !m_pDraggingConnection.Parent.IsComplete) { m_pDraggingConnection.removeGraphic(); }
				m_pDraggingConnection = null;
				m_bIsDraggingConnection = false;
			}
		}
	}
}
