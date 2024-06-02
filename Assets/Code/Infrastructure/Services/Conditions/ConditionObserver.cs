using Code.Data.Enums;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;

namespace Code.Infrastructure.Services
{
    public abstract class ConditionObserver: IEntity,IGameInitListener, IGameStartListener, IGameExitListener
    {
        public abstract GameCondition Condition { get; }
        public abstract bool IsTrue { get; protected set; }

        public virtual void GameStart()
        {
            SubscribeToEvents(true);
            RefreshState();
        }

        public virtual void GameExit()
        {
            SubscribeToEvents(false);
        }

        public abstract void RefreshState();
        public abstract void GameInit();

        public abstract void SubscribeToEvents(bool flag);
    }
}