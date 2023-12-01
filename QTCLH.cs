using System.Reflection;
using System.Text.Json;

namespace qtcl
{
    /* 
     * This is a set of helper classes that isn't strictly nessacary and can be refactored out.
     * Instead, this is to help make the app logic clearer and to make portability a bit easier.
     */
    internal static class QTCLH
    {
        public static string APP_DIR = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.qtcl\\";

        public static void CheckForAppDir()
        {
            if (!QTCLH.DIR.Exists(QTCLH.APP_DIR))
            {
                QTCLH.DIR.Create(QTCLH.APP_DIR);
                QTCLH.CLI.PrintInfo($"The default configuration directory was not found. The directory \"{QTCLH.APP_DIR}\" was created.");
                QTCLH.CLI.PrintWarning("Some features will not work correctly unless configured correctly in this directory. Please check the user manual for more information.");
            }
        }

        public static void ShowHelp()
        {
            string[] helpText = [
                "qtcl v1.0 (Beta 1)",
                "",
                "Possible Arguments:",
                "<inputFilepath>\n\tRun the parser and intrepreter from the specified inputFilepath.",
                "<inputFilepath> <outputFilepath>\n\tRun the parser and intrepreter on the specified inputFilepath and saves the results to the outputFilepath.",
                "--help\n-h\n\tShows this screen."
            ];
            foreach (string line in helpText)
            {
                QTCLH.CLI.Print(line + "\n");
            }
        }
        public static class CLI {
            public static void Print(string message)
            {
                Console.WriteLine(message);
            }
            public static void PrintInfo(string message)
            {
                ConsoleColor oldFColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = oldFColor;
            }
            public static void PrintWarning(string message)
            {
                ConsoleColor oldFColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ForegroundColor = oldFColor;
            }
            public static void PrintError(string message)
            {
                ConsoleColor oldFColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = oldFColor;
            }
            public static string UserInputPrompt(string inputName)
            {
                ConsoleColor oldConsoleColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[Input Request] ");
                Console.ForegroundColor = oldConsoleColor;

                Console.Write($"Please enter a value for \"{inputName}\": ");
                string? output = Console.ReadLine();

                output ??= "";
                return output;
            }
        }

        public static class DIR
        {
            public static void Create(string path)
            {
                if (Directory.Exists(path))
                    return;
                Directory.CreateDirectory(path);
            }
            public static bool Exists(string path)
            {
                return Directory.Exists(path);
            }
        }

        public static class FILE
        {
            public static string GetAllContent(string path)
            {
                if (File.Exists(path)) {
                    return File.ReadAllText(path);
                }
                else
                {
                    return "";
                }
            }

            public static void PutAllContent(string path, string content)
            {
                if (!File.Exists (path))
                {
                    File.Create(path).Close();
                }
                File.WriteAllText(path, content);
            }

            public static void Create(string path)
            {
                if ( File.Exists(path) )
                    return;
                File.Create(path).Close();
            }

            public static bool Exists(string path)
            {
                return File.Exists(path);
            }

        }
        
    }
}
