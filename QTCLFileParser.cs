namespace qtcl
{
    internal class QTCLFileParser
    {
        QTCLInterpreter interpreter;
        public QTCLFileParser()
        {
            interpreter = new();
        }
        private string parse(string input)
        {
            string ParsedContentBuffer = "";
            string UnparsedContentBuffer = input;
            int UnparsedContentBufferIndex = 0;
            bool CommandStringStarted = false;
            string CommandStringBuffer = "";

            while (UnparsedContentBufferIndex < UnparsedContentBuffer.Length)
            {
                string NextCharInUnparsedBuffer = UnparsedContentBuffer[UnparsedContentBufferIndex].ToString();
                switch (NextCharInUnparsedBuffer)
                {
                    case "`":
                        UnparsedContentBufferIndex++;
                        ParsedContentBuffer += UnparsedContentBuffer[UnparsedContentBufferIndex];
                        break;
                    case "{":
                        CommandStringStarted = true;
                        CommandStringBuffer = "";
                        break;
                    case "}":
                        CommandStringStarted = false;
                        string output = interpreter.Interpret(CommandStringBuffer);
                        ParsedContentBuffer += output;
                        CommandStringBuffer = "";
                        break;
                    default:
                        if (CommandStringStarted)
                        {
                            CommandStringBuffer += NextCharInUnparsedBuffer;
                        }
                        else
                        {
                            ParsedContentBuffer += NextCharInUnparsedBuffer;
                        }
                        break;
                }
                UnparsedContentBufferIndex++;
            }
            return ParsedContentBuffer;
        }
        public void ParseToFile(string inputPath, string outputPath)
        {
            // Allow the user to lazily just specify a directory to save the output to.
            // We'll name the file output.txt in that directory
            if (QTCLH.DIR.Exists(outputPath))
            {
                outputPath += "\\output.txt";
            }
            string output = AttemptParseFile(inputPath);
            QTCLH.FILE.PutAllContent(outputPath, output);

        }

        public void ParseToCLI(string filePath)
        {
            string output = AttemptParseFile(filePath);
            QTCLH.CLI.Print("--START--");
            QTCLH.CLI.Print(output);
            QTCLH.CLI.Print("---END---");
        }
        public string AttemptParseFile(string filePath)
        {
            string ret = "";
            QTCLH.CLI.PrintInfo($"Attempting to parse the file: {filePath}");
            try
            {
                if (QTCLH.FILE.Exists(filePath))
                {
                    string fileContents = QTCLH.FILE.GetAllContent(filePath);
                    ret = parse(fileContents);
                    QTCLH.CLI.PrintInfo("Parsing completed!\n");
                }
                else
                {
                    QTCLH.CLI.PrintError("Error: The requested file doesn't exist.");
                }
            }
            catch (Exception e)
            {
                QTCLH.CLI.PrintError("There was a problem with parsing and outputting the results.\nPrinted below is information that you can send to the application maintainers to review for more help.\n");
                QTCLH.CLI.PrintError(e.ToString());
            }
            return ret;
        }
    }
}
