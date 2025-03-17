using MuscicCenter.Storage;
using MusicBusniess;
using MusicCenterAPI.ProcedureStorage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace MusicCenterAPI.Data
{
    public class ArtistData: DatabaseStruct
    {
        public string ArtistUid { get; set; }
        public string StageName { get; set; }
        public string Avata { get; set; }
        public int Visits { get; set; }
        public ArtistData()
        {

        }

        public List<Dictionary<string, object>> Records = new List<Dictionary<string, object>>();
        private IMusicCenterAPI _api;
        public ArtistData(IMusicCenterAPI api)
        {
            _api = api;
        }
        public ArtistData(IMusicCenterAPI api, Guid uid)
        {
            _api = api;
            if (_api.getValueByKey(ArtistTable, "ArtistUid", "ArtistUid", uid.ToString()) == null) return;
            this.ArtistUid = uid.ToString();
            this.StageName = (string)_api.getValueByKey(ArtistTable, "StageName", "ArtistUid", uid.ToString());
            this.Avata = (string)_api.getValueByKey(ArtistTable, "Avata", "ArtistUid", uid.ToString());
            this.Visits = (int)_api.getValueByKey(ArtistTable, "Visits", "ArtistUid", uid.ToString());
        }
        public List<Dictionary<string, object>> getRecords()
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ArtistUid", System.Data.SqlDbType.NVarChar) {Value = ArtistUid.ToString()}
            };
            return _api.CoventToDictionarysWithDataTable(_api.ProcedureCall(ProcedureName.GET_RECORDS_FOR_ARTIST, parameters));
        }

        public void Save()
        {
            if (_api.getValueByKey(ArtistTable, "ArtistUid", "ArtistUid", ArtistUid.ToString()) == null)
            {
                _api.InsertData(ArtistTable, "ArtistUid", new Dictionary<string, object>
            {
                { "ArtistUid", ArtistUid.ToString() },
                { "StageName", StageName},
                { "Avata", Avata },
                { "Visits", Visits }
            });
            }
            else
            {
                _api.UpdateData(ArtistTable, "StageName", "ArtistUid", ArtistUid.ToString(), StageName);
                _api.UpdateData(ArtistTable, "Avata", "ArtistUid", ArtistUid.ToString(), Avata);
                _api.UpdateData(ArtistTable, "Visits", "ArtistUid", ArtistUid.ToString(), Visits);
            }
        }
    }
}
