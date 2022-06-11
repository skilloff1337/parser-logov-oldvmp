using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParsesVMPLogs.Scripts
{
    public static class GetHardWareFromString
    {
        private static readonly Regex _regex = new(".*(HW: .*[A-Za-z0-9_])", RegexOptions.Compiled);
        
        public static List<string> GetHardWareList(string line)
        {
            var match = _regex.Match(line);
            if (!match.Success) return null;

            var hardWares = match.Groups[1].Value[3..].Trim().Split("// ");
            var hardWareList = hardWares.Where(hardWare => !IsBadHw(hardWare)).ToList();

            return hardWares.Length == 0 ? null : hardWareList;
        }

        private static bool IsBadHw(string hw) =>
            string.IsNullOrEmpty(hw) || 
            string.IsNullOrWhiteSpace(hw) || 
            hw.Contains("None") || 
            hw.Contains("To be filled by O.E.M.") ||
            hw.Contains("SystemSerialNumber");
    }
}