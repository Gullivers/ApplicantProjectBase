using GGPlugins.GGStateMachine.Scripts.Abstract;
using GGPlugins.GGStateMachine.Scripts.Data;
using UnityEngine;
using Zenject;

namespace GGPlugins.GGStateMachine.Scripts.Installers
{
    /// <summary>
    /// The factory is used to create a new general-use state machine
    /// </summary>
    public class GGStateMachineFactory : PlaceholderFactory<IGGStateMachine>
    {
    }
    

    [CreateAssetMenu(fileName = "GGStateMachineInstaller", menuName = "GGInstallers/GGStateMachineInstaller")]
    public class GGStateMachineInstaller : ScriptableObjectInstaller<GGStateMachineInstaller>
    {
        [Header("Default Settings")]
        [SerializeField][Tooltip("If a state is requested and it's right after the same type of state (in queue or after the active one), it will be ignored")]
        private bool dontSwitchToSameState;
        
        public override void InstallBindings()
        {
            var defaultSettings = new StateMachineSettings(dontSwitchToSameState);
            Container.BindFactory<IGGStateMachine, GGStateMachineFactory>().To<IggStateMachine>().WithArguments(defaultSettings);
        }
    }
}