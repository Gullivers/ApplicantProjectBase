using System;

namespace GGPlugins.GGStateMachine.Scripts.Data
{
    [Serializable]
    public readonly struct StateMachineSettings
    {
        /// <summary>
        /// If this is true, the state machine won't exit and reenter a state if the same one was requested.
        /// </summary>
        public readonly bool DontSwitchToSameState;

        public StateMachineSettings(bool dontSwitchToSameState)
        {
            DontSwitchToSameState = dontSwitchToSameState;
        }
    }
}
