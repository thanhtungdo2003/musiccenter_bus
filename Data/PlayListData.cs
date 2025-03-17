using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Data;
using MusicCenterAPI.ProcedureStorage;
using MusicBusniess;
using MuscicCenter.Storage;

namespace MusicCenterAPI.Data
{
    public class PlayListData : DatabaseStruct
    {
        public Guid playlistUid { get; set; }
        public string playlistName { get; set; }
        public string userName { get; set; }
        public List<RecordData> records;
        private IMusicCenterAPI _api;
        public PlayListData(IMusicCenterAPI api)
        {
            _api = api;
        }
        public PlayListData(IMusicCenterAPI api, Guid pid)
        {
            _api = api;
            records = new List<RecordData>();
            if (_api.getValueByKey(PlaylistTable, "PlaylistUid", "PlaylistUid", pid.ToString()) != null)
            {
                this.playlistUid = pid;
                this.playlistName = (string)_api.getValueByKey(PlaylistTable, "DisplayName", "PlaylistUid", pid.ToString());
                this.userName = (string)_api.getValueByKey(PlaylistTable, "UserName", "PlaylistUid", pid.ToString());
                SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@PlaylistUid", SqlDbType.NVarChar) { Value = pid.ToString() }
           };
                DataTable playlist = _api.ProcedureCall(ProcedureName.GET_RECORDS_FROM_PLAYLIST, parameters);
                foreach (var item in _api.CoventToDictionarysWithDataTable(playlist))
                {
                    if (item.ContainsKey("RecordUid"))
                    {
                        records.Add(new RecordData(api, Guid.Parse((string)item["RecordUid"]).ToString()));
                    }
                }
            }
        }
        public void Save()
        {
            if (_api.getValueByKey(PlaylistTable, "PlaylistUid", "PlaylistUid", playlistUid.ToString()) == null)
            {
                _api.InsertData(PlaylistTable, "PlaylistUid", new Dictionary<string, object>
                {
                { "PlaylistUid", playlistUid.ToString() },
                { "DisplayName", playlistName},
                { "UserName", userName}
            });
            }
            else
            {
                _api.UpdateData(PlaylistTable, "DisplayName", "PlaylistUid", playlistUid.ToString(), playlistName);
                _api.UpdateData(PlaylistTable, "UserName", "PlaylistUid", playlistUid.ToString(), userName);
            }
        }
    }
}
