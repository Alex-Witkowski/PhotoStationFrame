namespace PhotoStationFrame.Api.Models
{
    public abstract class PhotoStationBaseResponse<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
    }
}
