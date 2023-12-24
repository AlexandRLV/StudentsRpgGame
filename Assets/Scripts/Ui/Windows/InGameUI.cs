using Services.DI;
using Services.WindowsSystem;
using UnityEngine;
using UnityEngine.UI;
using Invector.vItemManager;

namespace Ui.Windows
{
    public class InGameUI : WindowBase
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private vInventory _inventory;

        [Inject] private WindowsSystem _windowsSystem;

        private void Awake()
        {
            //var itemManager = FindObjectOfType<vItemManager>();
            //itemManager.inventory = _inventory;
        }

        private void Start()
        {
            _pauseButton.onClick.AddListener(Pause);
        }

        private void Pause()
        {
            _windowsSystem.CreateWindow<PauseWindow>();
        }
    }
}