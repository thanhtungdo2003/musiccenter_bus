using MusicBusniess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT_DBLib.Storage.Database;

namespace TT_DBLib.Storage
{
    public abstract class BaseModel
    {
        protected IMusicCenterAPI _api;
        protected abstract string TableName { get; }
        protected abstract string KeyColumn { get; }

        public string Key { get; set; }
        public BaseModel()
        {

        }

        public BaseModel(IMusicCenterAPI api)
        {
            _api = api;
        }

        public BaseModel(IMusicCenterAPI api, string key)
        {
            _api = api;
            Key = key;
            LoadData();
        }

        private void LoadData()
        {
            foreach (var pro in this.GetType().GetProperties())
            {
                if (pro.CanWrite && pro.Name != "Key" && pro.Name != "KeyColumn" && pro.Name != "TableName")
                {
                    var value = _api.getValueByKey(TableName, pro.Name, KeyColumn, Key);
                    if (value != null && value != DBNull.Value)
                    {
                        pro.SetValue(this, value);
                    }
                }
            }
        }
        public bool Exist()
        {
            if (_api.getValueByKey(TableName, KeyColumn, KeyColumn, Key) != null)
            {
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            if (Exist())
            {
                return _api.DeleteDataByValue(TableName, KeyColumn, Key);
            }
            return false;
        }

        public void Save()
        {
            if (!Exist())
            {
                // Insert logic
                var datas = new Dictionary<string, object>();
                foreach (var pro in this.GetType().GetProperties())
                {
                    if (pro.CanWrite && pro.Name != "Key" && pro.Name != "KeyColumn" && pro.Name != "TableName")
                    {
                        var value = pro.GetValue(this);
                        if (value != null)
                        {
                            datas[pro.Name] = value;
                        }
                    }
                }
                _api.InsertData(TableName, KeyColumn, datas);
            }
            else
            {
                // Update logic
                foreach (var pro in this.GetType().GetProperties())
                {
                    if (pro.CanWrite && pro.Name != "Key" && pro.Name != "KeyColumn" && pro.Name != "TableName")
                    {
                        var value = pro.GetValue(this);
                        if (value != null)
                        {
                            _api.UpdateData(TableName, pro.Name, KeyColumn, Key, value);
                        }
                    }
                }
            }
        }
    }
}
