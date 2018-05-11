namespace PhotoStationFrame.Api.Models
{
    public class SmartAlbumsResponse :PhotoStationBaseResponse<SmartAlbumData>
    {
    }

    public class SmartAlbumData
    {
        public int total { get; set; }
        public int offset { get; set; }
        public Smart_Album[] smart_albums { get; set; }
    }

    public class Smart_Album
    {
        public string id { get; set; }
        public string type { get; set; }
        public string thumbnail_status { get; set; }
        public string name { get; set; }
    }

}
