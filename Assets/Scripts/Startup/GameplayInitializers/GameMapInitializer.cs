using Cysharp.Threading.Tasks;
using Services.DI;
using Services.WindowsSystem;
using Ui.Windows;
using UnityEngine.SceneManagement;

namespace Startup.GameplayInitializers
{
    public class GameMapInitializer : IInitializer
    {
        private const string SceneToLoad = "Level";
        
        public async UniTask Initialize()
        {
            await SceneManager.LoadSceneAsync(SceneToLoad);

            var windowsSystem = GameContainer.Common.Resolve<WindowsSystem>();
            windowsSystem.CreateWindow<InGameUI>();
        }

        public async UniTask Dispose()
        {
            var windowsSystem = GameContainer.Common.Resolve<WindowsSystem>();
            windowsSystem.DestroyWindow<InGameUI>();
            
            await SceneManager.UnloadSceneAsync(SceneToLoad);
        }
    }
}