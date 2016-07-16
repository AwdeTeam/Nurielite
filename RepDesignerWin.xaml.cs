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
		private MainWindow m_parent;

		public RepDesignerWin(MainWindow parent)
		{
			m_parent = parent;
			InitializeComponent();

			//loadAlgorithmOptions(PyAlgorithm.getUnloadedAlgorithm()); // DEBUG
		}

		private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
		{
			AlgorithmLoader.loadAlgorithmBlock(cmbAlgorithmSpecific.SelectedItem.ToString(), (AlgorithmType)cmbAlgorithmType.SelectedItem, ray(lstInputs.SelectedItems), ray(lstOutputs.SelectedItems));
            //m_parent.appendBlock(pBlock);
			Close();
		}

		private Datatype[] ray(System.Collections.IList list)
		{
			Datatype[] ray = new Datatype[list.Count];
			for (int i = 0; i < list.Count; i++)
				ray[i] = (Datatype)list[i];

			return ray;
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
				lstInputs.IsEnabled = false;
				lstOutputs.IsEnabled = true;	
				lstInputs.SelectedIndex = -1;
			}
			else if (cmbAlgorithmType.SelectedItem.Equals(AlgorithmType.Output))
			{
				lstInputs.IsEnabled = true;
				lstOutputs.IsEnabled = false;
				lstInputs.SelectedIndex = -1;
			}
			else 
			{
				lstOutputs.IsEnabled = true;
				lstInputs.IsEnabled = true;
			}

			// get all algorithm names from folder structure
			
			List<DirectoryInfo> pAlgorithmDirs = new DirectoryInfo("./" + cmbAlgorithmType.SelectedItem.ToString()).EnumerateDirectories().ToList();
			List<string> pAlgorithms = new List<string>();
			foreach (DirectoryInfo pDI in pAlgorithmDirs) { pAlgorithms.Add(pDI.Name); }

			// populate combo box
			cmbAlgorithmSpecific.IsEnabled = true;
			cmbAlgorithmSpecific.ItemsSource = pAlgorithms;
			lblType.Content = cmbAlgorithmType.SelectedItem.ToString();

		}

		private void lstOutputs_Loaded(object sender, RoutedEventArgs e)
		{
			lstOutputs.ItemsSource = Datatype.Directory.Values;
		}

		private void lstInputs_Loaded(object sender, RoutedEventArgs e)
		{
			lstInputs.ItemsSource = Datatype.Directory.Values;
            foreach(Datatype d in Datatype.Directory.Values)
            {
                TextBox textbox = new TextBox();
                textbox.Height = 18;
                textbox.Uid = d.Name;
                textbox.Text = "0";
                pnlMult.Children.Add(textbox);
            }
		}

		private void lstInputs_SelectionChanged(object sender, RoutedEventArgs e)
		{

		}

		private static Datatype[] getSelectedTypes(ListBox box)
		{
			//List dexes = box.SelectedIndices;
			return null;
		}

		private void cmbAlgorithmSpecific_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
