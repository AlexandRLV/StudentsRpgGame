using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private Button _saveButton;
        
        [Inject] private WindowsSystem _windowsSystem;
        
        private void Start()
        {
            _saveButton.onClick.AddListener(() =>
            {
                _windowsSystem.DestroyWindow(this);
            });
        }
    }
}