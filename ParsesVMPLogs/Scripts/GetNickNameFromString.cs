using System.Text.RegularExpressions;

namespace ParsesVMPLogs.Scripts
{
    public static class GetNickNameFromString
    {
        private static readonly Regex _regex = new("([A-z]+_[A-z]+)", RegexOptions.Compiled);

        public static string GetNickName(string line)
        {
            var match = _regex.Match(line);
            return !match.Success ? null : match.Groups[1].Value;
        }
    }
}