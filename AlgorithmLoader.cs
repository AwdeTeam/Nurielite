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
        public static Block generateBlock(string name, string path, AlgorithmType family, Datatype[] inputs, Datatype[] outputs)
        {
            Color color = Colors.Gray;
            switch(family)
            {
                case AlgorithmType.Classifier:
                {
                    color = Colors.SeaGreen;
                    break;
                }

                case AlgorithmType.Clustering:
                {
                    color = Colors.Turquoise;
                    break;
                }

                case AlgorithmType.DimensionReduction:
                {
                    color = Colors.Thistle;
                    break;
                }

                case AlgorithmType.Operation:
                {
                    color = Colors.SkyBlue;
                    break;
                }

                case AlgorithmType.Input:
                {
                    color = Colors.Violet;
                    break;
                }

                case AlgorithmType.Output:
                {
                    color = Colors.Violet;
                    break;
                }
            }
            return new Block(inputs, outputs, name, path, family, color);
        }

        public static Block generateBlock(string name, string path, int family, Datatype[] inputs, Datatype[] outputs)
        {
            return generateBlock(name, path, (AlgorithmType) family, inputs, outputs);
        }

        public static Block load(string path)
        {
            string filePath = path;
            StreamReader sr = new StreamReader(filePath);
            string[] data = sr.ReadLine().Split(',');
            return generateBlock(data[0], path, AlgorithmType.Input, null, null);
        }
    }
}
