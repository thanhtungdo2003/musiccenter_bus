using MuscicCenter.Storage;
using MusicBusniess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace MusicCenterAPI.Data
{
    public class InfoPlayListData : DatabaseStruct
    {
        public Guid playlistUid { get; set; }
        public Guid uid { get; set; }
        private IMusicCenterAPI _api;
        public InfoPlayListData(IMusicCenterAPI api)
        {
            _api = api;
        }
        public void Save()
        {
            _api.InsertData(InfoPlaylistTable, "PlaylistUid", new Dictionary<string, object>
                {
                { "PlaylistUid", playlistUid.ToString() },
                { "RecordUid", uid}
            });
        }
    }
}
