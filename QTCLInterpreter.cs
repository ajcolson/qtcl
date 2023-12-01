namespace qtcl
{
    internal partial class QTCLInterpreter
    {
        public QTCLInterpreter()
        {

        }

        public string Interpret(string commandString)
        {
            string buffer = "";
            if (commandString != null)
            {
                string[] commandWords = commandString.Split(' ');
                for (int i = 0; i < commandWords.Length; i++)
                {
                    bool HasOperator = QTCLStandardLibrary.QTCLOperatorRegex().IsMatch(commandWords[i].Substring(0, 1));
                    if ( HasOperator )
                    {
                        var foundOperator = QTCLStandardLibrary.Operators.Where((c => c.OperatorWord == commandWords[i].Substring(0,1)));
                        bool remainderIsValidTextOnly = QTCLStandardLibrary.QTCLCommandWordRegex().IsMatch(commandWords[i].Substring(1));
                        bool HasValidOperatorString = foundOperator.Count() > 0 && remainderIsValidTextOnly;
                        if ( HasValidOperatorString )
                        {
                            buffer += foundOperator.First().Execute(commandWords[i].Substring(1));
                        }
                    }
                    else
                    {
                        var foundCommand = QTCLStandardLibrary.Commands.Where((c => c.CommandWord == commandWords[i]));
                        if (foundCommand.Count() > 0)
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
