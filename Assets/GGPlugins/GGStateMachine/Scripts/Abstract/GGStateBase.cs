using System.Threading;
using Cysharp.Threading.Tasks;

namespace GGPlugins.GGStateMachine.Scripts.Abstract
{
    /// <inheritdoc />
    public abstract class GGStateBase : IGGState
    {
        private IGGStateMachine _stateMachine;

        protected IGGStateMachine StateMachine => _stateMachine;
        
        public void SetStateMachine(IGGStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnMachineStarted(){}
        public virtual void OnMachineStartState(){}

        public virtual void OnMachineExit(){}

        public abstract void Setup();
        
        public abstract UniTask Entry(CancellationToken cancellationToken);


        public abstract UniTask Exit(CancellationToken cancellationToken);
        
        public abstract void CleanUp();
    }
    

    /// <inheritdoc />
    public abstract class GGStateBase<T> : IGGState<T>
    {
        private IGGStateMachine _stateMachine;

        protected IGGStateMachine StateMachine => _stateMachine;
        
        public void SetStateMachine(IGGStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnMachineStarted(){}
        public virtual void OnMachineStartState(){}

        public virtual void OnMachineExit(){}

        public abstract void Setup(T param);
        
        public abstract UniTask Entry(CancellationToken cancellationToken);


        public abstract UniTask Exit(CancellationToken cancellationToken);
        
        public abstract void CleanUp();
    }
    

    /// <inheritdoc />
    public abstract class GGStateBase<T,K> : IGGState<T,K>
    {
        private IGGStateMachine _stateMachine;

        protected IGGStateMachine StateMachine => _stateMachine;
        
        public void SetStateMachine(IGGStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnMachineStarted(){}
        public virtual void OnMachineStartState(){}

        public virtual void OnMachineExit(){}

        public abstract void Setup(T param1,K param2);
        
        public abstract UniTask Entry(CancellationToken cancellationToken);


        public abstract UniTask Exit(CancellationToken cancellationToken);
        
        public abstract void CleanUp();
    }
}
