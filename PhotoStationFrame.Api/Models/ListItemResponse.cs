namespace PhotoStationFrame.Api.Models
{

    public class ListItemResponse:PhotoStationBaseResponse<ListData<PhotoItem>>
    {
    }
    public class PhotoItem
    {
        public string id { get; set; }
        public string type { get; set; }
        public PhotoInfo info { get; set; }
        public Additional additional { get; set; }
        public string thumbnail_status { get; set; }
    }

    public class PhotoInfo
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string createdate { get; set; }
        public string takendate { get; set; }
        public int size { get; set; }
        public int resolutionx { get; set; }
        public int resolutiony { get; set; }
        public bool rotated { get; set; }
        public int rotate_version { get; set; }
        public int rotation { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
        public int rating { get; set; }
    }

    public class Additional
    {
        public Photo_Exif photo_exif { get; set; }
        public string file_location { get; set; }
        public Thumb_Size thumb_size { get; set; }
    }

    public class Photo_Exif
    {
        public string takendate { get; set; }
        public string camera { get; set; }
        public string camera_model { get; set; }
        public string exposure { get; set; }
        public string aperture { get; set; }
        public int iso { get; set; }
        public Gps gps { get; set; }
        public string focal_length { get; set; }
        public string lens { get; set; }
        public string flash { get; set; }
    }

    public class Gps
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class Thumb_Size
    {
        public Size preview { get; set; }
        public Size small { get; set; }
        public Size large { get; set; }
        public string sig { get; set; }
    }

    public class Size
    {
        public int resolutionx { get; set; }
        public int resolutiony { get; set; }
        public int mtime { get; set; }
    }


}
