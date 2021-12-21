using System;
using Cysharp.Threading.Tasks;
using GGPlugins.GGStateMachine.Scripts.Data;

namespace GGPlugins.GGStateMachine.Scripts.Abstract
{
    /// <summary>
    /// A state machine where states can be added and a switch to any state can be requested at any time.
    /// </summary>
    /// <inheritdoc />
    public interface IGGStateMachine : IDisposable
    {
        /// <summary>
        /// Adds a state to the state machine. If the same type of state has been added before, make sure to input an unique identifier.
        /// The type of the state is converted to a string to be used as an identifier if no identifier was given.
        /// </summary>
        /// <param name="state"> The state to be added </param>
        /// <param name="identifier"> Unique identifier if necessary</param>
        /// <returns></returns>
        IGGStateMachine RegisterUniqueState(IGGStateBase state,string identifier = null);

        /// <summary>
        /// Updates the settings of the machine
        /// </summary>
        /// <param name="settings"></param>
        void SetSettings(StateMachineSettings settings);

        /// <summary>
        /// Starts the machine with the given identifier
        /// </summary>
        /// <param name="entryStateIdentifier"> Identifier of the entry state </param>
        /// <param name="parameters"></param>
        void StartStateMachine(string entryStateIdentifier,params object[] parameters);
        /// <summary>
        /// Starts the machine with the state of given type. Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="type"> Type of the entry state </param>
        void StartStateMachine(Type type);

        /// <summary>
        /// Starts the machine with the state of given type and the given parameter. Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="type"> Type of the entry state </param>
        /// <param name="param"></param>
        void StartStateMachine<Param1>(Type type,Param1 param);

        /// <summary>
        /// Starts the machine with the state of given type and the given parameters. Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="type"> Type of the entry state </param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void StartStateMachine<Param1,Param2>(Type type,Param1 param1, Param2 param2);
        /// <summary>
        /// Starts the machine with the state of given type. Won't work if that state was added with an explicit string identifier.
        /// </summary>
        void StartStateMachine<T>() where T : IGGState;
        /// <summary>
        /// Starts the machine with the state of given type and the given parameter. Won't work if that state was added with an explicit string identifier.
        /// </summary>
        void StartStateMachine<T,Param1>(Param1 param) where T : IGGState<Param1>;
        /// <summary>
        /// Starts the machine with the state of given type and the given parameters. Won't work if that state was added with an explicit string identifier.
        /// </summary>
        void StartStateMachine<T,Param1,Param2>(Param1 param1, Param2 param2) where T : IGGState<Param1,Param2>;

        /// <summary>
        /// Requests the machine to switch to the state with the given identifier. The current state will have to exit first. Calling this multiple times will queue them.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="parameters"></param>
        void EnqueueState(string identifier,params object[] parameters);
        /// <summary>
        /// Requests the machine to switch to the state of the given type. The current state will have to exit first. Calling this multiple times will queue them.
        /// Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="type"></param>
        void EnqueueState(Type type);

        /// <summary>
        /// Requests the machine to switch to the state of the given type and the given parameter. The current state will have to exit first. Calling this multiple times will queue them.
        /// Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        void EnqueueState<Param1>(Type type,Param1 param);

        /// <summary>
        /// Requests the machine to switch to the state of the given type and the given parameters. The current state will have to exit first. Calling this multiple times will queue them.
        /// Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void EnqueueState<Param1,Param2>(Type type,Param1 param1, Param2 param2);

        /// <summary>
        /// Requests the machine to switch to the state of the given type. The current state will have to exit first. Calling this multiple times will queue them.
        /// Won't work if that state was added with an explicit string identifier.
        /// </summary>
        void EnqueueState<T>() where T : IGGState;

        /// <summary>
        /// Requests the machine to switch to the state of the given type and the given parameter. The current state will have to exit first. Calling this multiple times will queue them.
        /// Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="param"></param>
        void EnqueueState<T,Param1>(Param1 param) where T : IGGState<Param1>;

        /// <summary>
        /// Requests the machine to switch to the state of the given type and the given parameters. The current state will have to exit first. Calling this multiple times will queue them.
        /// Won't work if that state was added with an explicit string identifier.
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void EnqueueState<T,Param1,Param2>(Param1 param1, Param2 param2) where T : IGGState<Param1,Param2>;
        
        
        
        /// <summary>
        /// Clears the requested state queue and exits out of the active state.
        /// </summary>
        void RequestExit();
        /// <summary>
        /// Returns an awaitable UniTask to wait until the machine has fully exit.
        /// </summary>
        /// <returns></returns>
        UniTask WaitUntilMachineExit();

        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="parameters"></param>
        void SwitchToState(string identifier,params object[] parameters);
        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        /// <param name="type"></param>
        void SwitchToState(Type type);

        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        void SwitchToState<Param1>(Type type,Param1 param);

        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void SwitchToState<Param1,Param2>(Type type,Param1 param1, Param2 param2);
        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        void SwitchToState<T>() where T : IGGState;
        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        void SwitchToState<T,Param1>(Param1 param) where T : IGGState<Param1>;
        /// <summary>
        /// Requests the machine to switch to this state without queue. The current state will have to exit first.
        /// The previous requests will all be ignored (the queue will be cleared)
        /// </summary>
        void SwitchToState<T,Param1,Param2>(Param1 param1, Param2 param2) where T : IGGState<Param1,Param2>;
        
        /// <summary>
        /// Enqueues whatever the last state was with the same parameters
        /// </summary>
        void EnqueuePreviousState();
        /// <summary>
        /// Switches to whatever the last state was with the same parameters. The queue will be cleared. The current state will have to exit first.
        /// </summary>
        void SwitchToPreviousState();

        /// <summary>
        /// Clears the queue but the current action won't be cleared.
        /// </summary>
        void ClearQueue();

        /// <summary>
        /// Returns info of currently active state
        /// </summary>
        /// <returns></returns>
        StateInfo GetCurrentState();
        /// <summary>
        /// Checks if the given identifier matches the currently active state
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        bool CheckCurrentState(string identifier);
        /// <summary>
        /// Checks if the type matches the currently active state.
        /// This uses the types name to check for identifier. Make sure to provide identifiers for your states IF they're not unique.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckCurrentState(Type type);
        /// <summary>
        /// Checks if the type matches the currently active state.
        /// This uses the types name to check for identifier. Make sure to provide identifiers for your states IF they're not unique.
        /// </summary>
        /// <returns></returns>
        bool CheckCurrentState<T>() where T : IGGState;
        /// <summary>
        /// Creates a new event handle instance which is used for registering to events.
        /// </summary>
        /// <returns></returns>
        IStateMachineEventHandle RequestEventHandle();

    }
}