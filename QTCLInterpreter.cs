namespace qtcl
{
    internal partial class QTCLInterpreter
    {
        public QTCLInterpreter()
        {

        }

        public string Interpret_v1b3(string commandString)
        {
            string buffer = "";
            if (commandString != null && commandString != "")
            {
                string[] commandWords = commandString.Split(' ');
                for (int i = 0; i < commandWords.Length; i++)
                {
                    bool HasOperator = QTCLStandardLibrary_v1b3.QTCLOperatorRegex().IsMatch(commandWords[i][..1]);
                    if ( HasOperator )
                    {
                        var foundOperator = QTCLStandardLibrary_v1b3.Operators.Where((c => c.OperatorWord == commandWords[i][..1]));
                        bool remainderIsValidTextOnly = QTCLStandardLibrary_v1b3.QTCLCommandWordRegex().IsMatch(commandWords[i][1..]);
                        bool HasValidOperatorString = foundOperator.Any() && remainderIsValidTextOnly;
                        if ( HasValidOperatorString )
                        {
                            buffer += foundOperator.First().Execute(commandWords[i][1..]);
                        }
                    }
                    else
                    {
                        var foundCommand = QTCLStandardLibrary_v1b3.Commands.Where((c => c.CommandWord == commandWords[i]));
                        if (foundCommand.Any())
                        {
                            buffer += foundCommand.First().Execute();
                        }
                    }
                }
            }
            return buffer;
        }
    }
}
