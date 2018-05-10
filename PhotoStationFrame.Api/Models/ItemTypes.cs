using System;

namespace PhotoStationFrame.Api.Models
{
    [Flags]
    public enum ItemTypes
    {
        album = 1,
        photo = 2,
        video = 4
    }
}
