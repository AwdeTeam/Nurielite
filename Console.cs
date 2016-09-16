using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		/// <summary>
		/// Sets up the console for use. MUST be called before any other functions in this class are used.
		/// </summary>
		/// <param name="txtCommandLine">Textbox where users will type commands.</param>
		/// <param name="lblConsoleTextDisplay">The RichTextBox that will display all console output.</param>
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
	}
}
