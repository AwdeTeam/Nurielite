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
using System.Windows.Navigation;
using System.Windows.Shapes;

// TODO: can have input file representations?


// NOTE: IF AN ELEMENT HAS A TRANSPARENT FILL (hasn't been assigned) IT CAN BE CLICKED THROUGH AND WILL NOT REGISTER EVENTS

namespace Nurielite
{
	public partial class MainWindow : Window
	{
		// member variables
		private bool m_bIsTyping = false; // meant to counteract window auto focusing textbox even if already typing there
		private bool m_bIsTypingNotAvailable = false; // set to true if need to type elsewhere so doesn't auto put cursor in console when start to type

		private List<string> m_lCommandHistory = new List<string>();
		private int m_iCommandIndex = 0; // keeps track of where in command history you are

		private GraphicContainer m_pGraphicContainer = new GraphicContainer();

		// construction
		public MainWindow()
		{
			InitializeComponent();
            btnToggleVerbose.Content = "Verbose: " + ((Master.VerboseMode) ? "\u2713" : "\u274C");
			cmd_clearConsole();
			Master.assignWindow(this);

			log("Program initialized!");

			// canvas initially wasn't handling events properly, so adding them to window instead
			this.MouseMove += world_MouseMove;

			// set path 
			Directory.SetCurrentDirectory(Master.PATH_TO_THETHING);

			testInputBlock();            //0
            testMaskBlock();             //1
            testMaskBlock();             //2
            testBayesBlock();            //3
            testTrainerOutputBlock();    //4

            int iY = 0;
            int iX = 100;
            int sy = 150; // sy??? as in sigh?? what is this magic number?
            int sx = 100;

			Master.Blocks[0].Graphic.move(iX -  0, iY + 0*sy);
			Master.Blocks[1].Graphic.move(iX - sx, iY + 1*sy);
			Master.Blocks[2].Graphic.move(iX + sx, iY + 1*sy);
            Master.Blocks[3].Graphic.move(iX - sx, iY + 2*sy);
            Master.Blocks[4].Graphic.move(iX -  0, iY + 3*sy);

			Master.Blocks[0].connectTo(Master.Blocks[1], 0);
			Master.Blocks[0].connectTo(Master.Blocks[2], 0);
			Master.Blocks[1].connectTo(Master.Blocks[3], 0);
            Master.Blocks[3].connectTo(Master.Blocks[4], 0);
            Master.Blocks[2].connectTo(Master.Blocks[4], 1);

            log("Testing Blocks Loaded");
		}

		public void testInputBlock() { AlgorithmLoader.loadAlgorithmBlock("FileInput", AlgorithmType.Input, 0, 1); }
		public void testOutputBlock() { AlgorithmLoader.loadAlgorithmBlock("FileOutput", AlgorithmType.Output, 1, 0); }
        public void testTrainerOutputBlock() { AlgorithmLoader.loadAlgorithmBlock("TrainerOutput", AlgorithmType.Output, 2, 0); }
		public void testOpBlock() { AlgorithmLoader.loadAlgorithmBlock("ScalarAdd", AlgorithmType.Operation, 1, 1); }
        public void testJoinBlock() { AlgorithmLoader.loadAlgorithmBlock("SimpleJoin", AlgorithmType.Operation, 2, 1); }
        public void testMaskBlock() { AlgorithmLoader.loadAlgorithmBlock("Mask", AlgorithmType.Operation, 1, 1); }
        public void testBayesBlock() { AlgorithmLoader.loadAlgorithmBlock("NaiveBayes", AlgorithmType.Classifier, 1, 1); }
		
		// properties
		public Canvas MainCanvas { get { return world; } }
		public GraphicContainer getGraphicContainer() { return m_pGraphicContainer; }
		
		// ------------------------------------
		//  EVENT HANDLERS
		// ------------------------------------

        private void Button_Click_addNode(object sender, RoutedEventArgs e)
        {
            RepDesignerWin pPopup = new RepDesignerWin(this);
            pPopup.Show();
        }

        private void Button_Click_generatePython(object sender, RoutedEventArgs e)
        {
            Master.log(" ");
            InterGraph pGraph = new InterGraph();

			//find first input node
			Block pFirstBlock = null;
			foreach (Block pBlock in Master.Blocks)
			{
				if (pBlock.Family == AlgorithmType.Input) { pFirstBlock = pBlock; break; }
			}
			if (pFirstBlock == null) { throw new Exception("THERE'S NO INPUT NODES."); }

			new InterNode(pFirstBlock, pGraph);
			
            List<PyAlgorithm> lAlgs = pGraph.topoSort();
            if (Master.VerboseMode) log("Count: " + lAlgs.Count);
			
            (new PythonGenerator()).generatePythonCode(lAlgs, Master.PATH_TO_THETHING, "COMPILED");
        }

        private void CheckBox_VerboseToggle(object sender, RoutedEventArgs e)
        {
            Master.VerboseMode = !Master.VerboseMode;
            btnToggleVerbose.Content = "Verbose: " + ((Master.VerboseMode) ? "\u2713" : "\u274C");
        }

		// If user starts typing (and wasn't typing in some other field), put cursor in command line bar
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_bIsTypingNotAvailable) { return; }
			if (m_bIsTyping) { return; }

			m_bIsTyping = true;
			txtConsoleCommand.Focus();
			txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; // move cursor to end of line
		}

		// NOTE: technically a PreviewKeyDown event, so that it also registers arrow keys
		private void txtConsoleCommand_KeyDown(object sender, KeyEventArgs e)
		{
			//log("Key was pressed"); // DEBUG
			if (e.Key == Key.Enter) { enterConsoleCommand(); }
			else if (e.Key == Key.Up || e.Key == Key.Down) // scroll command history
			{
				if (e.Key == Key.Up) { m_iCommandIndex--; }
				else { m_iCommandIndex++; }
				if (m_iCommandIndex < 0 || m_iCommandIndex >= m_lCommandHistory.Count) { txtConsoleCommand.Text = ""; }
				else { txtConsoleCommand.Text = m_lCommandHistory[m_iCommandIndex]; }
			}
			else if (e.Key == Key.Escape) { txtConsoleCommand.Text = ""; }
		}

		private void txtConsoleCommand_LostFocus(object sender, RoutedEventArgs e)
		{
			m_bIsTyping = false;
		}

		// technically window_mousemove
		private void world_MouseMove(object sender, MouseEventArgs e) { m_pGraphicContainer.evt_MouseMove(sender, e); }
		private void world_MouseDown(object sender, MouseButtonEventArgs e) { m_pGraphicContainer.evt_MouseDown(sender, e); }
		private void world_MouseUp(object sender, MouseButtonEventArgs e) { m_pGraphicContainer.evt_MouseUp(sender, e); }


		// ------------------------------------
		//  FUNCTIONS
		// ------------------------------------

		public void setCommandPrompt(string sPrompt) { txtConsoleCommand.Text = sPrompt; txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; }

		public void log(string sMessage) { log(sMessage, Colors.DarkCyan); }
		public void log(string sMessage, Color pColor) 
		{
			Run pRun = new Run(sMessage);
			pRun.Foreground = new SolidColorBrush(pColor);
			lblConsole.Document.Blocks.Add(new Paragraph(pRun));
			lblConsole.ScrollToEnd();
		}
		

		// when user hits enter in the console bar
		private void enterConsoleCommand()
		{
			// first get command
			string sCommand = txtConsoleCommand.Text;
			txtConsoleCommand.Text = "";

			// add to console
			log("> " + sCommand, Colors.White);
			txtConsoleCommand.Focus(); // make sure command line retains focus

			// add to command history
			m_lCommandHistory.Add(sCommand);
			m_iCommandIndex = m_lCommandHistory.Count;

			// check for and run command
			try { parseCommand(sCommand); }
			catch (Exception e)
			{
				log(">>> COMMAND PARSING FAILED", Colors.Red);
				log(e.Message, Colors.Red);
			}
		}

		// take a command string and figure out what's what (USE THIS FOR HANDLING COMMAND SCRIPTS)
		// NOTE: this function is NOT setup to handle more than one set of quotes in an entire command
		// (essentially adds every space-delimited word to a prelist, then selectively adds to actual word
		//   list, combining multiple into one 'word' if quotes were detected)
		private void parseCommand(string command)
		{
			List<string> lPrePassWords = new List<string>(); // used for handling quoted text
			List<string> lWords = new List<string>();

			List<string> lKeys = new List<string>();
			List<string> lVals = new List<string>();

			// get all words within quotes and condense
			lPrePassWords = command.Split(' ').ToList();
			for (int i = 0; i < lPrePassWords.Count; i++)
			{
				string sWord = lPrePassWords[i];
				if (sWord.Contains("\""))
				{
					// check for just one word in quotes
					if (sWord.Substring(sWord.IndexOf("\"") + 1).Contains("\"")) { sWord = sWord.Replace("\"", ""); lWords.Add(sWord); continue; }

					// go through and append into single 'word' until hit another quote
					string sNextWord = lPrePassWords[i + 1];
					do
					{
						i++;
						sNextWord = lPrePassWords[i];
						sWord += " " + sNextWord;
					}
					while (!sNextWord.Contains("\""));

					// remove both quotes
					sWord = sWord.Replace("\"","");
				}
				lWords.Add(sWord);
				//log("adding " + word, Colors.Gray); // DEBUG
			}

			// separate into keys and vals
			foreach (string sWord in lWords)
			{
				if (sWord.Length == 0) { return; }
				if (sWord[0] == '-') { lVals.Add(sWord.Substring(1)); }
				else { lKeys.Add(sWord); }
			}
			try { handleCommand(lKeys, lVals); }
			catch (Exception e) 
			{ 
				log(">>> COMMAND FAILED", Colors.Red);
				log(e.Message, Colors.Red);
			}
		}

		// NOTE: rough command syntax convention: [VERB] [NOUN] [-VALSLIST]
		// NOTE: No need to make basic validation checks for if keys[1] exists etc, try catch in parseCommand will handle
		// based on parsed command, figure out what function to execute
		private void handleCommand(List<string> lKeys, List<string> lVals)
		{
			if (lKeys[0] == "exit" || lKeys[0] == "quit") { cmd_exit(); }
			else if (lKeys[0] == "help") { cmd_printHelp(); }
			else if (lKeys[0] == "clear" || lKeys[0] == "cls") { cmd_clearConsole(); }
			/*else if (keys[0] == "add")
			{
				
				else if (keys[1] == "rep" || keys[1] == "representation")
				{
					List<string> args = vals[0].Split(',').ToList();
					int ins = Int32.Parse(args[0]);
					int outs = Int32.Parse(args[1]);
					addRep(ins, outs);
				}
			}*/
			else if (lKeys[0] == "edit")
			{
				if (lKeys[1] == "rep" || lKeys[1] == "representation")
				{
					int iID = Int32.Parse(lVals[0]);
					string sAttribute = lVals[1];
					string sValue = lVals[2];

					Block pBlock = Master.Blocks[iID];

					if (sAttribute == "lbl") 
					{
						pBlock.Name = sValue;
						log("Updated block label to '" + sValue + "'");
					}
					else if (sAttribute == "color") 
					{
                        pBlock.Graphic.BaseColor = ((SolidColorBrush)(new BrushConverter().ConvertFrom("#" + sValue))).Color; 
						log("Updated block color to #" + sValue);
					}
				}
			}
            else if(lKeys[0] == "list")
            {
                if(lKeys.Count == 0)
                {
                    log("hmm");
                    lKeys.Add("");
                }

                switch (lKeys[1])
                {
                    case "blk":
                    case "blocks":
                        log(listBlocks());
                        break;
                    case "":
                    default:
                        log("Please enter a type! (rep or dt)", Colors.Red);
                        break;
                }
            }
           
		}

        private string listBlocks()
        {
            string sBlockString = "";
            for(int i = 0; i < Master.Blocks.Count; i++)
            {
                sBlockString += "\t" + Master.Blocks[i].ID + ")";
                sBlockString += Master.Blocks[i].Name + ", ";
                //r += Master.Blocks[i].Family + ": " + Master.Blocks[i].AlgorithmName + "\n";
            }
            return sBlockString;
        }


		// ------------------------------------
		//  COMMAND FUNCTIONS
		// ------------------------------------

		private void cmd_exit() { Application.Current.Shutdown(); }
		private void cmd_printHelp()
		{
			log("exit | quit", Colors.Yellow);
			log("clear | cls \t// clears console", Colors.Yellow);
			log("help", Colors.Yellow);
            log("list (rep, dt)", Colors.Yellow);
			log("add rect[angle] -[x] -[y] -[width] -[height]\add rect[angle] -[x],[y],[width],[height]", Colors.Yellow);
			log("add rep[resentation] -[numInputs],[numOutputs]", Colors.Yellow);
			log("edit rep[resentation] -[id] -[attr] -[value]\n\tattr: color, lbl", Colors.Yellow);
		}
		private void cmd_clearConsole() { lblConsole.Document.Blocks.Clear(); }
    }
}
