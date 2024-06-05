namespace Code.Data.Interfaces
{
    public interface IPartListener
    {
    }

    public interface IPartStartListener: IPartListener
    {
        void PartStart();
    }
    
    public interface IPartTickListener: IPartListener
    {
        void PartTick();
    }

    public interface IPartFixedTickListener : IPartListener
    {
        void PartFixedTick();
    }

    public interface IPartExitListener : IPartListener
    {
        void PartExit();
    }
}