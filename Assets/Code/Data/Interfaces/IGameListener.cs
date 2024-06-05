namespace Code.Data.Interfaces
{
    public interface IGameListener
    {
        
    }
    
    internal interface IGameInitListener: IGameListener
    {
        void GameInit();
    }

    public interface IGameLoadListener : IGameListener
    {
        void GameLoad();
    }
    
    public interface IGameStartListener : IGameListener
    {
        void GameStart();
    }

    public interface IGameTickListener : IGameListener
    {
        void GameTick();
    }
    
    public interface IGameFixedTickListener : IGameListener
    {
        void GameFixedTick();
    }
    
    public interface IGameExitListener : IGameListener
    {
        void GameExit();
    }
    
}