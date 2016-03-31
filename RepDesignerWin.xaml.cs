using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nurielite
{
    /// <summary>
    /// Interaction logic for RepDesignerWin.xaml
    /// </summary>
    public partial class RepDesignerWin : Window
    {
        MainWindow m_parent;

        public RepDesignerWin(MainWindow parent)
        {
            InitializeComponent();
        }

        private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
        {
            Representation newrep = AlgorithmLoader.generateRepresentation(txtNameInput.Text, Representation.AlgorithmFamily.Operation,
                    new Datatype[] { Datatype.getType(0) }, new Datatype[] { Datatype.getType(1) });
            Close();
        }
    }
}
