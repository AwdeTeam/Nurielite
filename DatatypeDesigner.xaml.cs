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
    /// Interaction logic for DatatypeDesigner.xaml
    /// </summary>
    public partial class DatatypeDesigner : Window
    {
        public DatatypeDesigner()
        {
            InitializeComponent();
        }

        private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
        {
            Datatype.defineDatatype(txtNameInput.Text, Int32.Parse(txtNumInts.Text), Int32.Parse(txtNumReals.Text), null);
            Close();
        }
    }
}
