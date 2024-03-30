using Data;
namespace Infrastructure.Services.SaveLoad
{
    public interface ISavedLoadService : IService
    {
        void SaveProgress();

        PlayerProgress LoadProgress();
    }
}
