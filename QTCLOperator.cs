namespace qtcl
{
    internal class QTCLOperator(string commandWord, Func<string, string> commandFunc)
    {
        public string OperatorWord = commandWord;
        private readonly Func<string,string> OperatorFunc = commandFunc;

        public string Execute(string input)
        {
            return OperatorFunc(input);
        }
    }
}
