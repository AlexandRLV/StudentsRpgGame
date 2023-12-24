using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class InGameUI : WindowBase
    {
        [SerializeField] private Button _pauseButton;

        [Inject] private WindowsSystem _windowsSystem;

        private void Start()
        {
            _pauseButton.onClick.AddListener(Pause);
        }

        private void Pause()
        {
            // _windowsSystem.CreateWindow<PauseWindow>();
            gameObject.SetActive(false);
        }
    }
}