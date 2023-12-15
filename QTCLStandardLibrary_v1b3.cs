using System.Text.RegularExpressions;

namespace qtcl
{
    internal static partial class QTCLStandardLibrary_v1b3
    {
        

        //
        // Public Facing Interfaces
        //

        internal static List<QTCLCommand> Commands { get => commands; }
        internal static List<QTCLOperator> Operators { get => operators; }
        
        [GeneratedRegex(@"[~*!@#$%^&*()_+\-=\[\];:\\|,.<>\/?]")]
        public static partial Regex QTCLOperatorRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9]+$")]
        public static partial Regex QTCLCommandWordRegex();

        //
        // Comannds
        //
        private static readonly List<QTCLCommand> commands =
        [
            //
            // DateTime Commands
            //
            new("Now", (() => DateTime.Now.ToString())),
            new("Today", (() => DateTime.Now.ToString("dd MMMM yyyy"))),
            new("dayToday", (() => DateTime.Now.ToString("%d"))),
            new("DayToday", (() => DateTime.Now.ToString("dd"))),
            new("DayNameShortToday", (() => DateTime.Now.ToString("ddd"))),
            new("DayNameToday", (() => DateTime.Now.ToString("dddd"))),
            new("monthToday", (() => DateTime.Now.ToString("%M"))),
            new("MonthToday", (() => DateTime.Now.ToString("MM"))),
            new("MonthNameShortToday", (() => DateTime.Now.ToString("MMM"))),
            new("MonthNameToday", (() => DateTime.Now.ToString("MMMM"))),
            new("YearToday", (() => DateTime.Now.ToString("yyyy"))),
            new("hourToday", (() => DateTime.Now.ToString("%h"))),
            new("HourToday", (() => DateTime.Now.ToString("hh"))),
            new("hour24Today", (() => DateTime.Now.ToString("%H"))),
            new("Hour24Today", (() => DateTime.Now.ToString("HH"))),
            new("minuteToday", (() => DateTime.Now.ToString("%m"))),
            new("MinuteToday", (() => DateTime.Now.ToString("mm"))),
            new("secondToday", (() => DateTime.Now.ToString("%s"))),
            new("SecondToday", (() => DateTime.Now.ToString("ss"))),
            new("PeriodOfDayToday", (() => DateTime.Now.ToString("tt"))),
            new("Tomorrow", (() => DateTime.Now.AddDays(1).ToString("dd MMMM yyyy"))),
            new("TomorrowFull", (() => DateTime.Now.AddDays(1).ToString("dddd, dd MMMM yyyy"))),
            new("Yesterday", (() => DateTime.Now.AddDays(-1).ToString("dd MMMM yyyy"))),
            new("YesterdayFull", (() => DateTime.Now.AddDays(-1).ToString("dddd, dd MMMM yyyy"))),
            new("UnixTime", (() => DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())),
            new("NextBusinessDay", (() =>
            {
                var day = DateTime.Now.AddDays(1);
                if (day.ToString("ddd") == "Sat")
                {
                    day = day.AddDays(2);
                }
                if (day.ToString("ddd") == "Sun")
                {
                    day = day.AddDays(1);
                }
                return day.ToString("dd MMMM yyyy");
            })),
            new("NextBusinessDayFull", (() =>
            {
                var day = DateTime.Now.AddDays(1);
                if (day.ToString("ddd") == "Sat")
                {
                    day = day.AddDays(2);
                }
                if (day.ToString("ddd") == "Sun")
                {
                    day = day.AddDays(1);
                }
                return day.ToString("dddd, dd MMMM yyyy");
            })),
            new("PreviousBusinessDay", (() =>
            {
                var day = DateTime.Now.AddDays(-1);
                if (day.ToString("ddd") == "Sat")
                {
                    day = day.AddDays(-1);
                }
                if (day.ToString("ddd") == "Sun")
                {
                    day = day.AddDays(-2);
                }
                return day.ToString("dd MMMM yyyy");
            })),
            new("LastBusinessDayFull", (() =>
            {
                var day = DateTime.Now.AddDays(-1);
                if (day.ToString("ddd") == "Sat")
                {
                    day = day.AddDays(-1);
                }
                if (day.ToString("ddd") == "Sun")
                {
                    day = day.AddDays(-2);
                }
                return day.ToString("dddd, dd MMMM yyyy");
            })),

            //
            // Math Commands
            //
            new("RandomNumber", (() =>
            {
                Random r = new();
                return r.Next().ToString();
            })),
            new("RandomDecimal", (() =>
            {
                Random r = new();
                return r.NextDouble().ToString();
            })),
            new("RandomLongNumber", (() =>
            {
                Random r = new();
                return r.NextInt64().ToString();
            })),
            new("RandomLongDecimal", (() =>
            {
                Random r = new();
                return string.Concat(r.NextInt64().ToString(), r.NextDouble().ToString().AsSpan(1));
            }))

            //
            // Other/Misc Commands
            //

        ];

        //
        // Operators
        //        
        
        private static Dictionary<string, string> VariablePromptOperatorPreviousResponses = [];

        private static readonly List<QTCLOperator> operators = [
            // User Input Prompt
            new("!", ((input) =>
            {
                if (VariablePromptOperatorPreviousResponses.TryGetValue(input, out string? value))
                {
                    return value;
                }
                else
                {
                    string userResponse = QTCLH.CLI.UserInputPrompt(input);
                    VariablePromptOperatorPreviousResponses[input] = userResponse;
                    return userResponse;
                }
            })),

            // Semi-Random File Text Includer
            new("@", ((input) =>
            {
                string ret = "";
                string? fName = $"{Program.CURRENT_CONFIG.RandomFileTextLookupOperator.Directory}{input}{Program.CURRENT_CONFIG.RandomFileTextLookupOperator.FileType}";
                
                if (QTCLH.FILE.Exists(fName))
                {
                    string[] fileContents = QTCLH.FILE.GetAllContent(fName).Split('\n');
                    List<string> lineOptions = [];
                    
                    //check for return carriages. Don't include the as a possible option.
                    for (int i = 0; i < fileContents.Length; i++)
                    {
                        if (fileContents[i] != "\r")
                            lineOptions.Add(fileContents[i]);
                    }
                    if (lineOptions.Count > 0)
                    {
                        Random r = new();
                        int ind = r.Next(0, lineOptions.Count);
                        ret = lineOptions[ind];
                    }
                } else
                {
                    QTCLH.CLI.PrintWarning($"Unable to parse the requsted text inclusion for the input named \"{input}\". A blank value will be inserted instead.\nPlease check the following file: \"{fName}\"");
                }
                return ret;
            }))
        ];
    }
}
