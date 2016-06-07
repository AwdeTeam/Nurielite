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
		public static ConnectionGraphic createConnectionGraphic(Connection parent)
		{
			return new ConnectionGraphic(parent);
		}
	}
}
