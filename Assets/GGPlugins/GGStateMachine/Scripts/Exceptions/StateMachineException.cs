using System;

namespace GGPlugins.GGStateMachine.Scripts.Exceptions
{
    public class StateMachineException : ArgumentException
    {
        public StateMachineException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}
