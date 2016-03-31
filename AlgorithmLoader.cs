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
        public static Representation generateRepresentation(String name, String family, Datatype[] inputs, Datatype[] outputs)
        {
            Representation r = new Representation(inputs, outputs);
            r.setFamily(family);
            r.setName(name);
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

            return r;
        }

        public static Representation generateRepresentation(String name, int family, Datatype[] inputs, Datatype[] outputs)
        {
            return generateRepresentation(name, Representation.ALGORITHM_TYPES[family], inputs, outputs);
        }

        public static Representation load(String path)
        {
            string filePath = path;
            StreamReader sr = new StreamReader(filePath);
            string[] data = sr.ReadLine().Split(',');
            return generateRepresentation(data[0], "input", null, null);
        }
    }
}
