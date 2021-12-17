﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GGPlugins.GGLogger;
using GGPlugins.GGStateMachine.Scripts.Abstract;
using GGPlugins.GGStateMachine.Scripts.Data;
using GGPlugins.GGStateMachine.Scripts.Exceptions;

#pragma warning disable 4014

namespace GGPlugins.GGStateMachine.Scripts
{
  
    internal class IggStateMachine : IGGStateMachine
    {
        private readonly struct QueuedState
        {
            public readonly string ID;
            public readonly object[] Parameters;

            public QueuedState(string id)
            {
                ID = id;
                Parameters = new object[0];
            }
            public QueuedState(string id, object[] parameters)
            {
                ID = id;
                Parameters = parameters;
            }
        }
        private readonly IGGLogger _logger;
        private StateMachineSettings _settings;
        
        private readonly Dictionary<string, StateWrapper> _stateMapping;
        private readonly Queue<QueuedState> _stateQueue;
        private readonly Stack<QueuedState> _history;
        private CancellationTokenSource _cts;
        private bool _stateMachineRunning;
        private bool _aStateLoopIsActive;
        private bool _exitRequested;

        public IggStateMachine(StateMachineSettings settings, IGGLogger logger)
        {
            _settings = settings;
            _logger = logger;
            _stateMapping = new Dictionary<string, StateWrapper>();
            _stateMachineRunning = false;
            _stateQueue = new Queue<QueuedState>();
            _aStateLoopIsActive = false;
            _history = new Stack<QueuedState>();
        }

        public IGGStateMachine RegisterUniqueState(IGGStateBase state,string identifier = null)
        {
            if (_stateMachineRunning)
            {
                _logger.W("StateMachine: Can't add new state while the state machine is running.");
                return this;
            }

            var id = !string.IsNullOrEmpty(identifier) ? identifier : GetTypeString(state.GetType());
            if (_stateMapping.ContainsKey(id))
            {
                _logger.W("StateMachine: Attempted to add a state machine but a state with the same identifier was added before. If you're adding two instances of the same state class, make sure you input an identifier to separate them.");
                return this;
            }
            state.SetStateMachine(this);
            _stateMapping.Add(id,new StateWrapper(state));
            return this;
        }

        private string GetTypeString(Type type)
        {
            return type.ToString();
        }

        private bool InteractionCheck()
        {
            if (!_stateMachineRunning)
            {
                _logger.W("StateMachine: Cannot interact with machine until the state machine starts.");
                return false;
            }

            if (_exitRequested)
            {
                _logger.W("StateMachine: Cannot interact with machine if exit was requested");
                return false;
            }

            return true;
        }

        private void EnsureIdentifier(string identifier)
        {
            if (!_stateMapping.ContainsKey(identifier)) throw new StateMachineException($"Invalid state identifier: {identifier}","identifier");
        }

        public void SetSettings(StateMachineSettings settings)
        {
            _settings = settings;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public void StartStateMachine(string entryStateIdentifier,params object[] parameters)
        {
            if (_exitRequested)
            {
                _logger.W("StateMachine: Exit not yet complete!");
                return;
            }
            if (_stateMachineRunning)
            {
                _logger.W("StateMachine: Cannot start machine twice.");
                return;
            }
            
            EnsureIdentifier(entryStateIdentifier);

            var stateElement = new QueuedState(entryStateIdentifier, parameters);
            _stateMachineRunning = true;
            _stateQueue.Enqueue(stateElement);
            _cts = new CancellationTokenSource();
            foreach (var pair in _stateMapping)
            {
                var result = pair.Value.OnMachineStarted();
                CheckResult(result,"OnMachineStarted",pair.Key);
                if (pair.Key == entryStateIdentifier)
                {
                    var startStateResult = pair.Value.OnMachineStartState();
                    CheckResult(startStateResult,"OnMachineStartState",pair.Key);
                }
            }
            RunNextState();
        }

        public void StartStateMachine(Type type)
        {
            StartStateMachine(GetTypeString(type));
        }

        public void StartStateMachine<Param1>(Type type, Param1 param)
        {
            StartStateMachine(GetTypeString(type),param);
        }

        public void StartStateMachine<Param1, Param2>(Type type, Param1 param1, Param2 param2)
        {
            StartStateMachine(GetTypeString(type),param1,param2);
        }

        public void StartStateMachine<T>() where T : IGGState
        {
            StartStateMachine(typeof(T));
        }

        public void StartStateMachine<T, Param1>(Param1 param) where T : IGGState<Param1>
        {
            StartStateMachine(typeof(T),param);
        }

        public void StartStateMachine<T, Param1, Param2>(Param1 param1, Param2 param2) where T : IGGState<Param1, Param2>
        {
            StartStateMachine(typeof(T),param1,param2);
        }

        public void EnqueueState(string identifier,params object[] parameters)
        {
            if (!InteractionCheck()) return;
            
            EnsureIdentifier(identifier);
            _stateQueue.Enqueue( new QueuedState(identifier,parameters));
        }

        public void EnqueueState(Type type)
        {
            EnqueueState(GetTypeString(type));
        }

        public void EnqueueState<Param1>(Type type, Param1 param)
        {
            EnqueueState(GetTypeString(type),param);
        }

        public void EnqueueState<Param1, Param2>(Type type, Param1 param1, Param2 param2)
        {
            EnqueueState(GetTypeString(type),param1,param2);
        }

        public void EnqueueState<T>() where T : IGGState
        {
            EnqueueState(typeof(T));
        }

        public void EnqueueState<T, Param1>(Param1 param) where T : IGGState<Param1>
        {
            EnqueueState(typeof(T),param);
        }

        public void EnqueueState<T, Param1, Param2>(Param1 param1, Param2 param2) where T : IGGState<Param1, Param2>
        {
            EnqueueState(typeof(T),param1,param2);
        }

        public void SwitchToState(string identifier,params object[] parameters)
        {
            if (!InteractionCheck()) return;
            EnsureIdentifier(identifier);
            _stateQueue.Clear();
            _stateQueue.Enqueue(new QueuedState(identifier,parameters));
        }

        public void SwitchToState(Type type)
        {
            SwitchToState(GetTypeString(type));
        }

        public void SwitchToState<Param1>(Type type, Param1 param)
        {
            SwitchToState(GetTypeString(type), param);
        }

        public void SwitchToState<Param1, Param2>(Type type, Param1 param1, Param2 param2)
        {
            SwitchToState(GetTypeString(type), param1, param2);
        }

        public void SwitchToState<T>() where T : IGGState
        {
            SwitchToState(typeof(T));
        }

        public void SwitchToState<T, Param1>(Param1 param) where T : IGGState<Param1>
        {
            SwitchToState(typeof(T),param);
        }

        public void SwitchToState<T, Param1, Param2>(Param1 param1, Param2 param2) where T : IGGState<Param1, Param2>
        {
            SwitchToState(typeof(T),param1,param2);
        }

        public void EnqueuePreviousState()
        {
            if (!InteractionCheck()) return;
            if (_history.Count == 0)
            {
                _logger.W("StateMachine: No previous state found to switch to");
                return;
            }

            var prev = _history.Pop();
            EnqueueState(prev.ID,prev.Parameters);
        }

        public void SwitchToPreviousState()
        {
            if (!InteractionCheck()) return;
            if (_history.Count == 0)
            {
                _logger.W("StateMachine: No previous state found to switch to");
                return;
            }

            var prev = _history.Pop();
            SwitchToState(prev.ID,prev.Parameters);
            
        }

        public void ClearQueue()
        {
            _stateQueue.Clear();
        }

        public void RequestExit()
        {
            if (!_stateMachineRunning)
            {
                _logger.W("StateMachine: Cannot exit machine if it is not running.");
                return;
            }

            if (_exitRequested)
            {
                _logger.W("StateMachine: Exit was already requested!");
                return;
            }
            _stateQueue.Clear();
            _exitRequested = true;
        }

        public async UniTask WaitUntilMachineExit()
        {
            await UniTask.WaitUntil(() => _stateMachineRunning == false);
        }

        private void CheckResult(StateResult result,string stage, string stateId)
        {
            if (result.ErrorOccured)
            {
                _logger.E($"StateMachine: An exception occured during execution of state {stateId} - {stage} phase. Will continue normally. {result.ExceptionThrown} ");
            }
        }

        private string _activeState;
        private async UniTask RunNextState()
        {
            if (_aStateLoopIsActive) return;
            _aStateLoopIsActive = true;
            if (_stateQueue.Count == 0) await UniTask.WaitUntil(() => _stateQueue.Count > 0);

            var stateElement = _stateQueue.Dequeue();
            var stateId = stateElement.ID;
            var state = _stateMapping[stateId];
            if (_settings.DontSwitchToSameState && stateId == _activeState)
            {
                RunNextState();
                return;
            }
            _activeState = stateId;
            StateResult result;
            
            result = state.Setup(stateElement.Parameters);
            _history.Push(stateElement);
            CheckResult(result,"Setup",stateId);
            result = await state.Entry(_cts.Token).AttachExternalCancellation(_cts.Token);
            CheckResult(result,"Entry",stateId);
            await UniTask.WaitUntil(() => _stateQueue.Count > 0 || _exitRequested).AttachExternalCancellation(_cts.Token);
            result =  await state.Exit(_cts.Token).AttachExternalCancellation(_cts.Token);
            CheckResult(result,"Exit",stateId);
            result = state.CleanUp();
            CheckResult(result,"CleanUp",stateId);
            _aStateLoopIsActive = false;
            if (_exitRequested)
            {
                _exitRequested = false;
                _stateMachineRunning = false;
                _history.Clear();
                _activeState = "";
                foreach (var pair in _stateMapping)
                {
                    result = pair.Value.OnMachineExit();
                    CheckResult(result,"OnMachineExit",pair.Key);
                }
                return;
            }
            RunNextState();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
