using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgGui
{
	public class GraphicFactory
	{
		public static RepresentationGraphic createRepresentationGraphic(Representation parent, int numIn, int numOut)
		{
			RepresentationGraphic rg = new RepresentationGraphic(parent, numIn, numOut);
			Master.getGraphicContainer().addRepresentationGraphic(rg);
			return rg;
		}

		public static RepresentationGraphic createRepresentationGraphic(Representation parent)
		{
			RepresentationGraphic rg = new RepresentationGraphic(parent, 1, 1);
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
