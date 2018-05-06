using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoStationFrame.Api.Models
{

    public class ListAlbumsResponse : PhotoStationBaseResponse<ListData<AlbumItem>>
    {
    }

    public class AlbumItem
    {
        public AlbumInfo info { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public object additional { get; set; }
        public string thumbnail_status { get; set; }
    }

    public class AlbumInfo
    {
        public string sharepath { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int hits { get; set; }
        public string type { get; set; }
        public bool conversion { get; set; }
        public bool allow_comment { get; set; }
        public bool allow_embed { get; set; }
    }

}
