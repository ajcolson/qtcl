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
            switch (args[0])
            {
                case "-L_V1B3":
                case "-L_v1b3":
                    QTCLH.USE_LEGACY_V1B3 = true;
                    parser.ParseToCLI(args[1]);
                    break;
                default:
                    parser.ParseToFile(args[0], args[1]);
                    break;
            }
            
        }
        else if (args.Length == 3)
        {
            switch (args[0])
            {
                case "-L_V1B3":
                case "-L_v1b3":
                    QTCLH.USE_LEGACY_V1B3 = true;
                    parser.ParseToFile(args[1], args[2]);
                    break;

            }

            
        }
    }
}