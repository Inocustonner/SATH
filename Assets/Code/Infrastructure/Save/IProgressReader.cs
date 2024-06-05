using Code.Data.StaticData;

namespace Code.Infrastructure.Save
{
    public interface IProgressReader
    {
        void LoadProgress(SavedData playerProgress);
    }
}