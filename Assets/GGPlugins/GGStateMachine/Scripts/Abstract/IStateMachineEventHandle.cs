using System;

namespace GGPlugins.GGStateMachine.Scripts.Abstract
{
    public readonly struct StateInfo
    {
        /// <summary>
        /// The type of the current state, this type inherits from IGGState
        /// </summary>
        public readonly Type Type;
        /// <summary>
        /// The identifier of this state, if an identifier isn't given during registry this is equal to the name of the type
        /// </summary>
        public readonly string Identifier;

        public StateInfo(Type type, string identifier)
        {
            this.Type = type;
            Identifier = identifier;
        }
    }

    public enum StateRequestType
    {
        Enqueue,
        Switch
    }
    public interface IStateMachineEventHandle
    {
        /// <summary>
        /// Called when the state machine starts. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback">Called with the starting state info</param>
        /// <returns>An action which removes the listener</returns>
        Action OnStateMachineStarted(Action<StateInfo> callback);
        
        /// <summary>
        /// Called when the state machine ends. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>An action which removes the listener</returns>
        Action OnStateMachineExit(Action callback);

        /// <summary>
        /// Called before the entry of a state is called. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback">Called with the state info</param>
        /// <returns>An action which removes the listener</returns>
        Action OnBeforeStateEntry(Action<StateInfo> callback);
        
        /// <summary>
        /// Called when the entry of a state is finished. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback">Called with the state info</param>
        /// <returns>An action which removes the listener</returns>
        Action OnAfterStateEntry(Action<StateInfo> callback);
        
        /// <summary>
        /// Called before the exit of a state is called. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback">Called with the state info</param>
        /// <returns>An action which removes the listener</returns>
        Action OnBeforeStateExit(Action<StateInfo> callback);
        
        /// <summary>
        /// Called when the exit of a state is finished. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback">Called with the state info</param>
        /// <returns>An action which removes the listener</returns>
        Action OnAfterStateExit(Action<StateInfo> callback);
        
        /// <summary>
        /// Called before the entry of a new state. Invoke the returned action to stop listening
        /// This is the same thing as OnAfterStateEntry
        /// </summary>
        /// <param name="callback">Called with the state info</param>
        /// <returns>An action which removes the listener</returns>
        Action OnCurrentStateChanged(Action<StateInfo> callback);

        
        /// <summary>
        /// Called when a new state is requested. Invoke the returned action to stop listening
        /// </summary>
        /// <param name="callback">Called with the state info and request type</param>
        /// <returns>An action which removes the listener</returns>
        Action OnStateRequested(Action<StateInfo,StateRequestType> callback);
        
        /// <summary>
        /// Clears all current listeners. This instance of IStateMachineEventHandle is unique to the requester, it will only clear events directly requested from this instance.
        /// </summary>
        void ClearAllListeners();
    }
}