﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Nurielite
{
	public class NoduleGraphic
	{
		private Ellipse m_pBody = new Ellipse();
		
		private Nodule m_pParent;

		private SolidColorBrush m_brushFill = new SolidColorBrush(Colors.White);
		private SolidColorBrush m_brushBorder = new SolidColorBrush(Colors.Black);

		private int m_iOffsetX = 0;
		private int m_iOffsetY = 0;

		// construction
		public NoduleGraphic(Nodule pParent)
		{
			m_pParent = pParent;
			m_iOffsetX = pParent.Parent.Graphic.getNoduleOffsetX(pParent.IsInput, pParent.GroupNum);
			m_iOffsetY = pParent.Parent.Graphic.getNoduleOffsetY(pParent.IsInput);

			createDrawing();
		}

		// properties
		public double CurrentX { get { return Canvas.GetLeft(m_pBody); } }
		public double CurrentY { get { return Canvas.GetTop(m_pBody); } }

		// -- FUNCTIONS --
		private void createDrawing()
		{
			// create body
			m_pBody.Fill = m_brushFill;
			m_pBody.Stroke = m_brushBorder;
			m_pBody.StrokeThickness = 2;
			m_pBody.Height = GraphicContainer.NODULE_SIZE;
			m_pBody.Width = GraphicContainer.NODULE_SIZE;
            m_pBody.ToolTip = m_pParent.Name;
			Canvas.SetZIndex(m_pBody, GraphicContainer.NODULE_Z_LEVEL);

			// inital position
			move(m_pParent.Parent.Graphic.CurrentX, m_pParent.Parent.Graphic.CurrentY);

			// add to canvas
			Master.getCanvas().Children.Add(m_pBody);

			// event handlers
			m_pBody.MouseDown += new MouseButtonEventHandler(evt_MouseDown);
			m_pBody.MouseUp += new MouseButtonEventHandler(evt_MouseUp);
		}

		public void deleteGraphic() { Master.getCanvas().Children.Remove(m_pBody); }

		public void move(double dX, double dY)
		{
			Canvas.SetLeft(m_pBody, dX + m_iOffsetX);
			Canvas.SetTop(m_pBody, dY + m_iOffsetY);
			
			// update all connections
			foreach (Connection pConnection in m_pParent.Connections) { pConnection.Graphic.adjustRelatedPoint(m_pParent); }
		}

        public void setTooltip(string sTooltip) //Not working, fix later
        {
            m_pBody.ToolTip = sTooltip;
        }

		// -- EVENT HANDLERS --

		private void evt_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed) { Connection con = new Connection(m_pParent); }
            else if (e.RightButton == MouseButtonState.Pressed) { new NameNoduleWin(m_pParent).Show(); }
		}

		private void evt_MouseUp(object sender, MouseEventArgs e)
		{
			if (Master.getDraggingConnection() != null)
			{
                if (Master.VerboseMode) Master.log("Released on node");
			
				m_pParent.connect(Master.getDraggingConnection().Parent);
				Master.setDraggingConnection(false, null);
			}
		}
    }
}
