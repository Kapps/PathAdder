using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathAdder {
    static class Program {

		private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("User32.dll")]
        private static extern bool SetProcessDPIAware();
		[DllImport("kernel32.dll")]
		private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args) {
			// For debugging, sometimes the manifest doesn't seem to apply.
			// Errors aren't a concern and shouldn't happen anyways.
            SetProcessDPIAware();

			// Check if we're being run from the command line by passing in arguments.
			string ArgPath = String.Join(" ", args).Trim();
			if(!String.IsNullOrWhiteSpace(ArgPath)) {
				try {
					AttachConsole(ATTACH_PARENT_PROCESS); // Ignore errors, can't do anything about it.
					EnvironmentUtils.AppendPath(ArgPath);
					Console.WriteLine("Successfully added \'" + ArgPath + "\' to path variable.");
					return 0;
				} catch(Exception ex) {
					Console.Error.WriteLine("Failed to append path: " + ex.Message);
					return 1;
				}
			}

			// If not, show a UI.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
			return 0;
        }
    }
}
