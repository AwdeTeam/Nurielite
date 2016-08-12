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
            StackPanel pGuiStackPanel = spnlAlgOptions;
			pGuiStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            if (!pAlgorithm.getOptions().ContainsKey("XML")) { return; /*No options, not necessarily an error*/ }

            string sOptionsXml = pAlgorithm.getOptions()["XML"];
            XElement pRootOptionsElement = XElement.Parse(sOptionsXml);

            // iterate through each option and construct gui element for each
            IEnumerable<XElement> pOptions = pRootOptionsElement.Elements();
            foreach (XElement pOption in pOptions)
            {
				// get the important data from the xml element
                string sPythonKey = pOption.Attribute("pythonkey").Value;
                string sGuiType = pOption.Attribute("guitype").Value;
                string sLabel = pOption.Attribute("label").Value;
                string sDescription = pOption.Attribute("description") == null ? "" : pOption.Attribute("description").Value;
                string sDefaultValue = "";
                if (pOption.Attribute("default") != null) { sDefaultValue = pOption.Attribute("default").Value; }

				// create a new stackpanel for this options "row"
                StackPanel pOptionRow = new StackPanel();
                pOptionRow.Orientation = Orientation.Horizontal;
                pOptionRow.ToolTip = sDescription;

				// create the label for this option
                Label pOptionLabel = new Label();
                pOptionLabel.Content = sLabel;
				pOptionLabel.Margin = new Thickness(5);
				pOptionLabel.Padding = new Thickness(2);
                pOptionRow.Children.Add(pOptionLabel);

				switch (sGuiType)
				{
					case "txtbox":
					case "text_box":
					case "literal_box":
					case "string_box":
					case "array_box":
						{
							TextBox pTextBox = new TextBox();
							pTextBox.Text = sDefaultValue;
							pTextBox.Width = 180;
							pTextBox.ToolTip = sDescription;
							pTextBox.Uid = sPythonKey;
							pTextBox.Margin = new Thickness(5);
							pTextBox.Padding = new Thickness(2);
							pOptionRow.Children.Add(pTextBox);
							break;
						}
					case "check_box":
						{
							CheckBox pCheckBox = new CheckBox();
							pCheckBox.ToolTip = sDescription;
							pCheckBox.Uid = sPythonKey;
							pCheckBox.Padding = new Thickness(2);
							pCheckBox.Margin = new Thickness(0, 8, 5, 0);
							pOptionRow.Children.Add(pCheckBox);
							break;
						}
					case "file_chooser":
						{
							TextBox pTextBox = new TextBox();
							pTextBox.Text = Directory.GetCurrentDirectory();
							pTextBox.Width = 260;
							pTextBox.Height = 20;
							pTextBox.Margin = new Thickness(5);
							pTextBox.Padding = new Thickness(2);
							pTextBox.HorizontalAlignment = HorizontalAlignment.Left;
							pTextBox.FontSize = 12;
							pTextBox.Uid = sPythonKey;
							pOptionRow.Children.Add(pTextBox);

							Button pButton = new Button();
							pButton.Content = "Select a File";
							pButton.HorizontalAlignment = HorizontalAlignment.Right;
							pButton.Width = 75;
							pButton.Height = 20;
							pButton.Click += (s, e) =>
							{
								System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
								dialog.InitialDirectory = Directory.GetCurrentDirectory();
								System.Windows.Forms.DialogResult result = dialog.ShowDialog();
								if (result == System.Windows.Forms.DialogResult.OK)
								{
									pTextBox.Text = dialog.FileName;
								}
							};
							pOptionRow.Children.Add(pButton);
							break;
						}
				default:
						{
							Label pErrorLabel = new Label();
							pErrorLabel.Content = "Invalid GUI Option!";
							pOptionRow.Children.Add(pErrorLabel);
							break;
						}
				}

				// add this whole option row stackpanel to the stackpanel in the GUI 
				pOptionRow.HorizontalAlignment = HorizontalAlignment.Stretch;
				pGuiStackPanel.Children.Add(pOptionRow);
			}
		}

		// TODO: CODE DUPLICATION. CODE DUPLICATION EVERYWHERE. I CAN HAZ FIX PLZ?
        private void ConfirmEdits(object sender, RoutedEventArgs e)
        {
            if (spnlAlgOptions.Children.Count > 0)
            {
                StackPanel pGuiStackPanel = spnlAlgOptions;
				
				// get the options from the python algorithm interface
                PyAlgorithm pAlgorithm = m_pBlkTarget.PyAlgorithm;
				Dictionary<string, dynamic> dAlgOptions = pAlgorithm.getOptions();

                if (!dAlgOptions.ContainsKey("XML")) { return; /*ERROR*/ }

				// get the options xml so we know the keys to set in the dictionary
                string sRawOptionsXml = pAlgorithm.getOptions()["XML"];
                XElement pOptionsRoot = XElement.Parse(sRawOptionsXml);
                IEnumerable<XElement> pOptions = pOptionsRoot.Elements();
                foreach (XElement pOption in pOptions)
                {
					// get important information
                    string sPythonKey = pOption.Attribute("pythonkey").Value; // THIS IS THE KEY TO SET
                    string sGuiType = pOption.Attribute("guitype").Value;
                    string sLabel = pOption.Attribute("label").Value;
                    string sDescription = pOption.Attribute("description") == null ? "" : pOption.Attribute("description").Value;
                    string sDefaultValue = "";
                    if (pOption.Attribute("default") != null) { sDefaultValue = pOption.Attribute("default").Value; }

                    switch (sGuiType)
                    {
                        case "txtbox":
                        case "text_box":
                        case "literal_box":
                            {
								dAlgOptions[sPythonKey] = ((TextBox)getByName(pGuiStackPanel.Children, sPythonKey)).Text;
                                break;
                            }
                        case "string_box":
                            {
                                dAlgOptions[sPythonKey] = "\"" + ((TextBox)getByName(pGuiStackPanel.Children, sPythonKey)).Text + "\"";
                                break;
                            }
						case "array_box":
							{
								string sArrayString = ((TextBox)getByName(pGuiStackPanel.Children, sPythonKey)).Text;
								List<string> lArrayParts = sArrayString.Split(',').ToList();
								List<int> lArrayInts = new List<int>();

                                if(sArrayString == "")
                                {
                                    dAlgOptions[sPythonKey] = "[]";
                                    break;
                                }

								// find any parts with a dash (indicating range) and replace with the in-between numbers
								for (int i = 0; i < lArrayParts.Count; i++)
								{
									string sPart = lArrayParts[i];
									if (sPart.Contains("-"))
									{
										// find range boundary numbers 
										int iStart = Convert.ToInt32(sPart.Substring(0, sPart.IndexOf("-")));
										int iEnd = Convert.ToInt32(sPart.Substring(sPart.IndexOf("-") + 1));
										if (iEnd < iStart) { throw new Exception("End of range cannot be lower than start"); }

										// add boundaries and numbers in between
										lArrayInts.Add(iStart);
										for (int j = iStart + 1; j < iEnd; j++) { lArrayInts.Add(j); }
										lArrayInts.Add(iEnd);
									}
									else { lArrayInts.Add(Convert.ToInt32(sPart)); }
								}

								// add in the array syntax stuff
                                string sArray = "";
                                for (int i = 0; i < lArrayInts.Count; i++)
                                {
                                    if (i == 0)
                                        sArray += "[";
                                    else if (i < lArrayInts.Count)
                                        sArray += ",";

                                    sArray += lArrayInts[i];

                                    if (i == lArrayInts.Count - 1)
                                        sArray += "]";
                                }

                                // assign option
                                dAlgOptions[sPythonKey] = sArray;
								break;	
							}
                        case "check_box":
                            {
								dAlgOptions[sPythonKey] = ((CheckBox)getByName(pGuiStackPanel.Children, sPythonKey)).IsChecked.ToString();
                                break;
                            }
                        case "file_chooser":
                            {
                                dAlgOptions[sPythonKey] = "\"" + ((TextBox)getByName(pGuiStackPanel.Children, sPythonKey)).Text.Replace("\\", "/") + "\"";
                                break;
                            }
                        default: { break; }
                    }
                }

				// finalize things
                m_pBlkTarget.Graphic.Name = txtbxName.Text;
				m_pBlkTarget.PyAlgorithm.setOptions(dAlgOptions);
				
            }
            Close();
        }

        private dynamic getByName(UIElementCollection pChildren, string sPythonKey)
        {
            foreach(StackPanel pStackPanel in pChildren)
            {
                foreach(UIElement pElement in pStackPanel.Children)
                {
                    if (pElement.Uid.Equals(sPythonKey))
                        return pElement;
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
