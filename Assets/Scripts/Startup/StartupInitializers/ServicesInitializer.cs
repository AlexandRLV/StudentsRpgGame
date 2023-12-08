using Cysharp.Threading.Tasks;
using GameSettings;
using Services;
using Services.DI;
using Services.SoundsSystem;
using UnityEngine;

namespace Startup.StartupInitializers
{
    public class ServicesInitializer : IInitializer
    {
        public UniTask Initialize()
        {
            var messageBroker = new MessageBroker();
            GameContainer.Common.Register(messageBroker);

            var gameSettings = new GameSettingsManager();
            GameContainer.Common.Register(gameSettings);
            
            var soundsSystemPrefab = Resources.Load<SoundsSystem>("Prefabs/SoundsSystem");
            var soundsSystem = GameContainer.InstantiateAndResolve(soundsSystemPrefab);
            GameContainer.Common.Register(soundsSystem);
            
            return UniTask.CompletedTask;
        }

        public UniTask Dispose()
        {
            return UniTask.CompletedTask;
        }
    }
}