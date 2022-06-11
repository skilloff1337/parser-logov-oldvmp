using ParsesVMPLogs.Scripts;

namespace ParsesVMPLogs
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            SearchLastNickName.LoadChangedNicknames(); 
            MainScript.Start();
        }
    }
}