using MuscicCenter.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT_DBLib.Storage;

namespace MusicBusniess.Data
{
    public class CollectionLinkRequest:BaseModel
    {
        protected override string TableName => DatabaseStruct.CollectionLinkMomoTable;
        protected override string KeyColumn => "requestId";

        public string orderInfo { get; set; }
        public string partnerCode { get; set; }
        public string redirectUrl { get; set; }
        public string ipnUrl { get; set; }
        public long amount { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public string extraData { get; set; }
        public string partnerName { get; set; }
        public string storeId { get; set; }
        public string requestType { get; set; }
        public string orderGroupId { get; set; }
        public bool autoCapture { get; set; }
        public bool active { get; set; }
        public string lang { get; set; }
        public string signature { get; set; }
        public string username { get; set; }

        public CollectionLinkRequest(IMusicCenterAPI api) : base(api) { }
        public CollectionLinkRequest(IMusicCenterAPI api, string id) : base(api, id) { }

    }

}
