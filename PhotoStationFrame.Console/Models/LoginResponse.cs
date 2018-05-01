namespace PhotoStationFrame.Consol.Models
{
    public class LoginResponse
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string sid { get; set; }
        public string username { get; set; }
        public bool reg_syno_user { get; set; }
        public bool is_admin { get; set; }
        public bool allow_comment { get; set; }
        public Permission permission { get; set; }
        public bool enable_face_recog { get; set; }
        public bool allow_public_share { get; set; }
        public bool allow_download { get; set; }
        public bool show_detail { get; set; }
    }

    public class Permission
    {
        public bool browse { get; set; }
        public bool upload { get; set; }
        public bool manage { get; set; }
    }

}
