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
	/// <summary>
	/// This class is the "central hub" for all display canvas event handlers. It takes any associated events from MainWindow and distributes them to the graphical elements as needed.
	/// </summary>
	public class GraphicContainer
	{
		// this class is "central hub" for all event handlers. Takes any events from MainWindow and distributes as necessary.
		// (handles dragging controls, etc)
		// ONLY GOAL OF EVENT HANDLERS HERE ARE TO CALL/ROUTE THE APPROPRIATE EVENT HANDLERS ON COMPONENTS (unless global thing like panning)

		// graphical data constants
		// NOTE: apparently const implicitly make these "static"
		/// <summary>
		/// Diameter of a nodule
		/// </summary>
		public const int NODULE_SIZE = 10;

		/// <summary>
		/// Width of a block graphic.
		/// </summary>
		public const int BLOCK_MINIMUM_WIDTH = 85;
		/// <summary>
		/// Height of a block graphic.
		/// </summary>
		public const int BLOCK_MINIMUM_HEIGHT = 80;
		/// <summary>
		/// Amount of padding from the top on the inside of a block graphic.
		/// </summary>
		public const int BLOCK_BOARD_PADDING_TOP = 24;
		/// <summary>
		/// Amount of padding from the left on the inside of a block graphic.
		/// </summary>
		public const int BLOCK_BOARD_PADDING_LEFT = 6;

		/// <summary>
		/// The default Z level of a block graphic. 
		/// </summary>
		public const int BLOCK_Z_LEVEL = 10;
		/// <summary>
		/// The default Z level of a nodule graphic.
		/// </summary>
		public const int NODULE_Z_LEVEL = 10;
		/// <summary>
		/// The default Z level of a connection graphic.
		/// </summary>
		public const int CONNECTION_Z_LEVEL = 9;

		// member variables
		private bool m_bIsDraggingScreen = false;
		private bool m_bIsDraggingConnection = false;
		private bool m_bIsDraggingBlock = false; 
		

		private BlockGraphic m_pDraggingBlock;
		private ConnectionGraphic m_pDraggingConnection;

		private Dictionary<int, BlockGraphic> m_dBlockGraphics = new Dictionary<int, BlockGraphic>();

		// PROPERTIES
		/// <summary>
		/// Adds a block graphic to the display canvas.
		/// </summary>
		/// <param name="pBlockGraphic">The block graphic to add.</param>
		public void addBlockGraphic(BlockGraphic pBlockGraphic) { m_dBlockGraphics.Add(pBlockGraphic.Parent.ID, pBlockGraphic); }

		/// <summary>
		/// Sets the dragging status of a particular block graphic. 
		/// </summary>
		/// <param name="bDragging">Determines whether the block graphic should act as though it is being dragged or not.</param>
		/// <param name="pBlockGraphic">The block graphic to change the property of.</param>
		public void setDraggingBlock(bool bDragging, BlockGraphic pBlockGraphic)
		{
			m_bIsDraggingBlock = bDragging;
			m_pDraggingBlock = pBlockGraphic;
		}

		/// <summary>
		/// Sets the dragging status of a connection graphic.
		/// </summary>
		/// <param name="bDragging">Determines whether the connection graphic should act as though it is being dragged or not.</param>
		/// <param name="pConnectionGraphic">The connection graphic to change the property of.</param>
		public void setDraggingConnection(bool bDragging, ConnectionGraphic pConnectionGraphic)
		{
			m_bIsDraggingConnection = bDragging;
			m_pDraggingConnection = pConnectionGraphic;
		}
		/// <summary>
		/// Returns the connection graphic of the connection that is currently being dragged.
		/// </summary>
		public ConnectionGraphic getDraggingConnection() { return m_pDraggingConnection; }

		// EVENT HANDLERS
		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_bIsDraggingScreen)
			{
				foreach (BlockGraphic pBlockGraphic in m_dBlockGraphics.Values)
				{
					Point pMousePoint = e.GetPosition(Master.getCanvas());
                    double dX = pMousePoint.X - pBlockGraphic.RelativeX;
					double dY = pMousePoint.Y - pBlockGraphic.RelativeY;
					pBlockGraphic.move(dX, dY);
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
				foreach (BlockGraphic pBlockGraphic in m_dBlockGraphics.Values)
				{
					Point pMousePoint = e.GetPosition(Master.getCanvas());
					pBlockGraphic.RelativeX = pMousePoint.X - pBlockGraphic.CurrentX;
					pBlockGraphic.RelativeY = pMousePoint.Y - pBlockGraphic.CurrentY;
				}
			}
		}

		public void evt_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (m_bIsDraggingScreen)
			{
				m_bIsDraggingScreen = false;
				foreach (BlockGraphic pBlockGraphic in m_dBlockGraphics.Values)
				{
                    pBlockGraphic.RelativeX = 0;
                    pBlockGraphic.RelativeY = 0;
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
