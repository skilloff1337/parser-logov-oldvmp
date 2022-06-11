using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ParsesVMPLogs.Data;

namespace ParsesVMPLogs.Scripts
{
    public static class SearchLastNickName
    {
        private const string PATH_LOG_NAME = "F:/V-MP/Logs/01/LogName.txt";

        private static readonly List<LastNickData> _listChangedNicknames = new();
        private static readonly Regex _regex = new(".* name: ([A-z,0-9]+).* to ([A-z,0-9]+)",RegexOptions.Compiled);
        private static readonly Stopwatch _stopwatch = new();

        public static void LoadChangedNicknames()
        {
           _stopwatch.Restart();
           
           using var sr = new StreamReader(PATH_LOG_NAME,Encoding.Default);
           var line = string.Empty;
           var numberLine = 0;
           
           while ((line = sr.ReadLine()) != null)
           {
               numberLine++;
               
               var match = _regex.Match(line);
               if (!match.Success) continue;
               
               var changedName = new LastNickData()
               {
                   NumberLine = numberLine,
                   CurrentNickName = match.Groups[2].Value,
                   PreviousNickName = match.Groups[1].Value
               };
               _listChangedNicknames.Add(changedName);
           }
           _stopwatch.Stop();
           Console.WriteLine($"Loading of changed nicknames is end. Elapsed: {_stopwatch.Elapsed}");
        }

        public static string SearchLastChangedNick(string nick)
        {
            if (string.IsNullOrEmpty(nick)) 
                return null;
            
            var currentNick = nick;
            var changedNick = _listChangedNicknames.Find(x => x.PreviousNickName == currentNick);
            while (_listChangedNicknames.Contains(changedNick))
            {
                currentNick = changedNick!.CurrentNickName;
                _listChangedNicknames.Remove(changedNick);
                changedNick = _listChangedNicknames.Find(x => x.PreviousNickName == currentNick);
            }
            return currentNick;
        }
    }
}