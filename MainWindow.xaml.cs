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
		private bool m_isTyping = false; // meant to counteract window auto focusing textbox even if already typing there
		private bool m_isTypingNotAvail = false; // set to true if need to type elsewhere so doesn't auto put cursor in console when start to type

		private List<string> m_commandHistory = new List<string>();
		private int m_commandIndex = 0; // keeps track of where in command history you are

		private GraphicContainer m_gc = new GraphicContainer();

		// construction
		public MainWindow()
		{

		
			InitializeComponent();
            btnToggleVerbose.Content = "Verbose: " + ((Master.VerboseMode) ? "\u2713" : "\u274C");
			cmd_clearConsole();
			Master.assignWindow(this);

			log("Program initialized!");
            //Datatype.genericTypes();
            //log("Datatypes Loaded!");

			// canvas initially wasn't handling events properly, so adding them to window instead
			this.MouseMove += world_MouseMove;

			// set path 
			Directory.SetCurrentDirectory(Master.PATH_TO_THETHING);

			testInputBlock();
            testBayesBlock();
            testTrainerOutputBlock();

			Master.Blocks[0].Graphic.move(300, 10);
			Master.Blocks[1].Graphic.move(100, 120);
			Master.Blocks[2].Graphic.move(250, 250);

			Master.Blocks[0].connectTo(Master.Blocks[1], 0);
			Master.Blocks[1].connectTo(Master.Blocks[2], 0);
			Master.Blocks[0].connectTo(Master.Blocks[2], 1);

            log("Testing Blocks Loaded");
		}

		public void testInputBlock()
		{
			AlgorithmLoader.loadAlgorithmBlock("FileInput", AlgorithmType.Input, 0, 1);
		}

		public void testOutputBlock()
		{
			AlgorithmLoader.loadAlgorithmBlock("FileOutput", AlgorithmType.Output, 1, 0);
		}

        public void testTrainerOutputBlock()
        {
            AlgorithmLoader.loadAlgorithmBlock("TrainerOutput", AlgorithmType.Output, 2, 0);
        }

		public void testOpBlock()
		{
			AlgorithmLoader.loadAlgorithmBlock("ScalarAdd", AlgorithmType.Operation, 1, 1);
		}

        public void testJoinBlock()
        {
            AlgorithmLoader.loadAlgorithmBlock("SimpleJoin", AlgorithmType.Operation, 2, 1);
        }

        public void testBayesBlock()
        {
            AlgorithmLoader.loadAlgorithmBlock("NaiveBayes", AlgorithmType.Classifier, 1, 1);
        }
		
        //END STUFF

		// properties
		public Canvas MainCanvas { get { return world; } }
		public GraphicContainer getGraphicContainer() { return m_gc; }
		
		// ------------------------------------
		//  EVENT HANDLERS
		// ------------------------------------

        private void Button_Click_addNode(object sender, RoutedEventArgs e)
        {
            RepDesignerWin popup = new RepDesignerWin(this);
            popup.Show();
        }

        private void Button_Click_addType(object sender, RoutedEventArgs e)
        {
            DatatypeDesigner popup = new DatatypeDesigner();
            popup.Show();
        }

        private void Button_Click_generatePython(object sender, RoutedEventArgs e)
        {
            Master.log(" ");
            InterGraph graph = new InterGraph();

			//find first input node
			Block pFirstBlock = null;
			foreach (Block pBlock in Master.Blocks)
			{
				if (pBlock.Family == AlgorithmType.Input) { pFirstBlock = pBlock; break; }
			}
			if (pFirstBlock == null) { throw new Exception("THERE'S NO INPUT NODES."); }

			new InterNode(pFirstBlock, graph);
			
            List<PyAlgorithm> algs = graph.topoSort();
            if (Master.VerboseMode) log("Count: " + algs.Count);
			
            (new PythonGenerator()).generatePythonCode(algs, Master.PATH_TO_THETHING, "COMPILED");
        }

        private void CheckBox_VerboseToggle(object sender, RoutedEventArgs e)
        {
            Master.VerboseMode = !Master.VerboseMode;
            btnToggleVerbose.Content = "Verbose: " + ((Master.VerboseMode) ? "\u2713" : "\u274C");
        }

		// If user starts typing (and wasn't typing in some other field), put cursor in command line bar
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_isTypingNotAvail) { return; }
			if (m_isTyping) { return; }
            //if (Popup1.IsOpen) { return; }

			m_isTyping = true;
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
				if (e.Key == Key.Up) { m_commandIndex--; }
				else { m_commandIndex++; }
				if (m_commandIndex < 0 || m_commandIndex >= m_commandHistory.Count) { txtConsoleCommand.Text = ""; }
				else { txtConsoleCommand.Text = m_commandHistory[m_commandIndex]; }
			}
			else if (e.Key == Key.Escape) { txtConsoleCommand.Text = ""; }
		}

		private void txtConsoleCommand_LostFocus(object sender, RoutedEventArgs e)
		{
			m_isTyping = false;
		}

		// technically window_mousemove
		private void world_MouseMove(object sender, MouseEventArgs e) { m_gc.evt_MouseMove(sender, e); }
		private void world_MouseDown(object sender, MouseButtonEventArgs e) { m_gc.evt_MouseDown(sender, e); }
		private void world_MouseUp(object sender, MouseButtonEventArgs e) { m_gc.evt_MouseUp(sender, e); }


		// ------------------------------------
		//  FUNCTIONS
		// ------------------------------------

		public void setCommandPrompt(string prompt) { txtConsoleCommand.Text = prompt; txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; }

		public void log(string message) { log(message, Colors.DarkCyan); }
		public void log(string message, Color color) 
		{
			Run r = new Run(message);
			r.Foreground = new SolidColorBrush(color);
			lblConsole.Document.Blocks.Add(new Paragraph(r));
			lblConsole.ScrollToEnd();
		}
		

		// when user hits enter in the console bar
		private void enterConsoleCommand()
		{
			// first get command
			string command = txtConsoleCommand.Text;
			txtConsoleCommand.Text = "";

			// add to console
			log("> " + command, Colors.White);
			txtConsoleCommand.Focus(); // make sure command line retains focus

			// add to command history
			m_commandHistory.Add(command);
			m_commandIndex = m_commandHistory.Count;

			// check for and run command
			try { parseCommand(command); }
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
			List<string> prePassWords = new List<string>(); // used for handling quoted text
			List<string> words = new List<string>();

			List<string> keys = new List<string>();
			List<string> vals = new List<string>();

			// get all words within quotes and condense
			prePassWords = command.Split(' ').ToList();
			for (int i = 0; i < prePassWords.Count; i++)
			{
				string word = prePassWords[i];
				if (word.Contains("\""))
				{
					// check for just one word in quotes
					if (word.Substring(word.IndexOf("\"") + 1).Contains("\"")) { word = word.Replace("\"", ""); words.Add(word); continue; }

					// go through and append into single 'word' until hit another quote
					string nextWord = prePassWords[i + 1];
					do
					{
						i++;
						nextWord = prePassWords[i];
						word += " " + nextWord;
					}
					while (!nextWord.Contains("\""));

					// remove both quotes
					word = word.Replace("\"","");
				}
				words.Add(word);
				//log("adding " + word, Colors.Gray); // DEBUG
			}

			// separate into keys and vals
			foreach (string word in words)
			{
				if (word.Length == 0) { return; }
				if (word[0] == '-') { vals.Add(word.Substring(1)); }
				else { keys.Add(word); }
			}
			try { handleCommand(keys, vals); }
			catch (Exception e) 
			{ 
				log(">>> COMMAND FAILED", Colors.Red);
				log(e.Message, Colors.Red);
			}
		}

		// NOTE: rough command syntax convention: [VERB] [NOUN] [-VALSLIST]
		// NOTE: No need to make basic validation checks for if keys[1] exists etc, try catch in parseCommand will handle
		// based on parsed command, figure out what function to execute
		private void handleCommand(List<string> keys, List<string> vals)
		{
			if (keys[0] == "exit" || keys[0] == "quit") { cmd_exit(); }
			else if (keys[0] == "help") { cmd_printHelp(); }
			else if (keys[0] == "clear" || keys[0] == "cls") { cmd_clearConsole(); }
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
			else if (keys[0] == "edit")
			{
				if (keys[1] == "rep" || keys[1] == "representation")
				{
					int id = Int32.Parse(vals[0]);
					string attr = vals[1];
					string val = vals[2];

					Block r = Master.Blocks[id];

					if (attr == "lbl") 
					{
						r.Name = val;
						log("Updated block label to '" + val + "'");
					}
					else if (attr == "color") 
					{
                        r.Graphic.BaseColor = ((SolidColorBrush)(new BrushConverter().ConvertFrom("#" + val))).Color; 
						log("Updated block color to #" + val);
					}
				}
			}
            else if(keys[0] == "list")
            {
                if(keys.Count == 0)
                {
                    log("hmm");
                    keys.Add("");
                }

                switch (keys[1])
                {
                    case "blk":
                    case "blocks":
                        log(listBlocks());
                        break;
                    case "dt":
                    case "datatypes":
                        log(listDatatypes());
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
            string r = "";
            for(int i = 0; i < Master.Blocks.Count; i++)
            {
                r += "\t" + Master.Blocks[i].ID + ")";
                r += Master.Blocks[i].Name + ", ";
                //r += Master.Blocks[i].Family + ": " + Master.Blocks[i].AlgorithmName + "\n";
            }
            return r;
        }

        private string listDatatypes()
        {
            string r = "";
			/*for(int i = 0; i < Datatype.numberOfTypes(); i++)
            {
                //r += "\t" + i + ")" + Datatype.getType(i).stringRep() + "\n";
            }*/
            return r;
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
