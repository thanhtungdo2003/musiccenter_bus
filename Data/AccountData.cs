using MuscicCenter.Storage;
using MusicBusniess;
using TT_DBLib.Storage;

namespace MusicCenterAPI.Data
{
    public class AccountData : BaseModel
    {
        protected override string TableName => DatabaseStruct.AccountTable;
        protected override string KeyColumn => "UserName";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string PremiumEx { get; set; }
        public string Token { get; set; }
        public DateTime JoinDay { get; set; }

        public AccountData(IMusicCenterAPI api) : base(api) { }
        public AccountData(IMusicCenterAPI api, string userName) : base(api, userName) { }
    }
}
