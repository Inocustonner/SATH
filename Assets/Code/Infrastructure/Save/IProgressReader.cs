namespace Code.Infrastructure.Save
{
    public interface IProgressReader
    {
        void LoadProgress(SavedData playerProgress);
    }
}