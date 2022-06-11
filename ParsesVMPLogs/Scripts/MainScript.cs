using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MongoDB.Driver;
using ParsesVMPLogs.Data;

namespace ParsesVMPLogs.Scripts
{
    public static class MainScript
    {
        private const string PATH_REG_LOG = "F:/V-MP/Logs/01/LogReg.txt";

        private static readonly MongoClient _client = new("mongodb://localhost:27017");
        private static readonly IMongoDatabase _database = _client.GetDatabase("parser");
        private static readonly IMongoCollection<AccountData> _collection =
            _database.GetCollection<AccountData>("accounts");

        private static readonly Stopwatch _stopwatch = new();
        private static readonly Dictionary<string, AccountData> _accounts = new();


        public static void Start()
        {
            _stopwatch.Start();
            Console.WriteLine("Start Parser.");
            LoadingNickNamesToList();
            AddMoneyToAccount();
            AddAccountsToMongoDateBase();
            _stopwatch.Stop();
            Console.WriteLine($"Stop Parser. Elapsed: {_stopwatch.Elapsed}");
        }


        private static void AddAccountsToMongoDateBase()
        {
            Console.WriteLine($"Start add accounts to database:");
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            var buffer = new List<AccountData>();
            foreach (var (_, account) in _accounts)
            {
                buffer.Add(account);
                if(buffer.Count < 1_000) continue;
                _collection.InsertMany(buffer);
                buffer.Clear();
            }
            stopwatch.Stop();
            Console.WriteLine($"Add accounts to database is end. Accounts: {_accounts.Count:#,#,#}. " +
                              $"Elapsed: {stopwatch.Elapsed}");
        }

        private static void AddMoneyToAccount()
        {
            Console.WriteLine("Start adding money to accounts");
            foreach (var (sum, nickName, lineNumber) in ReadLogMoney.GetMoney())
            {
                if (!_accounts.TryGetValue(nickName, out var account)) continue;
                if (account.LineNumber >= lineNumber) continue;

                account.NeedReturnMoney = sum;
                account.LineNumber = lineNumber;
            }

            Console.WriteLine("Adding money to accounts is end.");
        }

        private static void LoadingNickNamesToList()
        {
            Console.WriteLine("Start Loading NickName to List");

            using var sr = new StreamReader(PATH_REG_LOG, Encoding.Default);
            var line = string.Empty;
            var numAcc = 1;

            while ((line = sr.ReadLine()) != null)
            {
                var nickName = GetNickNameFromString.GetNickName(line);
                nickName = SearchLastNickName.SearchLastChangedNick(nickName);

                var hardWare = GetHardWareFromString.GetHardWareList(line);
                if (string.IsNullOrEmpty(nickName) || hardWare == null) continue;

                var acc = new AccountData()
                {
                    NickName = nickName,
                    HardWare = hardWare,
                    NeedReturnMoney = 0
                };
                _accounts.TryAdd(acc.NickName, acc);
                numAcc++;
            }

            Console.WriteLine("Loading of nicknames to list is end.");
        }
    }
}