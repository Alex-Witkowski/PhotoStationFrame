using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoStationFrame.Api.Models
{

    public class ListAlbumsResponse
    {
        public bool success { get; set; }
        public ListAlbumsData data { get; set; }
    }

    public class ListAlbumsData
    {
        public int total { get; set; }
        public int offset { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public Info info { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public object additional { get; set; }
        public string thumbnail_status { get; set; }
    }

    public class Info
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
