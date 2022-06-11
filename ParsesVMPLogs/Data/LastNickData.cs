namespace ParsesVMPLogs.Data
{
    public class LastNickData
    {
        public int NumberLine { get; set; }
        public string CurrentNickName { get; set; }
        public string PreviousNickName { get; set; }

        public LastNickData(int numberLine, string currentNickName, string previousNickName)
        {
            NumberLine = numberLine;
            CurrentNickName = currentNickName;
            PreviousNickName = previousNickName;
        }

        public LastNickData()
        {
            
        }
    }
}