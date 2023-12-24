using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;
using Ui.Windows;

namespace Ui.Extra
{
    public class DisableUiOnOpenInventory : MonoBehaviour
    {
        [Inject] private WindowsSystem _windowsSystem;

        private void OnEnable()
        {
            GameContainer.InjectToInstance(this);
            _windowsSystem.TryGetWindow<InGameUI>(out var inGameUi);
            inGameUi.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _windowsSystem.TryGetWindow<InGameUI>(out var inGameUi);
            inGameUi.gameObject.SetActive(true);
        }
    }
}