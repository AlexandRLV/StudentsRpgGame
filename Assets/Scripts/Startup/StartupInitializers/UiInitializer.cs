using Cysharp.Threading.Tasks;
using Services.DI;
using Services.WindowsSystem;
using Ui;
using Ui.Windows;
using UnityEngine;

namespace Startup.StartupInitializers
{
    public class UiInitializer : IInitializer
    {
        public UniTask Initialize()
        {
            var uiRoot = Object.FindObjectOfType<UiRoot>();
            Object.DontDestroyOnLoad(uiRoot);
            GameContainer.Common.Register(uiRoot);

            var loadingScreenPrefab = Resources.Load<LoadingScreen>("Prefabs/LoadingScreen");
            var loadingScreen = Object.Instantiate(loadingScreenPrefab, uiRoot.OverlayParent);
            GameContainer.Common.Register(loadingScreen);

            var gameWindows = Resources.Load<GameWindows>("Configs/Game Windows");
            GameContainer.Common.Register(gameWindows);
            
            var windowsSystem = GameContainer.Create<WindowsSystem>();
            GameContainer.Common.Register(windowsSystem);

            windowsSystem.CreateWindow<MainMenu>();
            loadingScreen.Active = false;
            
            return UniTask.CompletedTask;
        }

        public UniTask Dispose()
        {
            return UniTask.CompletedTask;
        }
    }
}