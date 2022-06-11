using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParsesVMPLogs.Data
{
    public class AccountData
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nickname")]
        public string NickName { get; set; }

        [BsonElement("hardWareID")]
        public List<string> HardWare { get; set; } = new();

        [BsonElement("needReturnMoney")]
        public long NeedReturnMoney { get; set; }
        
        [BsonIgnore]
        public long LineNumber { get; set; }

        public AccountData(ObjectId id, string nick, List<string> hardWare, long needReturnMoney, long lineNumber)
        {
            Id = id;
            NickName = nick;
            HardWare = hardWare;
            NeedReturnMoney = needReturnMoney;
            LineNumber = lineNumber;
        }

        public AccountData()
        {
            
        }
    }
    
}