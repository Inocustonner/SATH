using Code.Data.StaticData;

namespace Code.Infrastructure.Save
{
    public interface IProgressWriter : IProgressReader
    {
        void UpdateProgress(SavedData playerProgress);
    }
}