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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nurielite
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RepDesignerControl : UserControl
    {
        MainWindow m_parent;

        public RepDesignerControl(MainWindow parent)
        {
            m_parent = parent;
            InitializeComponent();
        }

        private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
        {
            Representation newrep = AlgorithmLoader.generateRepresentation(txtNameBox.Text, Representation.AlgorithmFamily.Operation,
                    new Datatype[] { Datatype.getType(0) }, new Datatype[] { Datatype.getType(1) });
            //m_parent.closePopup();
        }
    }
}
