using System.Threading.Tasks;

namespace PhotoStationFrame.Uwp.Settings
{
    public interface ISettingsHelper
    {
        Task SaveAsync(PhotoFrameSettings settings);
        Task<PhotoFrameSettings> LoadAsync();
    }
}