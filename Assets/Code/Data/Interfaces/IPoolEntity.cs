namespace Code.Data.Interfaces
{
    public interface  IPoolEntity
    {
        void InitEntity(params object[] parameters);
        void EnableEntity();
        void DisableEntity();
    }
}