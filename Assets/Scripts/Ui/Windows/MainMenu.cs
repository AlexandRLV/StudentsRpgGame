using Cysharp.Threading.Tasks;
using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class MainMenu : WindowBase
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _playButton;
        
        [Inject] private Startup.Startup _startup;
        [Inject] private WindowsSystem _windowsSystem;

        private void Start()
        {
            _settingsButton.onClick.AddListener(OpenSettings);
            _playButton.onClick.AddListener(Play);
        }

        private void OpenSettings()
        {
            _windowsSystem.CreateWindow<SettingsWindow>();
        }

        private void Play()
        {
            _windowsSystem.DestroyWindow(this);
            _startup.Play().Forget();
        }
    }
}