﻿using System;
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
            Representation newrep = AlgorithmLoader.generateRepresentation(txtNameInput.Text, cmbAlgorithmType.SelectedIndex,
                    ray(lstInputs.SelectedItems), ray(lstOutputs.SelectedItems) );
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
            cmbAlgorithmType.ItemsSource = Representation.ALGORITHM_TYPES.ToList();
            cmbAlgorithmType.SelectedIndex = 0;
        }

        private void cmbAlgorithmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbAlgorithmType.SelectedItem.Equals("input"))
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

            if(!((cmbAlgorithmType.SelectedItem.Equals("input"))||(cmbAlgorithmType.SelectedItem.Equals("output"))))
            {
                lstOutputs.IsEnabled = true;
                lstInputs.IsEnabled = true;
            }

            switch(cmbAlgorithmType.SelectedItem.ToString())
            {
                case "operation":
                    lblType.Content = "operation:";
                    cmbAlgorithmSpecific.IsEnabled = true;
                    cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType("operation");
                    break;
                case "classifier":
                    lblType.Content = "classifier:";
                    cmbAlgorithmSpecific.IsEnabled = true;
                    cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType("classifier");
                    break;
                case "clustering":
                    lblType.Content = "clustering:";
                    cmbAlgorithmSpecific.IsEnabled = true;
                    cmbAlgorithmSpecific.ItemsSource = PythonGenerator.getAllOfType("clustering");
                    break;
                case "input":
                    lblType.Content = "input";
                    cmbAlgorithmSpecific.IsEnabled = false;
                    break;
                case "output":
                    lblType.Content = "output";
                    cmbAlgorithmSpecific.IsEnabled = false;
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

        private static Datatype[] getSelectedTypes(ListBox box)
        {
            //List dexes = box.SelectedIndices;
            return null;
        }
    }
}
