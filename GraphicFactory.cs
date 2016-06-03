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
		public static BlockGraphic createBlockGraphic(Block parent, int numIn, int numOut, Color color)
		{
			BlockGraphic bg = new BlockGraphic(parent, numIn, numOut, color);
			Master.getGraphicContainer().addBlockGraphic(bg);
			return bg;
		}

		public static BlockGraphic createBlockGraphic(Block parent, Color color)
		{
			BlockGraphic bg = new BlockGraphic(parent, 1, 1, color);
			Master.getGraphicContainer().addBlockGraphic(bg);
			return bg;
		}
	
		public static NoduleGraphic createNoduleGraphic(Nodule parent)
		{
			return new NoduleGraphic(parent);
		}

		public static ConnectionGraphic createConnectionGraphic(Connection parent)
		{
			return new ConnectionGraphic(parent);
		}
	}
}
