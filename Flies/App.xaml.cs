using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace Flies
{
    public partial class App : Application
    {
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        // https://stackoverflow.com/a/37975090
        private string GetArgument(IEnumerable<string> args, string option) => args.SkipWhile(i => i != option).Skip(1).Take(1).FirstOrDefault();

        private bool OptionIsSet(IEnumerable<string> args, string option) => args.SkipWhile(i => i != option).FirstOrDefault() == option;

        private void Application_Startup(object sender, EventArgs e)
        {
            int flyCount = 1;
            int delay = 0;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 0)
            {
                bool showHelp = OptionIsSet(args, "-h") || OptionIsSet(args, "--help");

                if (showHelp)
                {
                    ShowHelp();
                    return;
                }

                string argCount = GetArgument(args, "-c") ?? GetArgument(args, "--count");
                if (argCount != null && !int.TryParse(argCount, out flyCount)) {
                    ShowError("ERROR: " + argCount + " is not a number");
                    return;
                }

                string argDelay = GetArgument(args, "-d") ?? GetArgument(args, "--delay");
                if (argDelay != null && !int.TryParse(argDelay, out delay))
                {
                    ShowError("ERROR: " + argDelay + " is not a number");
                    return;
                }

                if (flyCount < 1)
                {
                    ShowError("ERROR: Fly count must be at least 1");
                    return;
                }

                if (delay < 0 || delay > 86400)
                {
                    ShowError("ERROR: Delay must be in range [0-86400]");
                    return;
                }

            }

            RunWithOptions(flyCount, delay);
        }

        private void RunWithOptions(int flyCount, int delay)
        {
            MainWindow mainWindow = new MainWindow(flyCount, delay);
            mainWindow.Show();
        }

        private void ShowHelp()
        {
            if (AttachConsole(ATTACH_PARENT_PROCESS))
            {
                Console.WriteLine("\n");
                Console.WriteLine("Flies.exe options:");
                Console.WriteLine();
                Console.WriteLine("-c\t--count\tCount of flies to display, default is 1 (more flies = more lag)");
                Console.WriteLine("-d\t--delay\tDelay (in seconds) until the flies appear (0-86400)");
                Console.WriteLine("-h\t--help\tShow this message and exit");
                Console.WriteLine();
                FreeConsole();
            }
            Shutdown();
        }

        private void ShowError(string error)
        {
            if (AttachConsole(ATTACH_PARENT_PROCESS))
            {
                Console.WriteLine("\n");
                Console.Write("Flies.exe: ");
                Console.WriteLine(error);
                Console.WriteLine("Try `--help' for more information.");
                Console.WriteLine();
                FreeConsole();
            }
            Shutdown(1);
        }

    }
}
