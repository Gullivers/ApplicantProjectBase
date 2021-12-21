using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GGPlugins.GGStateMachine.Scripts.Abstract;

namespace GGPlugins.GGStateMachine.Scripts
{
    internal readonly struct StateResult
    {
        internal static StateResult Success => new StateResult(false);
        internal static StateResult Fail(Exception e) => new StateResult(true,e);
        internal readonly bool ErrorOccured;
        internal readonly Exception ExceptionThrown;

        private StateResult(bool errorOccured, Exception exceptionThrown = null)
        {
            ErrorOccured = errorOccured;
            ExceptionThrown = exceptionThrown;
        }
    }
    /// <summary>
    /// Serves as an error handling wrapper for any provided state
    /// </summary>
    internal class StateWrapper
    {
        private readonly IGGStateBase _wrappedState;

        public StateWrapper(IGGStateBase wrappedState)
        {
            _wrappedState = wrappedState ?? throw new ArgumentNullException(nameof(wrappedState),"StateWrapper cannot have null state as the wrapped state");
        }

        public Type GetStateType() => _wrappedState.GetType();

        public StateResult OnMachineStarted()
        {
            try
            {
                _wrappedState.OnMachineStarted();
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }
        
        public StateResult OnMachineStartState()
        {
            try
            {
                _wrappedState.OnMachineStartState();
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }

        public StateResult OnMachineExit()
        {
            try
            {
                _wrappedState.OnMachineExit();
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }

        public StateResult Setup(params object[] parameters)
        {
            try
            {
                var type = _wrappedState.GetType();
                {
                    var state = type.GetInterface(nameof(IGGState));
                    if (state!=null)
                    {
                        ((IGGState)_wrappedState).Setup();
                        return StateResult.Success;
                    }
                }
                {
                    bool isSingleGeneric = type.GetInterfaces().Any(x =>
                        x.IsGenericType &&
                        x.GetGenericTypeDefinition() == typeof(IGGState<>));
                    if (isSingleGeneric)
                    {
                        type.GetMethod(nameof(IGGState<object>.Setup))?.Invoke(_wrappedState,parameters);
                        return StateResult.Success;
                    }
                }
                {
                    bool isSingleGeneric = type.GetInterfaces().Any(x =>
                        x.IsGenericType &&
                        x.GetGenericTypeDefinition() == typeof(IGGState<,>));
                    if (isSingleGeneric)
                    {
                        type.GetMethod(nameof(IGGState<object>.Setup))?.Invoke(_wrappedState,parameters);
                        return StateResult.Success;
                    }
                }
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }

        public async UniTask<StateResult> Entry(CancellationToken cancellationToken)
        {
            try
            {
                await _wrappedState.Entry(cancellationToken);
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }

        public async UniTask<StateResult> Exit(CancellationToken cancellationToken)
        {
            try
            {
                await _wrappedState.Exit(cancellationToken);
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }

        public StateResult CleanUp()
        {
            try
            {
                _wrappedState.CleanUp();
            }
            catch (Exception e)
            {
                return StateResult.Fail(e);
            }

            return StateResult.Success;
        }
    }
}
