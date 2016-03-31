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
        private MainWindow m_parent;

        public RepDesignerWin(MainWindow parent)
        {
            m_parent = parent;
            InitializeComponent();
        }

        private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
        {
            m_parent.log("" + cmbAlgorithmType.SelectedIndex);
            Representation newrep = AlgorithmLoader.generateRepresentation(txtNameInput.Text, cmbAlgorithmType.SelectedIndex,
                    new Datatype[] { Datatype.getType(0) }, new Datatype[] { Datatype.getType(1) });
            Close();
        }

        private void cmbAlgorithmType_Loaded(object sender, RoutedEventArgs e)
        {
            cmbAlgorithmType.ItemsSource = Representation.ALGORITHM_TYPES.ToList();
        }

        private void cmbAlgorithmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_parent.log("" + Representation.ALGORITHM_TYPES[ cmbAlgorithmType.SelectedIndex ]);
        }
    }
}
