using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services.DI;
using Services.SoundsSystem;
using Services.WindowsSystem;
using Startup.GameplayInitializers;
using Startup.StartupInitializers;
using Ui.Windows;
using UnityEngine;

namespace Startup
{
    public class Startup : MonoBehaviour
    {
        private List<IInitializer> _startupInitializers = new List<IInitializer>
        {
            new ServicesInitializer(),
            new UiInitializer()
        };

        private List<IInitializer> _gameplayInitializers = new List<IInitializer>()
        {
            new GameMapInitializer(),
        };
        
        [Inject] private SoundsSystem _soundsSystem;
        [Inject] private WindowsSystem _windowsSystem;

        private void Awake()
        {
            Initialize().Forget();
        }

        public async UniTask Play()
        {
            GameContainer.InGame = new Container();
            foreach (var initializer in _gameplayInitializers)
            {
                await initializer.Initialize();
            }
            
            // _soundsSystem.PlayMusic(MusicType.InGame);
        }

        public async UniTask GoToMenu()
        {
            foreach (var initializer in _gameplayInitializers)
            {
                await initializer.Dispose();
            }
            GameContainer.InGame = null;
            
            // _soundsSystem.PlayMusic(MusicType.MainMenu);
            _windowsSystem.CreateWindow<MainMenu>();
        }

        private async UniTask Initialize()
        {
            GameContainer.Common = new Container();
            GameContainer.Common.Register(this);
            foreach (var initializer in _startupInitializers)
            {
                await initializer.Initialize();
            }
            
            GameContainer.InjectToInstance(this);
            // _soundsSystem.PlayMusic(MusicType.MainMenu);
        }
    }
}