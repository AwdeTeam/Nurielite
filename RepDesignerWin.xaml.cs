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
using System.IO;

using System.Xml.Linq;

namespace Nurielite
{
    /// <summary>
    /// Interaction logic for RepDesignerWin.xaml
    /// </summary>
	public partial class RepDesignerWin : Window
	{
		private MainWindow m_pParent;
        private int m_iNumOut = 1;

		public RepDesignerWin(MainWindow pParent)
		{
			m_pParent = pParent;
			InitializeComponent();

			//loadAlgorithmOptions(PyAlgorithm.getUnloadedAlgorithm()); // DEBUG
		}

		private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
		{
            AlgorithmLoader.loadAlgorithmBlock(cmbAlgorithmSpecific.SelectedItem.ToString(), (AlgorithmType)cmbAlgorithmType.SelectedItem, Int32.Parse(numInputs.Text), m_iNumOut);
            //m_parent.appendBlock(pBlock);
			Close();
		}

		private void cmbAlgorithmType_Loaded(object sender, RoutedEventArgs e)
		{
			List<AlgorithmType> pTypes = Enum.GetValues(typeof(AlgorithmType)).Cast<AlgorithmType>().ToList();
			cmbAlgorithmType.ItemsSource = pTypes.ToList();
			cmbAlgorithmType.SelectedIndex = 0;
		}

		private void cmbAlgorithmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbAlgorithmType.SelectedItem.Equals(AlgorithmType.Input))
			{
                numInputs.IsEnabled = false;
                numInputs.Text = "0";
                m_iNumOut = 1;
			}
			else if (cmbAlgorithmType.SelectedItem.Equals(AlgorithmType.Output))
			{
                m_iNumOut = 0;
                numInputs.IsEnabled = true;
                if (numInputs.Text == "0")
                    numInputs.Text = "1";
			}
			else 
			{
                m_iNumOut = 1;
                numInputs.IsEnabled = true;
                if (numInputs.Text == "0")
                    numInputs.Text = "1";
			}

			// get all algorithm names from folder structure
			
			List<DirectoryInfo> lAlgorithmDirs = new DirectoryInfo("./" + cmbAlgorithmType.SelectedItem.ToString()).EnumerateDirectories().ToList();
			List<string> lAlgorithms = new List<string>();
			foreach (DirectoryInfo pDI in lAlgorithmDirs) { lAlgorithms.Add(pDI.Name); }

			// populate combo box
			cmbAlgorithmSpecific.IsEnabled = true;
			cmbAlgorithmSpecific.ItemsSource = lAlgorithms;
			//lblType.Content = cmbAlgorithmType.SelectedItem.ToString();
		}
	}
}
