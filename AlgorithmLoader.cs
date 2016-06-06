using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nurielite
{
    /// <summary>
    /// Builds a Block from a set of constraints
    /// </summary>
    /// <remarks>
    /// So... this class might be pretty much pointless as it's basically just a constructor for Block
    /// </remarks>
    class AlgorithmLoader
    {
        public static Block generateBlock(String name, String family, Datatype[] inputs, Datatype[] outputs)
        {
            Color color = Colors.Gray;
            switch(family)
            {
                case "classifier":
                {
                    color = Colors.SeaGreen;
                    break;
                }

                case "clustering":
                {
                    color = Colors.Turquoise;
                    break;
                }

                case "dimension_reduction":
                {
                    color = Colors.Thistle;
                    break;
                }

                case "operation":
                {
                    color = Colors.SkyBlue;
                    break;
                }

                case "input":
                {
                    color = Colors.Violet;
                    break;
                }

                case "output":
                {
                    color = Colors.Violet;
                    break;
                }
            }
            return new Block(inputs, outputs, name, family, color);
        }

        public static Block generateBlock(String name, int family, Datatype[] inputs, Datatype[] outputs)
        {
            return generateBlock(name, Block.ALGORITHM_TYPES[family], inputs, outputs);
        }

        public static Block load(String path)
        {
            string filePath = path;
            StreamReader sr = new StreamReader(filePath);
            string[] data = sr.ReadLine().Split(',');
            return generateBlock(data[0], "input", null, null);
        }
    }
}
