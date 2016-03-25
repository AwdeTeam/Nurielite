using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nurielite
{
    class AlgorithmLoader
    {
        public static Representation generateRepresentation(String name, Representation.AlgorithmFamily family, Datatype[] inputs, Datatype[] outputs)
        {
            Representation r = new Representation(inputs, outputs);
            r.setFamily(family);
            r.setName(name);
            Color color = Colors.Gray;
            switch(family)
            {
                case Representation.AlgorithmFamily.Classifier:
                {
                    color = Colors.SeaGreen;
                    break;
                }

                case Representation.AlgorithmFamily.Clustering:
                {
                    color = Colors.Turquoise;
                    break;
                }

                case Representation.AlgorithmFamily.DimensionReduction:
                {
                    color = Colors.Thistle;
                    break;
                }

                case Representation.AlgorithmFamily.Operation:
                {
                    color = Colors.SkyBlue;
                    break;
                }

                case Representation.AlgorithmFamily.Input:
                {
                    color = Colors.Violet;
                    break;
                }

                case Representation.AlgorithmFamily.Output:
                {
                    color = Colors.Violet;
                    break;
                }
            }

            return r;
        }

        public static Representation load(String path)
        {
            string filePath = path;
            StreamReader sr = new StreamReader(filePath);
            string[] data = sr.ReadLine().Split(',');
            return generateRepresentation(data[0], Representation.AlgorithmFamily.Input, null, null);
        }
    }
}
