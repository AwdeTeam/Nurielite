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
        private void loadAlgorithmOptions(PyAlgorithm pAlgorithm)
        {
            StackPanel guiStkPnl = spnlAlgOptions;
			guiStkPnl.HorizontalAlignment = HorizontalAlignment.Stretch;

            if (!pAlgorithm.getOptions().ContainsKey("XML")) { return; /*No options, not necessarily an error*/ }

            string sOptionsXml = pAlgorithm.getOptions()["XML"];
            XElement pRootOptionsElement = XElement.Parse(sOptionsXml);
            //string xml = " <options> <option pythonkey='ColIndices' guitype='txtbox' label='Column Indices' description='Comma separated list of column indices to ignore' default='' /> <option pythonkey='NumOutputs' guitype='txtbox' label='# Outputs' description='Number of outputs…' default='1' /> </options> ";

            // iterate through each option and construct gui element for each
            IEnumerable<XElement> pOptions = pRootOptionsElement.Elements();
            //IEnumerable<XElement> options = parsedXML.Elements();
            foreach (XElement pOption in pOptions)
            {
                string sPythonKey = pOption.Attribute("pythonkey").Value;
                string sGuiType = pOption.Attribute("guitype").Value;
                string sLabel = pOption.Attribute("label").Value;
                string sDescription = pOption.Attribute("description") == null ? "" : pOption.Attribute("description").Value;
                string sDefaultValue = "";
                if (pOption.Attribute("default") != null) { sDefaultValue = pOption.Attribute("default").Value; }

                StackPanel pOptionRow = new StackPanel();
                pOptionRow.Orientation = Orientation.Horizontal;
                pOptionRow.ToolTip = sDescription;

                Label optionLbl = new Label();
                optionLbl.Content = sLabel;
				optionLbl.Margin = new Thickness(5);
				optionLbl.Padding = new Thickness(2);
                pOptionRow.Children.Add(optionLbl);

                switch(sGuiType)
                {
					case "txtbox":
					case "text_box":
					case "literal_box":
					case "string_box":
					case "array_box":
						{
							TextBox tb = new TextBox();
							tb.Text = sDefaultValue;
							tb.Width = 180;
							tb.ToolTip = sDescription;
							tb.Uid = sPythonKey;
							tb.Margin = new Thickness(5);
							tb.Padding = new Thickness(2);
							pOptionRow.Children.Add(tb);
							break;
						}

					case "check_box":
						{
							CheckBox cb = new CheckBox();
							cb.ToolTip = sDescription;
							cb.Uid = sPythonKey;
							cb.Padding = new Thickness(2);
							cb.Margin = new Thickness(0,8,5,0);
							pOptionRow.Children.Add(cb);
							break;
						}

					case "file_chooser":
						{
							TextBox txt = new TextBox();
							txt.Text = Directory.GetCurrentDirectory();
							txt.Width = 260;
							txt.Height = 20;
							txt.Margin = new Thickness(5);
							txt.Padding = new Thickness(2);
							txt.HorizontalAlignment = HorizontalAlignment.Left;
							txt.FontSize = 12;
							txt.Uid = sPythonKey;
							pOptionRow.Children.Add(txt);

							Button btn = new Button();
							btn.Content = "Select a File";
							btn.HorizontalAlignment = HorizontalAlignment.Right;
							btn.Width = 75;
							btn.Height = 20;
							//btn.Margin = new Thickness(265, 0, 0, 0);
							btn.Click += (s, e) =>
							{
								System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
								dialog.InitialDirectory = Directory.GetCurrentDirectory();
								System.Windows.Forms.DialogResult result = dialog.ShowDialog();
								if (result == System.Windows.Forms.DialogResult.OK)
								{
									txt.Text = dialog.FileName;
								}
							};
							pOptionRow.Children.Add(btn);
							break;
						}

					default:
						{
							Label error = new Label();
							error.Content = "Invalid GUI Option!";
							pOptionRow.Children.Add(error);
							break;
						}
				}

				pOptionRow.HorizontalAlignment = HorizontalAlignment.Stretch;
				guiStkPnl.Children.Add(pOptionRow);
			}
		}

        private void ConfirmEdits(object sender, RoutedEventArgs e)
        {
            if (spnlAlgOptions.Children.Count > 0)
            {
                StackPanel guiStkPnl = spnlAlgOptions;
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
                        case "literal_box":
                            {
								pAlgOptions[pythonKey] = ((TextBox)getByName(guiStkPnl.Children, pythonKey)).Text;
                                break;
                            }

                        case "string_box":
                            {
                                pAlgOptions[pythonKey] = "\"" + ((TextBox)getByName(guiStkPnl.Children, pythonKey)).Text + "\"";
                                break;
                            }
							
						case "array_box":
							{
								string sArrayString = ((TextBox)getByName(guiStkPnl.Children, pythonKey)).Text;
								List<string> pArrayParts = sArrayString.Split(',').ToList();
								List<int> pArrayInts = new List<int>();

								// find any parts with a dash (indicating range) and replace with the inbetween numbers
								for (int i = 0; i < pArrayParts.Count; i++)
								{
									string sPart = pArrayParts[i];
									if (sPart.Contains("-"))
									{
										// find range boundary numbers 
										int iStart = Convert.ToInt32(sPart.Substring(0, sPart.IndexOf("-")));
										int iEnd = Convert.ToInt32(sPart.Substring(sPart.IndexOf("-") + 1));
										if (iEnd < iStart) { throw new Exception("End of range cannot be lower than start"); }

										// add boundaries and numbers in between
										pArrayInts.Add(iStart);
										for (int j = iStart + 1; j < iEnd; j++) { pArrayInts.Add(j); }
										pArrayInts.Add(iEnd);
									}
									else { pArrayInts.Add(Convert.ToInt32(sPart)); }
								}

								// assign option
								pAlgOptions[pythonKey] = pArrayInts;
								break;	
							}

                        case "check_box":
                            {
								pAlgOptions[pythonKey] = ((CheckBox)getByName(guiStkPnl.Children, pythonKey)).IsChecked.ToString();
                                break;
                            }

                        case "file_chooser":
                            {
                                pAlgOptions[pythonKey] = "\"" + ((TextBox)getByName(guiStkPnl.Children, pythonKey)).Text.Replace("\\", "/") + "\"";
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
            foreach(StackPanel stkpanel in children)
            {
                foreach(UIElement element in stkpanel.Children)
                {
                    if (element.Uid.Equals(pythonKey))
                        return element;
                }
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
