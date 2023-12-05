using qtcl;
using System.Reflection;
using System.Text.Json;
internal class Program
{

    public static QTCLConfig? CURRENT_CONFIG { get; set; }
    private static void Main(string[] args)
    {
        QTCLH.CheckForMainAppDir();
        CURRENT_CONFIG = QTCLConfigLoader.Load();
        List<string> configFeatureDirs = [CURRENT_CONFIG.RandomFileTextLookupOperator.Directory];
        QTCLH.CheckForFeatureDirs(configFeatureDirs);
        QTCLFileParser parser = new();        
        if (args.Length == 0)
        {
            QTCLH.ShowHelp();
        }
        else if (args.Length == 1)
        {
            if (args[0] != "") {
                switch (args[0])
                {
                    case "--help":
                    case "-h":
                        QTCLH.ShowHelp();
                        break;
                    default:
                        parser.ParseToCLI(args[0]);
                        break;
                }
            }    
        }
        else if (args.Length == 2)
        {
            parser.ParseToFile(args[0], args[1]);
        }
    }
}