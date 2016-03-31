using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nurielite
{
	public class GraphicFactory
	{
		public static RepresentationGraphic createRepresentationGraphic(Representation parent, int numIn, int numOut, Color color)
		{
			RepresentationGraphic rg = new RepresentationGraphic(parent, numIn, numOut, color);
			Master.getGraphicContainer().addRepresentationGraphic(rg);
			return rg;
		}

		public static RepresentationGraphic createRepresentationGraphic(Representation parent, Color color)
		{
			RepresentationGraphic rg = new RepresentationGraphic(parent, 1, 1, color);
			Master.getGraphicContainer().addRepresentationGraphic(rg);
			return rg;
		}
	
		public static NodeGraphic createNodeGraphic(Node parent)
		{
			return new NodeGraphic(parent);
		}

		public static ConnectionGraphic createConnectionGraphic(Connection parent)
		{
			return new ConnectionGraphic(parent);
		}
	}
}
