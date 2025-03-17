using MuscicCenter.Storage;
using MusicBusniess;

namespace MusicCenterAPI.Data
{
    public class CategoryData : DatabaseStruct
    {
        public string categoryUid { get; set; }
        public string displayName { get; set; }
        private IMusicCenterAPI _api;
        public CategoryData()
        {

        }
        public CategoryData(IMusicCenterAPI api)
        {
            _api = api;
        }
        public CategoryData(IMusicCenterAPI api, Guid uid)
        {
            _api = api;
            if (_api.getValueByKey(CategoryTable, "CategoryUid", "CategoryUid", uid.ToString()) == null)
            {
                categoryUid = null;
                return;
            }
            categoryUid = uid.ToString();
            this.displayName = (string)_api.getValueByKey(CategoryTable, "DisplayName", "CategoryUid", uid.ToString());
        }
        public void Save()
        {
            if (_api.getValueByKey(CategoryTable, "CategoryUid", "CategoryUid", categoryUid.ToString()) == null)
            {
                _api.InsertData(CategoryTable, "CategoryUid", new Dictionary<string, object>
                {
                { "CategoryUid", categoryUid.ToString() },
                { "DisplayName", displayName}
            });
            }
            else
            {
                _api.UpdateData(CategoryTable, "CategoryUid", "CategoryUid", categoryUid.ToString(), categoryUid.ToString());
                _api.UpdateData(CategoryTable, "DisplayName", "CategoryUid", categoryUid.ToString(), displayName);
            }
        }
    }
}
