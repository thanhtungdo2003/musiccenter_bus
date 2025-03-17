using MuscicCenter.Storage;
using MusicBusniess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TT_DBLib.Storage;
using TT_DBLib.Storage.Database;

namespace MusicCenterAPI.Data
{
    public class RecordData : BaseModel
    {
        protected override string TableName => DatabaseStruct.RecordTable;
        protected override string KeyColumn => "RecordUid";

        public string RecordUid { get; set; }
        public string CategoryUid { get; set; }
        public string DisplayName { get; set; }
        public int Views { get; set; }
        public string ArtistUid { get; set; }
        public string Record { get; set; }
        public string Poster { get; set; }
        public string CoverPhoto { get; set; }
        public string Payfee { get; set; }
        public string? Lyrics { get; set; }
        public string? Description { get; set; }
        public ArtistData Artist { get; set; }
        public CategoryData Category { get; set; }

        public RecordData(IMusicCenterAPI api) : base(api) { }
        public RecordData(IMusicCenterAPI api, string id) : base(api, id) { }

        public RecordData() : base() { }
        public RecordData SetUp(IMusicCenterAPI api)
        {
            Artist = new ArtistData(api, Guid.Parse(ArtistUid));
            Category = new CategoryData(api, Guid.Parse(CategoryUid));
            return this;
        }
    }
}
