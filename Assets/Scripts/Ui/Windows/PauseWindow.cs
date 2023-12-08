using Cysharp.Threading.Tasks;
using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _menuButton;

        [Inject] private WindowsSystem _windowsSystem;
        [Inject] private Startup.Startup _startup;

        private void Start()
        {
            _continueButton.onClick.AddListener(Continue);
            _settingsButton.onClick.AddListener(OpenSettings);
            _menuButton.onClick.AddListener(GoToMenu);
        }

        private void Continue()
        {
            _windowsSystem.DestroyWindow(this);
        }

        private void GoToMenu()
        {
            _windowsSystem.DestroyWindow(this);
            _startup.GoToMenu().Forget();
        }

        private void OpenSettings()
        {
            _windowsSystem.CreateWindow<SettingsWindow>();
        }
    }
}