namespace Code.Infrastructure.Polls
{
    public interface  IPoolEntity
    {
        void InitEntity(params object[] parameters);
        void EnableEntity();
        void DisableEntity();
    }
}