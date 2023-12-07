using Cysharp.Threading.Tasks;
using Services.DI;
using Services.WindowsSystem;
using Ui;
using UnityEngine;

namespace Startup.StartupInitializers
{
    public class UiInitializer : IInitializer
    {
        public UniTask Initialize()
        {
            var uiRoot = Object.FindObjectOfType<UiRoot>();
            GameContainer.Common.Register(uiRoot);

            var gameWindows = Resources.Load<GameWindows>("Configs/Game Windows");
            GameContainer.Common.Register(gameWindows);
            
            var windowsSystem = GameContainer.Create<WindowsSystem>();
            GameContainer.Common.Register(windowsSystem);
            
            return UniTask.CompletedTask;
        }

        public UniTask Dispose()
        {
            return UniTask.CompletedTask;
        }
    }
}