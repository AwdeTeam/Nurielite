using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media;

namespace Nurielite
{
	/// <summary>
	/// Takes care of displaying logging information as well as handling any typed commands the user wants to run.
	/// </summary>
	public class Console
	{
		// member variables
		private static RichTextBox m_rtxtConsoleTextDisplay;
		private static TextBox m_txtCommandLine;

		private static List<string> m_lCommandHistory = new List<string>();
		private static int m_iCommandIndex = 0; // keep strack of where in command history you are

		/// <summary>
		/// Sets up the console for use. MUST be called before any other functions in this class are used.
		/// </summary>
		/// <param name="txtCommandLine">Textbox where users will type commands.</param>
		/// <param name="rtxtConsoleTextDisplay">The RichTextBox that will display all console output.</param>
		public static void Initialize(TextBox txtCommandLine, RichTextBox rtxtConsoleTextDisplay)
		{
			m_rtxtConsoleTextDisplay = rtxtConsoleTextDisplay;
			m_txtCommandLine = txtCommandLine;
		}

		public static void log(string sMessage) { log(sMessage, Colors.DarkCyan); }
		public static void log(string sMessage, Color pColor)
		{
			Run pRun = new Run(sMessage);
			pRun.Foreground = new SolidColorBrush(pColor);
			m_rtxtConsoleTextDisplay.Document.Blocks.Add(new Paragraph(pRun));
			m_rtxtConsoleTextDisplay.ScrollToEnd();
		}

		public static void navigateUpCommandStack()
		{
			m_iCommandIndex--;
			resolveCommandStackIndex();
		}
		public static void navigateDownCommandStack()
		{
			m_iCommandIndex++;
			resolveCommandStackIndex();
		}
		private static void resolveCommandStackIndex()
		{
			if (m_iCommandIndex < 0 || m_iCommandIndex >= m_lCommandHistory.Count) { m_txtCommandLine.Text = ""; return; }
			m_txtCommandLine.Text = m_lCommandHistory[m_iCommandIndex];
		}

		// when user hits enter in the console bar
		public static void enterConsoleCommand()
		{
			// first get command
			string sCommand = m_txtCommandLine.Text;
			m_txtCommandLine.Text = "";

			// add to console
			log("> " + sCommand, Colors.White);
			m_txtCommandLine.Focus(); // make sure command line retains focus

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

		// NOTE: this function DOES run the command after parsing
		// take a command string and figure out what's what (USE THIS FOR HANDLING COMMAND SCRIPTS)
		// NOTE: this function is NOT setup to handle more than one set of quotes in an entire command
		// (essentially adds every space-delimited word to a prelist, then selectively adds to actual word
		//   list, combining multiple into one 'word' if quotes were detected)
		private static void parseCommand(string command)
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
					sWord = sWord.Replace("\"", "");
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
		private static void handleCommand(List<string> lKeys, List<string> lVals)
		{
			if (lKeys[0] == "exit" || lKeys[0] == "quit") { cmd_exit(); }
			else if (lKeys[0] == "help") { cmd_printHelp(); }
			else if (lKeys[0] == "clear" || lKeys[0] == "cls") { cmd_clearConsole(); }
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
			else if (lKeys[0] == "list")
			{
				if (lKeys.Count == 0)
				{
					log("hmm");
					lKeys.Add("");
				}

				switch (lKeys[1])
				{
					case "blk":
					case "blocks":
						log(cmd_listBlocks());
						break;
					case "":
					default:
						log("Please enter a type! (rep or dt)", Colors.Red);
						break;
				}
			}
		}

		// ------------------------------------
		//  COMMAND FUNCTIONS
		// ------------------------------------

		private static void cmd_exit() { Application.Current.Shutdown(); }
		private static void cmd_printHelp()
		{
			log("exit | quit", Colors.Yellow);
			log("clear | cls \t// clears console", Colors.Yellow);
			log("help", Colors.Yellow);
			log("list (rep, dt)", Colors.Yellow);
			log("add rect[angle] -[x] -[y] -[width] -[height]\add rect[angle] -[x],[y],[width],[height]", Colors.Yellow);
			log("add rep[resentation] -[numInputs],[numOutputs]", Colors.Yellow);
			log("edit rep[resentation] -[id] -[attr] -[value]\n\tattr: color, lbl", Colors.Yellow);
		}
		private static void cmd_clearConsole() { m_rtxtConsoleTextDisplay.Document.Blocks.Clear(); } // NOTE: these blocks are not to be confused with our blocks!!
		private static string cmd_listBlocks()
		{
			string sBlockString = "";
			for (int i = 0; i < Master.Blocks.Count; i++)
			{
				sBlockString += "\t" + Master.Blocks[i].ID + ")";
				sBlockString += Master.Blocks[i].Name + ", ";
			}
			return sBlockString;
		}
	}
}
