using MuscicCenter.Storage;
using MusicBusniess;

namespace MusicCenterAPI.Data
{
    public class CommentsData : DatabaseStruct
    {
        public Guid CommentUid { get; set; }
        public string UserName { get; set; }
        public Guid RecordUid { get; set; }
        public string? Content { get; set; }
        public DateTime TimeOf { get; set; }
        private IMusicCenterAPI _api;
        public CommentsData(IMusicCenterAPI api)
        {
            _api = api;
        }
        public CommentsData(IMusicCenterAPI api, string rawUid)
        {
            _api = api;

            if (Guid.TryParse(rawUid, out Guid uid))
            {
                if (_api.getValueByKey(CommentsTable, "CommentUid", "CommentUid", rawUid) == null)
                {
                    Content = null;
                    return;
                }
                CommentUid = uid;
                UserName = (string)_api.getValueByKey(CommentsTable, "UserName", "CommentUid", rawUid);
                RecordUid = Guid.Parse((string)_api.getValueByKey(CommentsTable, "RecordUid", "CommentUid", rawUid));
                Content = (string)_api.getValueByKey(CommentsTable, "Content", "CommentUid", rawUid);
                TimeOf = (DateTime)_api.getValueByKey(CommentsTable, "TimeOfComment", "CommentUid", rawUid);
            }
        }
        public void Save()
        {
            if (_api.getValueByKey(CommentsTable, "CommentUid", "CommentUid", CommentUid.ToString()) == null)
            {
                _api.InsertData(CommentsTable, "CommentUid", new Dictionary<string, object>
                {
                { "CommentUid", CommentUid.ToString() },
                { "UserName", UserName},
                { "RecordUid", RecordUid.ToString()},
                { "Content", Content}
            });
            }
            else
            {
                _api.UpdateData(CommentsTable, "Content", "CommentUid", CommentUid.ToString(), Content);
            }
        }
    }
}
