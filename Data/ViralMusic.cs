using MusicCenterAPI.Data;

namespace MusicCenterAPI.Models
{
    public class ViralMusic
    {
        public ViralMusic(string id, int view, RecordData record)
        {
            uid = id;
            views = view;
            this.record = record;
        }
        public RecordData record { get; set; }
        public string uid { get; set; }
        public int views { get; set; }
    }
}
