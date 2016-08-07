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
    /// Interaction logic for NameNoduleWin.xaml
    /// </summary>
    public partial class NameNoduleWin : Window
    {
        private Nodule m_pMaster;

        public NameNoduleWin(Nodule master)
        {
            m_pMaster = master;
            InitializeComponent();
            txtNoduleName.Text = m_pMaster.Name;
            txtNoduleName.Focus();
            txtNoduleName.SelectAll();
        }

        private void txtNoduleName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                m_pMaster.Name = txtNoduleName.Text;
                m_pMaster.Graphic.setTooltip(txtNoduleName.Text);
                Close();
            }
        }
    }
}
