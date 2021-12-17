using System.Threading;
using Cysharp.Threading.Tasks;

namespace GGPlugins.GGStateMachine.Scripts.Abstract
{
    public interface IGGStateBase
    {
        /// <summary>
        /// You don't need to manually call this.
        /// </summary>
        /// <param name="stateMachine"></param>
        void SetStateMachine(IGGStateMachine stateMachine);
        /// <summary>
        /// Called when the machine starts for all states
        /// </summary>
        void OnMachineStarted();
        /// <summary>
        /// Called when the machine starts from this state (after OnMachineStarted)
        /// </summary>
        void OnMachineStartState();

        /// <summary>
        /// Called for all states after exit is requested and the last state has completed
        /// </summary>
        void OnMachineExit();
        /// <summary>
        /// The entry of this state, Exit won't be called until this is completed.
        /// Make sure you properly use the cancellation token 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask Entry(CancellationToken cancellationToken);
        /// <summary>
        /// The exit of this state, next state won't start until this is complete.
        /// Make sure you properly use the cancellation token 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask Exit(CancellationToken cancellationToken);
        /// <summary>
        /// Called after Exit state before the next state starts
        /// </summary>
        void CleanUp();
    }

    /// <inheritdoc />
    public interface IGGState : IGGStateBase
    {
        /// <summary>
        /// Called before Entry for any reinitialization functions
        /// </summary>
        void Setup();
    }

    /// <inheritdoc />
    public interface IGGState<in T> : IGGStateBase
    {
        /// <summary>
        /// Called before Entry for any reinitialization functions.
        /// The provided parameter can change each time
        /// </summary>
        /// <param name="param"> parameter </param>
        void Setup(T param);
    }


    /// <inheritdoc />
    public interface IGGState<in T, in K> : IGGStateBase
    {
        /// <summary>
        /// Called before Entry for any reinitialization functions.
        /// The provided parameters can change each time
        /// </summary>
        /// <param name="param1"> parameter </param>
        /// <param name="param2"> parameter </param>
        void Setup(T param1, K param2);
    }
    

}