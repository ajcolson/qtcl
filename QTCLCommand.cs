namespace qtcl
{
    internal class QTCLCommand(string commandWord, Func<string> commandFunc)
    {
        public string CommandWord = commandWord;
        private readonly Func<string> CommandFunc = commandFunc;

        public string Execute()
        {
            return CommandFunc();
        }
        
    }
}
