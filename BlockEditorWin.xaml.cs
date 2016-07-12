using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for BlockEditorWin.xaml
    /// </summary>
    public partial class BlockEditorWin : Window
    {
        private Block m_pBlkTarget;

        public BlockEditorWin(Block pBlock)
        {
            m_pBlkTarget = pBlock;
            InitializeComponent();
            txtbxName.Text = m_pBlkTarget.Graphic.Name;
            loadAlgorithmOptions(m_pBlkTarget.PyAlgorithm);
        }

        // TODO: figure out how to integrate/wire this up.  Maybe not necessary.
        // TODO: Make it look a little nicer
        private void loadAlgorithmOptions(PyAlgorithm algorithm)
        {
            StackPanel guiStkPnl = spnlAlgOptions;

            if (!algorithm.getOptions().ContainsKey("XML")) { return; /*No options, not necessarily an error*/ }

            string xml = algorithm.getOptions()["XML"];
            XElement optionsRoot = XElement.Parse(xml);
            //string xml = " <options> <option pythonkey='ColIndices' guitype='txtbox' label='Column Indices' description='Comma separated list of column indices to ignore' default='' /> <option pythonkey='NumOutputs' guitype='txtbox' label='# Outputs' description='Number of outputs…' default='1' /> </options> ";

            // iterate through each option and construct gui element for each
            IEnumerable<XElement> options = optionsRoot.Elements();
            //IEnumerable<XElement> options = parsedXML.Elements();
            foreach (XElement option in options)
            {
                string pythonKey = option.Attribute("pythonkey").Value;
                string guiType = option.Attribute("guitype").Value;
                string label = option.Attribute("label").Value;
                string description = option.Attribute("description") == null ? "" : option.Attribute("description").Value;
                string defaultValue = "";
                if (option.Attribute("default") != null) { defaultValue = option.Attribute("default").Value; }

                StackPanel optionContainer = new StackPanel();
                optionContainer.Orientation = Orientation.Horizontal;
                optionContainer.ToolTip = description;

                Label optionLbl = new Label();
                optionLbl.Content = label;
                optionContainer.Children.Add(optionLbl);

                switch(guiType)
                {
                        case "txtbox":
                        case "text_box":
                        {
                            TextBox tb = new TextBox();
                            tb.Text = defaultValue;
                            tb.Width = 180;
                            tb.ToolTip = description;
                            tb.Uid = pythonKey;
                            optionContainer.Children.Add(tb);
                            break;
                        }

                        case "check_box":
                        {
                            CheckBox cb = new CheckBox();
                            cb.ToolTip = description;
                            cb.Uid = pythonKey;
                            optionContainer.Children.Add(cb);
                            break;
                        }

                        case "file_chooser":
                        {
                            Canvas c = new Canvas();
                            TextBox txt = new TextBox();
                            txt.Text = Directory.GetCurrentDirectory();
                            txt.Width = 260;
                            txt.Height = 20;
                            txt.Margin = new Thickness(0, 0, 0, 0);
                            txt.HorizontalAlignment = HorizontalAlignment.Left;
                            txt.FontSize = 12;
                            txt.Uid = "txtfile";
                            c.Children.Add(txt);

                            Button btn = new Button();
                            btn.Content = "Select a File";
                            btn.HorizontalAlignment = HorizontalAlignment.Left;
                            btn.Width = 75;
                            btn.Height = 20;
                            btn.Margin = new Thickness(265, 0, 0, 0);
                            btn.Click += (s, e) => 
                            {
                                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                                if(result == System.Windows.Forms.DialogResult.OK)
                                {
                                    txt.Text = dialog.FileName;
                                }
                            };
                            c.Children.Add(btn);
                            c.Uid = pythonKey;
                            optionContainer.Children.Add(c);
                            break;
                        }

                        default:
                        {
                            Label error = new Label();
                            error.Content = "Invalid GUI Option!";
                            optionContainer.Children.Add(error);
                            break;
                        }
                }

                guiStkPnl.Children.Add(optionContainer);
            }
        }

        private void ConfirmEdits(object sender, RoutedEventArgs e)
        {
            if (spnlAlgOptions.Children.Count > 0)
            {
                StackPanel guiStkPnl = (StackPanel)spnlAlgOptions.Children[0];
                Dictionary<string, dynamic> finished = new Dictionary<string, dynamic>();
				
                PyAlgorithm pAlgorithm = m_pBlkTarget.PyAlgorithm;
				Dictionary<string, dynamic> pAlgOptions = pAlgorithm.getOptions();

                if (!pAlgOptions.ContainsKey("XML")) { return; /*ERROR*/ }

                string xml = pAlgorithm.getOptions()["XML"];
                XElement optionsRoot = XElement.Parse(xml);
                IEnumerable<XElement> options = optionsRoot.Elements();
                //IEnumerable<XElement> options = parsedXML.Elements();
                foreach (XElement option in options)
                {
                    string pythonKey = option.Attribute("pythonkey").Value;
                    string guiType = option.Attribute("guitype").Value;
                    string label = option.Attribute("label").Value;
                    string description = option.Attribute("description") == null ? "" : option.Attribute("description").Value;
                    string defaultValue = "";
                    if (option.Attribute("default") != null) { defaultValue = option.Attribute("default").Value; }

                    switch (guiType)
                    {
                        case "txtbox":
                        case "text_box":
                            {
                                //finished.Add(pythonKey, ((TextBox)getByName(guiStkPnl.Children, pythonKey)).Text);
								pAlgOptions[pythonKey] = ((TextBox)getByName(guiStkPnl.Children, pythonKey)).Text;
                                break;
                            }

                        case "check_box":
                            {
                                //finished.Add(pythonKey, ((CheckBox)getByName(guiStkPnl.Children, pythonKey)).IsChecked);
								pAlgOptions[pythonKey] = ((CheckBox)getByName(guiStkPnl.Children, pythonKey)).IsChecked;
                                break;
                            }

                        case "file_chooser":
                            {
                                //finished.Add(pythonKey, "\"" + (((TextBox)getByName(((Canvas)getByName(guiStkPnl.Children, pythonKey)).Children, "txtfile")).Text) + "\"");
								pAlgOptions[pythonKey] = "\"" + (((TextBox)getByName(((Canvas)getByName(guiStkPnl.Children, pythonKey)).Children, "txtfile")).Text) + "\"";
                                break;
                            }

                        default: { break; }
                    }
                }

                m_pBlkTarget.Graphic.Name = txtbxName.Text;
				//m_pBlkTarget.PyAlgorithm.setPreOptions(finished);
				m_pBlkTarget.PyAlgorithm.setOptions(pAlgOptions);
				
            }
            Close();
        }

        private dynamic getByName(UIElementCollection children, string pythonKey)
        {
            foreach(UIElement element in children)
            {
                if (element.Uid.Equals(pythonKey))
                    return element;
            }
            return null;
        }

        private void deleteBlock(object sender, RoutedEventArgs e)
        {
			//TODO Implement
			Master.removeBlock(m_pBlkTarget.ID);
            Close();
        }
    }
}
