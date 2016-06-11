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

		// hashmap is probably unnecessary, but I actually understand how they work now, so yeah ^_^
		private Dictionary<int, Block> m_blocks = new Dictionary<int, Block>();

		private GraphicContainer m_gc = new GraphicContainer();

		// construction
		public MainWindow()
		{
			InitializeComponent();
			cmd_clearConsole();
			Master.assignWindow(this);

			log("Program initialized!");
            Datatype.genericTypes();
            log("Datatypes Loaded!");

            /*Representation r = new AlgorithmRepresentation(2, 3);
            m_representations.Add(r.getID(), r);
			parseCommand("edit rep -1 -color -ff0000");*/
			
			// canvas initially wasn't handling events properly, so adding them to window instead
			this.MouseMove += world_MouseMove;

            /*START STUFF

			// python generator testing
			log("----PYTHON----");
			PythonGenerator gen = new PythonGenerator();

			PyAlgorithm testAlg = gen.loadPythonAlgorithm("../../OperationTest.py");
			Dictionary<string, dynamic> stuffs = testAlg.getOptions();
			foreach (string key in stuffs.Keys)
			{
				log("PYTHON ALG OPTION: {" + key + ":" + stuffs[key] + "}");
			}

			// unsure if not enforcing metadata types is a good idea....
			Dictionary<string, string> metaData = testAlg.getMetaData();
			log("Algorithm name: " + metaData["Name"]);
			log("Algorithm creator: " + metaData["Creator"]);
			log("Algorithm version: " + metaData["Version"]);
			log("Algorithm accuracy: " + metaData["Accuracy"]);

			log("\nChanging option1 to 4...");
			stuffs["option1"] = 4;
			testAlg.setOptions(stuffs);

			Dictionary<string, dynamic> stuffs2 = testAlg.getOptions();
			foreach (string key in stuffs2.Keys)
			{
				log("New python alg option: {" + key + ":" + stuffs2[key] + "}");
			}

			log("code:\n" + testAlg.generateRunnableCode());

			// ----- python generation test -------

			List<PyAlgorithm> algs = new List<PyAlgorithm>();

			PyAlgorithm inputAlg = gen.loadPythonAlgorithm("../../AlgTest/FileInput.py");
			Dictionary<string, dynamic> inputAlgOptions = inputAlg.getOptions();
			inputAlgOptions["File Path"] = "TestData.dat"; // theoretically, depending on what the goal is, this is something that the meta compiler doesn't even need, just generates python code USER can use (manually supplying input makes more sense)
			inputAlg.setOptions(inputAlgOptions);
			algs.Add(inputAlg);

			PyAlgorithm sumAlg = gen.loadPythonAlgorithm("../../AlgTest/SumOperation.py");
			Dictionary<string, dynamic> sumAlgOptions = sumAlg.getOptions();
			sumAlgOptions["Sum Amount"] = 5;
			sumAlg.setOptions(sumAlgOptions);
			algs.Add(sumAlg);
			
			PyAlgorithm sumAlg2 = gen.loadPythonAlgorithm("../../AlgTest/SumOperation.py");
			Dictionary<string, dynamic> sumAlg2Options = sumAlg2.getOptions();
			sumAlg2Options["Sum Amount"] = 1;
			sumAlg2.setOptions(sumAlg2Options);
			algs.Add(sumAlg2);

			PyAlgorithm outAlg = gen.loadPythonAlgorithm("../../AlgTest/FileOutput.py");
			Dictionary<string, dynamic> outAlgOptions = outAlg.getOptions();
			outAlgOptions["File Path"] = "OUTPUT.dat";
			outAlg.setOptions(outAlgOptions);
			algs.Add(outAlg);

			gen.generatePythonCode(algs, "../../AlgTest", "./COMPILED");
            */


            //Representation inputRep = new Representation()
            /*
			Block inprep = AlgorithmLoader.generateBlock("inputthingy", AlgorithmType.Input, new Datatype[0], new Datatype[] { Datatype.findType("Scalar Integer") });
			Block oprep = AlgorithmLoader.generateBlock("sumthingy", AlgorithmType.Operation, new Datatype[] { Datatype.findType("Scalar Integer") }, new Datatype[] { Datatype.findType("Scalar Integer") });
			Block outrep = AlgorithmLoader.generateBlock("output", AlgorithmType.Output,  new Datatype[] { Datatype.findType("Scalar Integer") },new Datatype[0]);

			m_blocks.Add(0, inprep);
			m_blocks.Add(1, oprep);
			m_blocks.Add(2, outrep);
			*/
		}

        //END STUFF

		// properties
		public Canvas getMainCanvas() { return world; }
		public GraphicContainer getGraphicContainer() { return m_gc; }
		//public Dictionary<int, Representation> getRepresentations() { return m_representations; } 
		

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
            InterGraph graph = new InterGraph();
            
            /*foreach(KeyValuePair<int, Representation> kvp in m_representations)
            {
                graph.append( new InterNode(kvp.Value, graph) );
            }*/

			new InterNode(m_blocks[0], graph);
			
            List<PyAlgorithm> algs = graph.topoSort();
            log("Count: " + algs.Count);
			foreach (PyAlgorithm alg in algs)
			{
				Dictionary<string, dynamic> dic = alg.getOptions();
				Block rep = (Block)dic["thing"];
				log(rep.getID().ToString());
			}

            //(new PythonGenerator()).generatePythonCode(algs, "../../AlgTest", "./COMPILED");
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

        public void appendBlock(Block block)
        {
            m_blocks.Add(block.getID(), block);
        }

		// create representation (eventually this should be based SOLELY on an imported algorithm, not created manually)
		/*private void addRep(int inputs, int outputs)
		{
			Representation r = new Representation(inputs, outputs);
			m_representations.Add(r.getID(), r);
		}*/

		private void loadData(string fileName)
		{
			// load me
		}

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

					Block r = m_blocks[id];

					if (attr == "lbl") 
					{ 
						r.setName(val);
						log("Updated block label to '" + val + "'");
					}
					else if (attr == "color") 
					{
                        r.getGraphic().BaseColor = ((SolidColorBrush)(new BrushConverter().ConvertFrom("#" + val))).Color; 
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
            for(int i = 0; i < m_blocks.Count; i++)
            {
                r += "\t" + m_blocks[i].getID() + ")";
                r += m_blocks[i].getName() + ", ";
                r += m_blocks[i].getFamily() + ": " + m_blocks[i].getAlgorithm() + "\n";
            }
            return r;
        }

        private string listDatatypes()
        {
            string r = "";
            for(int i = 0; i < Datatype.numberOfTypes(); i++)
            {
                r += "\t" + i + ")" + Datatype.getType(i).stringRep() + "\n";
            }
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
