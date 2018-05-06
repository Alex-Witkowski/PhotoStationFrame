namespace PhotoStationFrame.Api.Models
{
    public class ListData<T>
    {
        public int total { get; set; }
        public int offset { get; set; }
        public T[] items { get; set; }
    }
}
