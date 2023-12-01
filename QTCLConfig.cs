using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace qtcl
{
    public static class QTCLConfigLoader
    {
        public static string DefaultJSON = $$"""
        {
            "RandomFileTextLookupOperator":
        	{
        		"Directory": "{{QTCLH.APP_DIR}}data\random-text-files\",
            	"FileType": ".txt"
        	}
        }
        """;

        public static QTCLConfig? Load()
        {
            string confPath = QTCLH.APP_DIR + "qtcl.conf";
            if (!QTCLH.FILE.Exists(confPath))
            {
                QTCLH.FILE.Create(confPath);
                QTCLH.FILE.PutAllContent(confPath, QTCLConfigLoader.DefaultJSON);
            }
            string rawConfContents = QTCLH.FILE.GetAllContent(confPath);

            // The config file is written to be user friendly. Some of the contents will make the JSONSerializer panic.
            // Below we "sanitize" the contents.

            // Filesystem Paths are written with single backslash chars "\", so we must manually escape them.
            // Note: We do not do any checking for invalid path chararters here. The parser and interpreter will handle these.
            string cleanConfContents = rawConfContents.Replace(@"\", @"\\");

            QTCLConfig? ret = JsonSerializer.Deserialize<QTCLConfig>(cleanConfContents);

            return ret;
        }
    }

    // https://json2csharp.com/ is a lifesaver for generating these classes.
    // NOTE: When you use the site, be sure to escape the strings.
    public class RandomFileTextLookupOperator
    {
        [JsonPropertyName("Directory")]
        public string? Directory { get; set; }

        [JsonPropertyName("FileType")]
        public string? FileType { get; set; }
    }

    public class QTCLConfig
    {
        [JsonPropertyName("RandomFileTextLookupOperator")]
        public RandomFileTextLookupOperator? RandomFileTextLookupOperator { get; set; }
    }
}
