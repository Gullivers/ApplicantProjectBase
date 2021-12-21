using System;
using GGPlugins.GGStateMachine.Scripts.Abstract;
using UnityEngine;

namespace GGPlugins.GGStateMachine.Scripts
{
    public class StateMachineEventHandle : IStateMachineEventHandle
    {
        private event Action<StateInfo> OnBeforeStart;
        private event Action<StateInfo> OnAfterStart;
        private event Action<StateInfo> OnBeforeExit;
        private event Action<StateInfo> OnAfterExit;
        private event Action<StateInfo> OnMachineStart;
        private event Action OnMachineExit;
        private event Action<StateInfo> OnStateChange;
        private event Action<StateInfo, StateRequestType> OnStateRequest;

        #region Callers

        public void CallBeforeStart(StateInfo info)
        {
            OnBeforeStart?.Invoke(info);
        }

        public void CallAfterStart(StateInfo info)
        {
            OnAfterStart?.Invoke(info);
        }

        public void CallBeforeExit(StateInfo info)
        {
            OnBeforeExit?.Invoke(info);
        }

        public void CallAfterExit(StateInfo info)
        {
            OnAfterExit?.Invoke(info);
        }

        public void CallMachineStarted(StateInfo info)
        {
            OnMachineStart?.Invoke(info);
        }

        public void CallMachineExit()
        {
            OnMachineExit?.Invoke();
        }

        public void CallCurrentStateChanged(StateInfo info)
        {
            OnStateChange?.Invoke(info);
        }

        public void CallStateChangeRequested(StateInfo info, StateRequestType type)
        {
            OnStateRequest?.Invoke(info, type);
        }

        #endregion

        #region Interface

        public Action OnStateMachineStarted(Action<StateInfo> callback)
        {
            OnMachineStart += callback;
            return () =>
            {
                try
                {
                    OnMachineStart -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnStateMachineExit(Action callback)
        {
            OnMachineExit += callback;
            return () =>
            {
                try
                {
                    OnMachineExit -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnBeforeStateEntry(Action<StateInfo> callback)
        {
            OnBeforeStart += callback;
            return () =>
            {
                try
                {
                    OnBeforeStart -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnAfterStateEntry(Action<StateInfo> callback)
        {
            OnAfterStart += callback;
            return () =>
            {
                try
                {
                    OnAfterStart -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnBeforeStateExit(Action<StateInfo> callback)
        {
            OnBeforeExit += callback;
            return () =>
            {
                try
                {
                    OnBeforeExit -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnAfterStateExit(Action<StateInfo> callback)
        {
            OnAfterExit += callback;
            return () =>
            {
                try
                {
                    OnAfterExit -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnCurrentStateChanged(Action<StateInfo> callback)
        {
            OnStateChange += callback;
            return () =>
            {
                try
                {
                    OnStateChange -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public Action OnStateRequested(Action<StateInfo, StateRequestType> callback)
        {
            OnStateRequest += callback;
            return () =>
            {
                try
                {
                    OnStateRequest -= callback;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to remove event from listener");
                }
            };
        }

        public void ClearAllListeners()
        {
            OnBeforeStart = null;
            OnAfterStart = null;
            OnBeforeExit = null;
            OnAfterExit = null;
            OnMachineStart = null;
            OnMachineExit = null;
            OnStateChange = null;
            OnStateRequest = null;
        }

        #endregion
    }
}