using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ParsesVMPLogs.Scripts
{
    public static class ReadLogMoney
    {
        private const string PATH_LOG_MONEY_PART_1 = "F:/V-MP/Logs/01/LogMoney.txt";
        private const string PATH_LOG_MONEY_PART_2 = "F:/V-MP/Logs/01/logMoney - 12.10.19.txt";
        private const string PATH_LOG_MONEY_PART_3 = "F:/V-MP/Logs/01/logMoney - Copy.txt";

        private static readonly Regex _regex = new (@".* ([A-z]+_[A-z]+) .* \(([0-9]+)\/([0-9]+)\).*",RegexOptions.Compiled);


        private static readonly Stopwatch _stopwatch = new();

        public static IEnumerable<(long, string, long)> GetMoney()
        {
            _stopwatch.Restart();
            Console.WriteLine("Start read money logs.");

            var lineNumber = 0L;
            for (var i = 0; i < 3; i++)
            {
                var path = i == 0 ? PATH_LOG_MONEY_PART_1 : i == 1 ? PATH_LOG_MONEY_PART_2 : PATH_LOG_MONEY_PART_3;
                Console.WriteLine("File: " + path);
                
                using var sr = new StreamReader(path,Encoding.Default);
                var line = string.Empty;
                
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    if(!line.Contains("GPM")) continue;

                    var match = _regex.Match(line);
                    if (!match.Success) continue;

                    var nick = match.Groups[1].Value;
                    var cashMoney = long.Parse(match.Groups[2].Value);
                    var bankMoney = long.Parse(match.Groups[3].Value);
                    var allMoney = cashMoney + bankMoney;
                    yield return (allMoney, nick, lineNumber);
                }
            }
            _stopwatch.Stop();
            Console.WriteLine($"Reading money logs is end. Elapsed: {_stopwatch.Elapsed}, lines: {lineNumber:#,#,#}");
        }
    }
}