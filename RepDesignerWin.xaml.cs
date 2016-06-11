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

			loadAlgorithmOptions(PyAlgorithm.getUnloadedAlgorithm()); // DEBUG
		}

		private void Button_Click_ConfirmNew(object sender, RoutedEventArgs e)
		{
			Block newrep = AlgorithmLoader.generateBlock(txtNameInput.Text, 
                PythonGenerator.getAlgorithmPath((AlgorithmType)cmbAlgorithmType.SelectedIndex, cmbAlgorithmSpecific.SelectedIndex), cmbAlgorithmType.SelectedIndex,
					ray(lstInputs.SelectedItems), ray(lstOutputs.SelectedItems));
            m_parent.appendBlock(newrep);
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
			cmbAlgorithmType.ItemsSource = new string[] { "operation", "classifier", "clustering", "dimension_reduction", "input", "output"}; //TODO, is there a way to make this work with the enum?
			cmbAlgorithmType.SelectedIndex = 0;
		}

		private void cmbAlgorithmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbAlgorithmType.SelectedItem.Equals("input"))
			{
				lstInputs.IsEnabled = false;
				lstInputs.SelectedIndex = -1;
				lstOutputs.IsEnabled = true;
			}

			if (cmbAlgorithmType.SelectedItem.Equals("output"))
			{
				lstOutputs.IsEnabled = false;
				lstOutputs.SelectedIndex = -1;
				lstInputs.IsEnabled = true;
			}

			if (!((cmbAlgorithmType.SelectedItem.Equals("input")) || (cmbAlgorithmType.SelectedItem.Equals("output"))))
			{
				lstOutputs.IsEnabled = true;
				lstInputs.IsEnabled = true;
			}

			switch (cmbAlgorithmType.SelectedItem.ToString())
			{
				case "operation":
					lblType.Content = "operation:";
					cmbAlgorithmSpecific.IsEnabled = true;
					cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType(AlgorithmType.Operation);
					break;
				case "classifier":
					lblType.Content = "classifier:";
					cmbAlgorithmSpecific.IsEnabled = true;
					cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType(AlgorithmType.Classifier);
					break;
				case "clustering":
					lblType.Content = "clustering:";
					cmbAlgorithmSpecific.IsEnabled = true;
					cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType(AlgorithmType.Clustering);
					break;
				case "input":
					lblType.Content = "input";
					cmbAlgorithmSpecific.IsEnabled = true;
					cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType(AlgorithmType.Input);
					break;
				case "output":
					lblType.Content = "output";
					cmbAlgorithmSpecific.IsEnabled = true;
					cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType(AlgorithmType.Output);
					break;
				default:
					lblType.Content = "";
					cmbAlgorithmSpecific.IsEnabled = false;
					break;
			}


		}

		private void lstOutputs_Loaded(object sender, RoutedEventArgs e)
		{
			lstOutputs.ItemsSource = Datatype.Directory;
		}

		private void lstInputs_Loaded(object sender, RoutedEventArgs e)
		{
			lstInputs.ItemsSource = Datatype.Directory;
		}

		private void lstInputs_SelectionChanged(object sender, RoutedEventArgs e)
		{

		}

		private static Datatype[] getSelectedTypes(ListBox box)
		{
			//List dexes = box.SelectedIndices;
			return null;
		}

		// TODO: figure out how to integrate/wire this up (will probably eventually be public)
		// TODO: Make it look a little nicer
		private void loadAlgorithmOptions(PyAlgorithm algorithm)
		{
			StackPanel guiStkPnl = spnlAlgOptions;
			
			//if (algorithm.getOptions()["XML"] == null) { /*ERROR, ERROR, ERROR*/ }

			//string xml = algorithm.getOptions()["XML"];
			string xml = " <options> <option pythonkey='ColIndices' guitype='txtbox' label='Column Indices' description='Comma separated list of column indices to ignore' default='' /> <option pythonkey='NumOutputs' guitype='txtbox' label='# Outputs' description='Number of outputs…' default='1' /> </options> ";
			XElement parsedXML = XElement.Parse(xml);
			//XElement optionsRoot = parsedXML.Element("options");

			// iterate through each option and construct gui element for each
			//IEnumerable<XElement> options = optionsRoot.Elements();
			IEnumerable<XElement> options = parsedXML.Elements();
			foreach(XElement option in options)
			{
				string pythonKey = option.Attribute("pythonkey").Value;
				string guiType = option.Attribute("guitype").Value;
				string label = option.Attribute("label").Value;
				string description = option.Attribute("description").Value;
				string defaultValue = "";
				if (option.Attribute("default") != null) { defaultValue = option.Attribute("default").Value; }

				StackPanel optionContainer = new StackPanel();
				optionContainer.Orientation = Orientation.Horizontal;
				optionContainer.ToolTip = description;

				Label optionLbl = new Label();
				optionLbl.Content = label;
				optionContainer.Children.Add(optionLbl);

				if (guiType == "txtbox")
				{
					TextBox tb = new TextBox();
					tb.Text = defaultValue;
					optionContainer.Children.Add(tb);
				}

				guiStkPnl.Children.Add(optionContainer);
			}
		}
	}
}
